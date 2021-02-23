module internal Facil.Db

open System
open System.Collections.Generic
open System.Data
open System.IO
open System.Text.RegularExpressions
open Microsoft.Data.SqlClient
open Microsoft.SqlServer.TransactSql.ScriptDom


let adjustSizeForDbType (dbType: SqlDbType) (size: int16) =
  match dbType with
  | SqlDbType.NChar | SqlDbType.NText | SqlDbType.NVarChar -> size / 2s
  | _ -> size


let getSysTypeIdLookup (conn: SqlConnection) =
  try
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "SELECT system_type_id, name FROM sys.types WHERE system_type_id = user_type_id"
    use reader = cmd.ExecuteReader()
    let lookup = ResizeArray()
    while reader.Read() do
      lookup.Add(reader.["system_type_id"] |> unbox<byte> |> int, reader.["name"] |> unbox<string>)
    lookup |> Map.ofSeq
  with ex ->
    raise <| Exception("Error getting system type IDs", ex)


// Prefix temp table names to ensure no collisions with existing global temp tables
let facilGlobalTempTablePrefix = $"""FACIL_TEMP_{Guid.NewGuid().ToString("N")}_"""

let rewriteLocalTempTablesToGlobalTempTablesWithPrefix (nameOrDefinition: string) =
  Regex.Replace(nameOrDefinition, "(?<!#)#(?=\w)", "##")
  |> fun s -> Regex.Replace(s, "##(?=\w)", $"##{facilGlobalTempTablePrefix}")


let createAndDropTempTables (tempTables: TempTable list) (conn: SqlConnection) =
  for tt in tempTables do
    use cmd = conn.CreateCommand()
    cmd.CommandText <- tt.Source |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix
    cmd.ExecuteNonQuery () |> ignore

  { new IDisposable with
      member _.Dispose () =
        // Drop all temp tables
        for tt in tempTables do
          use cmd = conn.CreateCommand()
          cmd.CommandText <- $"DROP TABLE {tt.Name |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix}"
          cmd.ExecuteNonQuery() |> ignore
  }


let getScriptParameters (cfg: RuleSet) (sysTypeIdLookup: Map<int, string>) (tableTypesByUserId: Map<int, TableType>) (script: Script) (conn: SqlConnection) =

  try

    // Prefix temp var names to ensure no collisions
    let facilTempVarPrefix = $"""@FACIL_VARIABLE_{Guid.NewGuid().ToString("N")}_"""

    let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg

    let paramsWithFirstUsageOffset = Dictionary()
    let parser = TSql150Parser(true)
    let fragment, _ = parser.Parse(new StringReader(script.Source))
    fragment.Accept {
      new TSqlFragmentVisitor() with
        member _.Visit(node: VariableReference) =
          base.Visit node
          match paramsWithFirstUsageOffset.TryGetValue node.Name with
          | true, offset when offset < node.StartOffset -> ()
          | _ -> paramsWithFirstUsageOffset.[node.Name] <- node.StartOffset
    }

    let declaredParams = ResizeArray()
    let parser = TSql150Parser(true)
    let fragment, _ = parser.Parse(new StringReader(script.Source))
    fragment.Accept {
      new TSqlFragmentVisitor() with
        member _.Visit(node: DeclareVariableElement) =
          base.Visit node
          declaredParams.Add node.VariableName.Value
    }

    let undeclaredParams = set paramsWithFirstUsageOffset.Keys - set declaredParams

    let sourceToUse =
      // Add parameter declarations from config
      (script.Source, undeclaredParams |> Seq.map (fun s -> s.TrimStart '@'))
      ||> Seq.fold (fun source paramName ->
            match rule |> EffectiveScriptRule.getParam paramName with
            | { Type = Some typeDef } ->
                $"DECLARE @%s{paramName} %s{typeDef} = %s{facilTempVarPrefix}%s{paramName}\n%s{source}"
            | _ -> source
      )
      |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix


    let unusedParamRules =
      (rule |> EffectiveScriptRule.allParamNames |> Set.map (fun s -> "@" + s))
      - (set paramsWithFirstUsageOffset.Keys)

    for paramName in unusedParamRules do
      logWarning $"Script '{script.GlobMatchOutput}' has a matching rule with parameter '%s{paramName}' that is not used in the script. Ignoring parameter."


    use __ = createAndDropTempTables script.TempTables conn
    
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "sys.sp_describe_undeclared_parameters"
    cmd.CommandType <- CommandType.StoredProcedure
    cmd.Parameters.AddWithValue("@tsql", sourceToUse) |> ignore
    use reader = cmd.ExecuteReader()
    let parameters = ResizeArray()
    while reader.Read() do

      let paramName =
        reader.["name"]
        |> unbox<string>
        |> fun s ->
            if s.StartsWith facilTempVarPrefix then
              "@" + s.Substring(facilTempVarPrefix.Length)
            else s

      let typeInfo =
        let userTypeId =
          if reader.IsDBNull "suggested_user_type_id"
          then None
          else reader.["suggested_user_type_id"] |> unbox<int> |> Some

        match userTypeId |> Option.bind tableTypesByUserId.TryFind with
        | Some tt ->
            match rule |> EffectiveScriptRule.getParam (paramName.TrimStart '@') with
            | { Nullable = Some true } ->
                logWarning $"The effective rule for script '{script.GlobMatchOutput}' and parameter '@{paramName}' specifies that the parameter is both nullable and a user-defined table type, but table-valued parameters cannot be nullable. Treating the parameter as non-nullable. To remove this warning, ensure that the parameter does not specify or inherit 'nullable: true'"
            | _ -> ()
            Table tt
        | None ->
            reader.["suggested_system_type_id"]
            |> unbox<int>
            |> fun id ->
                sysTypeIdLookup.TryFind id
                |> Option.defaultWith (fun () -> failwith $"Unsupported SQL system type ID '%i{id}' for parameter '%s{paramName}'")
            |> fun typeName ->
                sqlDbTypeMap.TryFind typeName
                |> Option.defaultWith (fun () -> failwith $"Unsupported SQL type '%s{typeName}' for parameter '%s{paramName}'")
            |> Scalar

      parameters.Add(
        { 
          Name = paramName
          SortKey = paramsWithFirstUsageOffset.[paramName]
          Size =
            reader.["suggested_max_length"]
            |> unbox<int16>
            |> adjustSizeForDbType (match typeInfo with Scalar ti -> ti.SqlDbType | Table _ -> SqlDbType.Structured)
          Precision = reader.["suggested_precision"] |> unbox<byte>
          Scale = reader.["suggested_scale"] |> unbox<byte>
          FSharpDefaultValueString =
            match rule |> EffectiveScriptRule.getParam (paramName.TrimStart '@') with
            | { Nullable = Some true } -> Some "null"
            | _ -> None
          TypeInfo = typeInfo
          IsOutput = reader.["suggested_is_output"] |> unbox<bool>
          IsCursorRef = false
        }
      )

    parameters |> Seq.toList |> List.sortBy (fun p -> p.SortKey)

  with
  | :? SqlException as ex when ex.Message.Contains "Procedure or function" && ex.Message.Contains "has too many arguments specified" ->
      raise <| Exception($"Error getting parameters for script {script.GlobMatchOutput}. If you are using EXEC statements, all parameters passed to the procedure/function you execute may need to be declared in the script or Facil config file.", ex)
  | :? SqlException as ex when ex.Message.StartsWith "Invalid object name '#" ->
      raise <| Exception($"Error getting parameters for script {script.GlobMatchOutput}. If you are using temp tables, you may need to define them in the script's `tempTables` array in the Facil config file.", ex)
  | :? SqlException as ex when ex.Message.Contains "used more than once in the batch being analyzed" ->
      raise <| Exception($"Error getting parameters for script {script.GlobMatchOutput}. Parameters that are used more than once must be specified in the Facil config file.", ex)
  | ex ->
      raise <| Exception($"Error getting parameters for script {script.GlobMatchOutput}", ex)



let getColumnsFromSpDescribeFirstResultSet (cfg: RuleSet) (sysTypeIdLookup: Map<int, string>) (executable: Choice<StoredProcedure, Script, TempTable>) (conn: SqlConnection) =

  let tempTablesToCreateAndDrop =
    match executable with
    | Choice2Of3 script -> script.TempTables
    | Choice1Of3 _ -> []
    | Choice3Of3 tempTable -> [tempTable]

  use __ = createAndDropTempTables tempTablesToCreateAndDrop conn

  use cmd = conn.CreateCommand()
  cmd.CommandText <- "sys.sp_describe_first_result_set"
  cmd.CommandType <- CommandType.StoredProcedure
  match executable with
  | Choice1Of3 sproc -> cmd.Parameters.AddWithValue("@tsql", sproc.SchemaName + "." + sproc.Name) |> ignore
  | Choice2Of3 script ->
      let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
      let sourceToUse =
        (script.Source, (rule |> EffectiveScriptRule.allParams |> Map.toList))
        ||> List.fold (fun source (paramName, p) ->
              match p.Type with
              | None -> source
              | Some typeDef -> $"DECLARE @{paramName} {typeDef}\n{source}"
        )
        |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix
      cmd.Parameters.AddWithValue("@tsql", sourceToUse) |> ignore
  | Choice3Of3 tt -> cmd.Parameters.AddWithValue("@tsql", $"SELECT * FROM {tt.Name |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix}") |> ignore
  use reader = cmd.ExecuteReader()
  let cols = ResizeArray()
  let allColNames = ResizeArray()
  while reader.Read() do
    let colName =
      if reader.IsDBNull "name" then None
      else
        reader.["name"]
        |> unbox<string>
        |> Some
        |> Option.filter (not << String.IsNullOrEmpty)

    colName |> Option.iter allColNames.Add

    let shouldSkipCol =
      match colName, executable with
      | Some name, Choice1Of3 sproc ->
          RuleSet.getEffectiveProcedureRuleFor sproc.SchemaName sproc.Name cfg
          |> EffectiveProcedureRule.getColumn name
          |> fun c -> c.Skip
          |> Option.defaultValue false
      | Some name, Choice2Of3 script ->
          RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
          |> EffectiveScriptRule.getColumn name
          |> fun c -> c.Skip
          |> Option.defaultValue false
      | None, _ | _, Choice3Of3 _ -> false

    if not shouldSkipCol then

      let typeInfo =
        reader.["system_type_id"]
        |> unbox<int>
        |> fun id ->
            sysTypeIdLookup.TryFind id
            |> Option.defaultWith (fun () -> failwith $"""Unsupported SQL system type ID '%i{id}' for column '%s{defaultArg colName "<unnamed column>"}'""")
        |> fun typeName ->
            sqlDbTypeMap.TryFind typeName
            |> Option.defaultWith (fun () -> failwith $"""Unsupported SQL type '%s{typeName}' for column '%s{defaultArg colName "<unnamed column>"}'""")

      cols.Add { 
          Name = colName
          SortKey = reader.["column_ordinal"] |> unbox<int>
          IsNullable = reader.["is_nullable"] |> unbox<bool>
          TypeInfo = typeInfo
      }

  if cols.Count = 0 then
    Seq.toList allColNames, None
  else
    Seq.toList allColNames, Seq.toList cols |> List.sortBy (fun c -> c.SortKey) |> Some


let getColumnsFromQuery (cfg: RuleSet) (executable: Choice<StoredProcedure, Script, TempTable>) (conn: SqlConnection) =
  let tempTablesToCreateAndDrop =
    match executable with
    | Choice2Of3 script -> script.TempTables
    | Choice1Of3 _ -> []
    | Choice3Of3 tempTable -> [tempTable]

  use __ = createAndDropTempTables tempTablesToCreateAndDrop conn

  use cmd = conn.CreateCommand()

  match executable with

  | Choice1Of3 sproc ->
      cmd.CommandText <- sproc.SchemaName + "." + sproc.Name
      cmd.CommandType <- CommandType.StoredProcedure
      let rule = RuleSet.getEffectiveProcedureRuleFor sproc.SchemaName sproc.Name cfg
      for param in sproc.Parameters do
        match param.TypeInfo with
        | Scalar ti ->
            let p = cmd.Parameters.Add(param.Name, ti.SqlDbType)
            rule
            |> EffectiveProcedureRule.getParam (param.Name.TrimStart '@')
            |> fun p -> p.BuildValue
            |> Option.map box
            |> Option.defaultValue ti.DefaultBuildValue
            |> fun v -> p.Value <- if isNull v then box DBNull.Value else v
        | Table tt ->
            cmd.Parameters.Add(param.Name, SqlDbType.Structured, TypeName = $"{tt.SchemaName}.{tt.Name}") |> ignore

  | Choice2Of3 script ->
      cmd.CommandText <- script.Source |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix
      let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
      for param in script.Parameters do
        match param.TypeInfo with
        | Scalar ti ->
            let p = cmd.Parameters.Add(param.Name, ti.SqlDbType)
            rule
            |> EffectiveScriptRule.getParam (param.Name.TrimStart '@')
            |> fun p -> p.BuildValue
            |> Option.map box
            |> Option.defaultValue ti.DefaultBuildValue
            |> fun v -> p.Value <- if isNull v then box DBNull.Value else v
        | Table tt ->
            cmd.Parameters.Add(param.Name, SqlDbType.Structured, TypeName = $"{tt.SchemaName}.{tt.Name}") |> ignore

  | Choice3Of3 tt -> cmd.CommandText <- $"SELECT * FROM {tt.Name |> rewriteLocalTempTablesToGlobalTempTablesWithPrefix}"

  use reader =
    try
      // SET FMTONLY ON, may fail with dynamic SQL
      cmd.ExecuteReader(CommandBehavior.SchemaOnly)
    with :? SqlException ->
      // Actually execute query
      cmd.ExecuteReader(CommandBehavior.SingleRow)

  let schemas = reader.GetColumnSchema()
  if schemas.Count = 0 then [], None
  else
    let cols = ResizeArray()
    let allColNames = ResizeArray()
    for schema in schemas do

      let colName =
        if String.IsNullOrEmpty schema.ColumnName then None
        else Some schema.ColumnName

      colName |> Option.iter allColNames.Add

      let shouldSkipCol =
        match colName, executable with
        | Some name, Choice1Of3 sproc ->
            RuleSet.getEffectiveProcedureRuleFor sproc.SchemaName sproc.Name cfg
            |> EffectiveProcedureRule.getColumn name
            |> fun c -> c.Skip
            |> Option.defaultValue false
        | Some name, Choice2Of3 script ->
            RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
            |> EffectiveScriptRule.getColumn name
            |> fun c -> c.Skip
            |> Option.defaultValue false
        | None, _ | _, Choice3Of3 _ -> false

      if not shouldSkipCol then

        let typeInfo =
          schema.DataTypeName
          |> fun typeName ->
              sqlDbTypeMap.TryFind typeName
              |> Option.defaultWith (fun () -> failwith $"""Unsupported SQL type '%s{typeName}' for column '%s{defaultArg colName "<unnamed column>"}'""")

        cols.Add {
          Name = colName
          SortKey = schema.ColumnOrdinal.Value
          IsNullable = schema.AllowDBNull.Value
          TypeInfo = typeInfo
        }

    Seq.toList allColNames, Seq.toList cols |> List.sortBy (fun c -> c.SortKey) |> Some


let getColumns conn cfg sysTypeIdLookup (executable: Choice<StoredProcedure, Script, TempTable>) =
  let executableName =
    match executable with
    | Choice1Of3 sp -> $"stored procedure %s{sp.SchemaName}.%s{sp.Name}"
    | Choice2Of3 s -> $"script {s.GlobMatchOutput}"
    | Choice3Of3 tt -> $"temp table {tt.Name}"

  let allColNames, cols =
    try
      try getColumnsFromSpDescribeFirstResultSet cfg sysTypeIdLookup executable conn
      with :? SqlException -> getColumnsFromQuery cfg executable conn
    with ex ->
      raise <| Exception($"Error getting output columns for {executableName}", ex)

  let allColumnNamesWithRules =
    match executable with
    | Choice1Of3 sproc ->
        RuleSet.getEffectiveProcedureRuleFor sproc.SchemaName sproc.Name cfg
        |> EffectiveProcedureRule.allColumnNames
    | Choice2Of3 script ->
        RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
        |> EffectiveScriptRule.allColumnNames
    | Choice3Of3 _ -> Set.empty

  for unmatchedColumn in allColumnNamesWithRules - set allColNames do
    logWarning $"Config contains unmatched rule for column '%s{unmatchedColumn}' in {executableName}"

  cols


let getTableTypes (conn: SqlConnection) =
  try
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "
      SELECT
        sys.table_types.user_type_id AS TableTypeUserTypeId,
        SCHEMA_NAME(sys.table_types.schema_id) AS TableTypeSchemaName,
        sys.table_types.name AS TableTypeName,
        sys.columns.name AS ColumnName,
        sys.columns.column_id AS ColumnId,
        sys.columns.max_length AS ColumnSize,
        sys.columns.precision AS ColumnPrecision,
        sys.columns.scale AS ColumnScale,
        sys.columns.is_nullable AS ColumnIsNullable,
        TYPE_NAME(sys.columns.system_type_id) AS ColumnTypeName
      FROM
        sys.table_types
      INNER JOIN
        sys.columns
          ON sys.columns.object_id = sys.table_types.type_table_object_id
    "
    use reader = cmd.ExecuteReader()
    let tableTypes = ResizeArray()
    while reader.Read() do
      let colName = reader.["ColumnName"] |> unbox<string>
      let typeInfo =
        reader.["ColumnTypeName"]
        |> unbox<string>
        |> fun typeName ->
            sqlDbTypeMap.TryFind typeName
            |> Option.defaultWith (fun () -> failwith $"Unsupported SQL type '%s{typeName}' for column '%s{colName}'")
      tableTypes.Add { 
          UserTypeId = reader.["TableTypeUserTypeId"] |> unbox<int>
          SchemaName = reader.["TableTypeSchemaName"] |> unbox<string>
          Name = reader.["TableTypeName"] |> unbox<string>
          // Merged later
          Columns = [
            {
              Name = colName
              IsNullable = reader.["ColumnIsNullable"] |> unbox<bool>
              SortKey = reader.["ColumnId"] |> unbox<int>
              Size = reader.["ColumnSize"] |> unbox<int16> |> adjustSizeForDbType typeInfo.SqlDbType
              Precision = reader.["ColumnPrecision"] |> unbox<byte>
              Scale = reader.["ColumnScale"] |> unbox<byte>
              TypeInfo = typeInfo
            }
          ]
      }

    tableTypes
    |> Seq.toList
    |> List.groupBy (fun t -> t.UserTypeId)
    |> List.map (fun (_, ts) ->
        { ts.Head with
            Columns =
              ts
              |> List.collect (fun t -> t.Columns)
              |> List.sortBy (fun c -> c.SortKey)
        }
    )
  with ex ->
    raise <| Exception("Error getting table types", ex)


let getStoredProcedures cfg sysTypeIdLookup (tableTypesByUserId: Map<int, TableType>) (conn: SqlConnection) =

  let getStoredProceduresWithoutParamsOrResultSet () =
    try
      use cmd = conn.CreateCommand()
      cmd.CommandText <- "
        SELECT
          object_id,
          SCHEMA_NAME(schema_id) AS SchemaName,
          name,
          OBJECT_DEFINITION(object_id) AS [Definition]
        FROM
          sys.objects
        WHERE
          [Type] = 'P'
      "
      use reader = cmd.ExecuteReader()
      let sprocs = ResizeArray()
      while reader.Read() do
        let schemaName = reader.["SchemaName"] |> unbox<string>
        let name = reader.["name"] |> unbox<string>

        sprocs.Add(
          { 
            ObjectId = reader.["object_id"] |> unbox<int>
            SchemaName = schemaName
            Name = name
            Definition =
              if reader.IsDBNull "Definition" then
                failwith $"Unable to get definition of procedure {schemaName}.{name}. Ensure the current principal has the VIEW DEFINITION permission on the procedure."
              else
                reader.["Definition"] |> unbox<string>
            Parameters = []  // Added later
            ResultSet = None  // Added later
          }
        )

      sprocs |> Seq.toList
    with ex ->
      raise <| Exception("Error getting stored procedures", ex)

  let getSprocParamsByObjectId () =
    try
      use cmd = conn.CreateCommand()
      cmd.CommandText <- "
        SELECT
          object_id,
          OBJECT_NAME(object_id) AS SprocName,
          name,
          parameter_id,
          user_type_id,
          max_length,
          precision,
          scale,
          is_output,
          is_cursor_ref,
          TYPE_NAME(system_type_id) AS SystemTypeName
        FROM
          sys.parameters
      "
      use reader = cmd.ExecuteReader()
      let parameters = ResizeArray()
      while reader.Read() do

        let paramName = reader.["name"] |> unbox<string>
        let sprocName = reader.["SprocName"] |> unbox<string>

        let typeInfo =
          match reader.["SystemTypeName"] |> unbox<string> with
          | "table type" ->
              let userTypeId = reader.["user_type_id"] |> unbox<int>
              tableTypesByUserId.TryFind userTypeId
              |> Option.defaultWith (fun () -> failwith $"Unknown user type ID '%i{userTypeId}' for table type parameter '%s{paramName}' in stored procedure '%s{sprocName}'")
              |> Table
          | typeName ->
              sqlDbTypeMap.TryFind typeName
              |> Option.defaultWith (fun () -> failwith $"Unsupported SQL type '%s{typeName}' for parameter '%s{paramName}' in stored procedure '%s{sprocName}'")
              |> Scalar

        parameters.Add(
          reader.["object_id"] |> unbox<int>,
          { 
            Name = reader.["name"] |> unbox<string>
            SortKey = reader.["parameter_id"] |> unbox<int>
            Size = 
              reader.["max_length"]
              |> unbox<int16>
              |> adjustSizeForDbType (match typeInfo with Scalar ti -> ti.SqlDbType | Table _ -> SqlDbType.Structured)
            Precision = reader.["precision"] |> unbox<byte>
            Scale = reader.["scale"] |> unbox<byte>
            FSharpDefaultValueString = None  // Added later
            TypeInfo = typeInfo
            IsOutput = reader.["is_output"] |> unbox<bool>
            IsCursorRef = reader.["is_cursor_ref"] |> unbox<bool>
          }
        )

      parameters
      |> Seq.toList 
      |> List.groupBy fst 
      |> List.map (fun (k, ps) -> k, ps |> List.map snd |> List.sortBy (fun p -> p.SortKey)) 
      |> Map.ofList

    with ex ->
      raise <| Exception("Error getting stored procedure parameters", ex)

  let sprocsWithoutParamsOrResultSet = getStoredProceduresWithoutParamsOrResultSet ()

  let sprocParamsByObjectId = getSprocParamsByObjectId ()

  sprocsWithoutParamsOrResultSet
  |> List.filter (fun sp -> RuleSet.shouldIncludeProcedure sp.SchemaName sp.Name cfg)
  // Add parameters
  |> List.map (fun sproc ->
      { sproc with
          Parameters =
            sprocParamsByObjectId.TryFind sproc.ObjectId
            |> Option.defaultValue []
      }
  )
  // Add parameter default values
  |> List.map (fun sproc ->
      let paramDefaults = getParameterDefaultValues sproc
      { sproc with
          Parameters =
            sproc.Parameters
            |> List.map (fun param ->
                { param with 
                    FSharpDefaultValueString =
                      match paramDefaults.TryGetValue param.Name with
                      | false, _ | true, None -> None
                      | true, Some null -> Some "null"
                      | true, Some x -> sprintf "%A" x |> Some
                }
            )
      }
  )
  // Add result sets
  |> List.map (fun sproc ->
      { sproc with ResultSet = getColumns conn cfg sysTypeIdLookup (Choice1Of3 sproc) }
  )


let getTableDtos cfg (sysTypeIdLookup: Map<int, string>) (conn: SqlConnection) =
  try
    use cmd = conn.CreateCommand()
    cmd.CommandText <- "
      SELECT
        SCHEMA_NAME(sys.tables.schema_id) AS SchemaName,
        sys.tables.name AS TableName,
        sys.all_columns.name AS ColName,
        sys.all_columns.column_id,
        sys.all_columns.is_nullable,
        sys.all_columns.system_type_id
      FROM
        sys.tables
      INNER JOIN
        sys.all_columns
          ON sys.all_columns.object_id = sys.tables.object_id
    "
    use reader = cmd.ExecuteReader()
    let tableDtos = ResizeArray()
    let allColumnsByTableSchemaAndName = Dictionary<string, ResizeArray<string>>()
    while reader.Read() do

      let schemaName = reader.["SchemaName"] |> unbox<string>
      let tableName = reader.["TableName"] |> unbox<string>
      let colName = reader.["ColName"] |> unbox<string>

      let key = $"{schemaName}.{tableName}"
      match allColumnsByTableSchemaAndName.TryGetValue key with
      | false, _->
          let r = ResizeArray()
          r.Add colName
          allColumnsByTableSchemaAndName.[key] <- r
      | true, names -> names.Add colName

      let shouldSkipCol =
        RuleSet.getEffectiveTableDtoRuleFor schemaName tableName cfg
        |> EffectiveTableDtoRule.getColumn colName
        |> fun c -> c.Skip
        |> Option.defaultValue false

      if not shouldSkipCol then

        let typeInfo =
          reader.["system_type_id"]
          |> unbox<byte>
          |> int
          |> fun id ->
              sysTypeIdLookup.TryFind id
              |> Option.defaultWith (fun () -> failwith $"Unsupported SQL system type ID '%i{id}' for column '%s{colName}' in table '%s{tableName}'")
          |> fun typeName ->
              sqlDbTypeMap.TryFind typeName
              |> Option.defaultWith (fun () -> failwith $"Unsupported SQL type '%s{typeName}' for column '%s{colName}' in table '%s{tableName}'")

        tableDtos.Add(
          { 
            SchemaName = schemaName
            Name = tableName
            // Merged later
            Columns = [
              {
                OutputColumn.Name = colName |> Some
                SortKey = reader.["column_id"] |> unbox<int>
                IsNullable = reader.["is_nullable"] |> unbox<bool>
                TypeInfo = typeInfo
              }
            ]
          }
        )

    tableDtos
    |> Seq.toList
    |> List.groupBy (fun dto -> dto.SchemaName, dto.Name)
    |> List.map (fun ((schemaName, tableName), xs) ->

        let allColumnNamesWithRules =
          RuleSet.getEffectiveTableDtoRuleFor schemaName tableName cfg
          |> EffectiveTableDtoRule.allColumnNames

        let key = $"{schemaName}.{tableName}"
        for unmatchedColumn in allColumnNamesWithRules - set allColumnsByTableSchemaAndName.[key] do
          logWarning $"Config contains unmatched rule for column '%s{unmatchedColumn}' in table {schemaName}.{tableName}"

        { 
          SchemaName = schemaName
          Name = tableName
          Columns = xs |> List.collect (fun x -> x.Columns) |> List.sortBy (fun c -> c.SortKey)
        }
    )
  with ex ->
    raise <| Exception("Error getting table DTOs", ex)



let getTempTable cfg (sysTypeIdLookup: Map<int, string>) definition (conn: SqlConnection) =
  try
    let mutable name = null
    let parser = TSql150Parser(true)
    let fragment, _ = parser.Parse(new StringReader(definition))
    fragment.Accept {
      new TSqlFragmentVisitor() with
        member _.Visit(node: CreateTableStatement) =
          if not (isNull name) then failwith "Temp table definition must not contain multiple CREATE TABLE statements"
          base.Visit node
          name <- node.SchemaObjectName.BaseIdentifier.Value
    }

    if isNull name then failwith "No CREATE TABLE statement was found in temp table definition"

    let tempTableWithoutColumns =
      {
        Name = name
        Source = definition
        Columns = []
      }

    // Create table so we can query it to get columns
    use cmd = conn.CreateCommand()
    cmd.CommandText <- definition
    cmd.ExecuteNonQuery() |> ignore

    let tempTable =
      { tempTableWithoutColumns with
          Columns = getColumns conn cfg sysTypeIdLookup (Choice3Of3 tempTableWithoutColumns) |> Option.defaultValue []
      }

    // Drop table in case other temp tables use the same name
    use cmd = conn.CreateCommand()
    cmd.CommandText <- $"DROP TABLE {tempTable.Name}"
    cmd.ExecuteNonQuery() |> ignore

    tempTable
  with ex ->
    raise <| Exception($"Error getting temp table from the following definition:\n%s{definition}", ex)



let getEverything (cfg: RuleSet) fullYamlPath (scriptsWithoutParamsOrResultSetsOrTempTables: Script list) (conn: SqlConnection) =

  let sysTypeIdLookup = getSysTypeIdLookup conn

  let tableTypes = getTableTypes conn

  let tableTypesByUserId = tableTypes |> List.map (fun t -> t.UserTypeId, t) |> Map.ofList

  let tempTablesByDefinition =
    cfg.Scripts
    |> List.collect (fun s -> s.TempTables |> Option.defaultValue [])
    |> List.map (fun rule -> rule.Definition)
    |> List.distinct
    |> List.map (fun definition -> definition, getTempTable cfg sysTypeIdLookup definition conn)
    |> Map.ofList

  let scripts =
    scriptsWithoutParamsOrResultSetsOrTempTables
    |> List.map (fun script ->
        let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg
        let tempTables = rule.TempTables |> List.map (fun tt -> tempTablesByDefinition.[tt.Definition])

        if tempTables |> List.countBy (fun tt -> tt.Name) |> List.exists (fun (_, count) -> count > 1) then
          failwithError $"The rule for script '%s{script.GlobMatchOutput}' contains multiple temp table definitions using the same temp table name. This is not supported."

        let paramNames = script.Parameters |> List.map (fun p -> p.FSharpParamName |> String.firstLower) |> set
        let tempTableNames = tempTables |> List.map (fun tt -> tt.FSharpName |> String.firstLower) |> set
        if Set.intersect paramNames tempTableNames |> Set.isEmpty |> not then
          failwithError $"Script '%s{script.GlobMatchOutput}' has a temp table with the same name as a parameter. This is not supported."

        { script with TempTables = tempTables }
    )
    |> List.map (fun script ->
        let parameters = getScriptParameters cfg sysTypeIdLookup tableTypesByUserId script conn
        { script with Parameters = parameters }
    )
    |> List.map (fun script ->
        let resultSet = getColumns conn cfg sysTypeIdLookup (Choice2Of3 script)
        { script with ResultSet = resultSet }
    )
    |> List.filter (fun s ->

        let hasUnsupportedParameter =
          s.Parameters |> List.exists (fun p ->
            if p.IsOutput then
              logWarning $"Parameter '%s{p.Name}' in script '%s{s.GlobMatchOutput}' is an output parameter, which is not supported. Ignoring script. To remove this warning, make sure this script is not included in any rules."
              true

            elif p.IsCursorRef then
              logWarning $"Parameter '%s{p.Name}' in script '%s{s.GlobMatchOutput}' is a cursor reference, which is not supported. Ignoring script. To remove this warning, make sure this script is not included in any rules."
              true

            else false
          )

        let hasDuplicateColumnNames =
          match s.ResultSet with
          | None -> false
          | Some cols ->
              match cols |> List.choose (fun c -> c.Name) |> List.countBy id |> List.filter (fun (_, c) -> c > 1) with
              | (name, count) :: _ ->
                  logWarning $"Script '%s{s.GlobMatchOutput}' returns %i{count} columns named '{name}'. Columns names must be unique. Ignoring script. To remove this warning, fix the column names or make sure this script is not included in any rules."
                  true
              | _ -> false

        not hasUnsupportedParameter && not hasDuplicateColumnNames
    )
    |> List.sortBy (fun s -> s.GlobMatchOutput)
  
  let sprocs =
    if cfg.Procedures.IsEmpty then []
    else
      getStoredProcedures cfg sysTypeIdLookup tableTypesByUserId conn
      |> List.filter (fun sp -> RuleSet.shouldIncludeProcedure sp.SchemaName sp.Name cfg)
      |> List.filter (fun sp ->

          let hasUnsupportedParameter =
            sp.Parameters |> List.exists (fun p ->
              if p.IsCursorRef then
                logWarning $"Parameter '%s{p.Name}' in stored procedure '%s{sp.SchemaName}.%s{sp.Name}' is a cursor reference, which is not supported. Ignoring stored procedure. To remove this warning, remove the parameter from the stored procedure or make the procedure is not included in any rules."
                true
              else false
            )

          let hasUnsupportedResultColumn =
            match sp.ResultSet with
            | None -> false
            | Some cols ->
                match cols |> List.tryFindIndex (fun c -> c.Name.IsNone) with
                | Some idx when idx > 0 || cols.Length > 1 ->
                    logWarning $"Column #{idx + 1} of {cols.Length} returned by stored procedure '{sp.SchemaName}.{sp.Name}' is missing a name. Columns without names are only supported if they are the only column in the result set. Ignoring stored procedure. To remove this warning, fix the result set make sure this stored procedure is not included in any rules."
                    true
                | _ -> false

          let hasDuplicateColumnNames =
            match sp.ResultSet with
            | None -> false
            | Some cols ->
                match cols |> List.choose (fun c -> c.Name) |> List.countBy id |> List.filter (fun (_, c) -> c > 1) with
                | (name, count) :: _ ->
                    logWarning $"Stored procedure '{sp.SchemaName}.{sp.Name}' returns %i{count} columns named '{name}'. Columns names must be unique. Ignoring stored procedure. To remove this warning, fix the column names or make sure this stored procedure is not included in any rules."
                    true
                | _ -> false

          not hasUnsupportedParameter && not hasUnsupportedResultColumn && not hasDuplicateColumnNames
      )
      |> List.sortBy (fun sp -> sp.SchemaName, sp.Name)

  let usedTableTypes =
    [
      yield! sprocs |> List.collect (fun sp -> sp.Parameters)
      yield! scripts |> List.collect (fun s -> s.Parameters)
    ]
    |> List.choose (fun p ->
        match p.TypeInfo with
        | Table tt -> Some (tt.SchemaName, tt.Name)
        | _ -> None
    )
    |> set

  let tableTypes =
    tableTypes
    |> List.filter (fun tt -> usedTableTypes |> Set.contains (tt.SchemaName, tt.Name))
    |> List.sortBy (fun tt -> tt.SchemaName, tt.Name)

  let tableDtos =
    getTableDtos cfg sysTypeIdLookup conn
    |> List.filter (fun dto -> RuleSet.shouldIncludeTableDto dto.SchemaName dto.Name cfg)
    |> List.sortBy (fun dto -> dto.SchemaName, dto.Name)


  for i, rule in Seq.indexed cfg.TableDtos do
    let matchesAnything = tableDtos |> List.exists (fun dto -> rule |> TableDtoRule.matches dto.SchemaName dto.Name)
    if not matchesAnything then
      let includeForExceptExpr =
        match rule.IncludeOrFor, rule.Except with
        | Include pattern, None -> $"include = '%s{pattern}'"
        | Include pattern, Some except -> $"include = '%s{pattern}' and except = '%s{except}'"
        | For pattern, None -> $"for = '%s{pattern}'"
        | For pattern, Some except -> $"for = '%s{pattern}' and except = '%s{except}'"
      logYamlWarning fullYamlPath 0 0 $"Table DTO rule at index %i{i} with %s{includeForExceptExpr} does not match any included table DTOs"


  for i, rule in Seq.indexed cfg.TableTypes do
    let matchesAnything = tableTypes |> List.exists (fun tt -> rule |> TableTypeRule.matches tt.SchemaName tt.Name)
    if not matchesAnything then
      let includeForExpr =
        match rule.Except with
        | None -> $"for = '%s{rule.For}'"
        | Some except -> $"for = '%s{rule.For}' and except = '%s{except}'"
      logYamlWarning fullYamlPath 0 0 $"Table type rule at index %i{i} with %s{includeForExpr} does not match any included table types"


  for i, rule in Seq.indexed cfg.Procedures do
    let matchesAnything = sprocs |> List.exists (fun sp -> rule |> ProcedureRule.matches sp.SchemaName sp.Name)
    if not matchesAnything then
      let includeForExceptExpr =
        match rule.IncludeOrFor, rule.Except with
        | Include pattern, None -> $"include = '%s{pattern}'"
        | Include pattern, Some except -> $"include = '%s{pattern}' and except = '%s{except}'"
        | For pattern, None -> $"for = '%s{pattern}'"
        | For pattern, Some except -> $"for = '%s{pattern}' and except = '%s{except}'"
      logYamlWarning fullYamlPath 0 0 $"Procedure rule at index %i{i} with %s{includeForExceptExpr} does not match any included procedures"


  for i, rule in Seq.indexed cfg.Scripts do
    let matchesAnything = scripts |> List.exists (fun s -> rule |> ScriptRule.matches s.GlobMatchOutput)
    if not matchesAnything then
      let includeForExceptExpr =
        match rule.IncludeOrFor, rule.Except with
        | Include pattern, None -> $"include = '%s{pattern}'"
        | Include pattern, Some except -> $"include = '%s{pattern}' and except = '%s{except}'"
        | For pattern, None -> $"for = '%s{pattern}'"
        | For pattern, Some except -> $"for = '%s{pattern}' and except = '%s{except}'"
      logYamlWarning fullYamlPath 0 0 $"Script rule at index %i{i} with %s{includeForExceptExpr} does not match any included scripts"


  for i, rule in Seq.indexed cfg.Procedures do
    for paramName in rule.Parameters |> Map.toList |> List.choose fst do
      let hasMatchingProcedureAndParam =
        sprocs
        |> List.exists (fun sp ->
            rule |> ProcedureRule.matches sp.SchemaName sp.Name
            && sp.Parameters |> List.exists (fun p -> p.FSharpParamName = paramName)
        )
      if not hasMatchingProcedureAndParam then
        let includeForExceptExpr =
          match rule.IncludeOrFor, rule.Except with
          | Include pattern, None -> $"include = '%s{pattern}'"
          | Include pattern, Some except -> $"include = '%s{pattern}' and except = '%s{except}'"
          | For pattern, None -> $"for = '%s{pattern}'"
          | For pattern, Some except -> $"for = '%s{pattern}' and except = '%s{except}'"
        logYamlWarning fullYamlPath 0 0 $"Procedure rule at index %i{i} with %s{includeForExceptExpr} has a rule for parameter '%s{paramName}', but the parameter does not exist in any matching procedures"


  for i, rule in Seq.indexed cfg.Scripts do
    for paramName in rule.Parameters |> Map.toList |> List.choose fst do
      let hasMatchingScriptAndParam =
        scripts
        |> List.exists (fun s ->
            rule |> ScriptRule.matches s.GlobMatchOutput
            && s.Parameters |> List.exists (fun p -> p.FSharpParamName = paramName)
        )
      if not hasMatchingScriptAndParam then
        let includeForExceptExpr =
          match rule.IncludeOrFor, rule.Except with
          | Include pattern, None -> $"include = '%s{pattern}'"
          | Include pattern, Some except -> $"include = '%s{pattern}' and except = '%s{except}'"
          | For pattern, None -> $"for = '%s{pattern}'"
          | For pattern, Some except -> $"for = '%s{pattern}' and except = '%s{except}'"
        logYamlWarning fullYamlPath 0 0 $"Script rule at index %i{i} with %s{includeForExceptExpr} has a rule for parameter '%s{paramName}', but the parameter does not exist in any matching scripts"


  {
    TableDtos = tableDtos
    TableTypes = tableTypes
    StoredProcedures = sprocs
    Scripts = scripts
  }
