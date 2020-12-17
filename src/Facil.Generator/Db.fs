module internal Facil.Db

open System
open System.Collections.Generic
open System.Data
open System.IO
open Microsoft.Data.SqlClient
open Microsoft.SqlServer.TransactSql.ScriptDom
open System.Text.RegularExpressions


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


let getScriptParameters (cfg: RuleSet) (sysTypeIdLookup: Map<int, string>) (tableTypesByUserId: Map<int, TableType>) (script: Script) (conn: SqlConnection) =

  try

    let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg

    // Use a GUID in the temp var names to ensure no collisions
    let facilTempVarPrefix = $"""@FACIL_VARIABLE_{Guid.NewGuid().ToString("N")}_"""

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

    let sourceToUse =
      (script.Source, (Map.toList rule.Parameters))
      ||> List.fold (fun source (paramName, p) ->
            if not (paramsWithFirstUsageOffset.ContainsKey $"@{paramName}") then
              logWarning $"Script '{script.GlobMatchOutput}' has a matching rule with parameter '@{paramName}' that is not used in the script. Ignoring parameter."
              source
            else
              match p.Type with
              | None -> source
              | Some typeDef -> $"DECLARE @{paramName} {typeDef} = {facilTempVarPrefix}{paramName}\n{source}"
      )
    
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
            match rule.Parameters.TryFind (paramName.TrimStart '@') with
            | Some paramRule when paramRule.Nullable = Some true ->
                logWarning $"The effective rule for script '{script.GlobMatchOutput}' and parameter '@{paramName}' specifies that the parameter is both nullable and a user-defined table type, but table-valued parameters cannot be nullable. Treating the parameter as non-nullable. To remove this warning, ensure that the parameter does not specify or inherit 'nullable: true'"
            | _ -> ()
            Table tt
        | None ->
            reader.["suggested_system_type_id"]
            |> unbox<int>
            |> fun id ->
                sysTypeIdLookup.TryFind id
                |> Option.defaultWith (fun () -> failwithf "Unknown SQL system type ID: %i" id)
            |> fun typeName ->
                sqlDbTypeMap.TryFind typeName
                |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type '%s' for parameter '%s' in script '%s'" typeName paramName script.NameWithoutExtension)
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
            match rule.Parameters.TryFind (paramName.TrimStart '@') with
            | Some { Nullable = Some true } -> Some "null"
            | _ -> None
          TypeInfo = typeInfo
          IsOutput = reader.["suggested_is_output"] |> unbox<bool>
          IsCursorRef = false
        }
      )

    parameters |> Seq.toList

  with ex ->
    raise <| Exception($"Error getting parameters for script {script.GlobMatchOutput}", ex)



let getColumnsFromSpDescribeFirstResultSet (sysTypeIdLookup: Map<int, string>) (executable: Choice<StoredProcedure, Script, TempTable>) (conn: SqlConnection) =
  use cmd = conn.CreateCommand()
  cmd.CommandText <- "sys.sp_describe_first_result_set"
  cmd.CommandType <- CommandType.StoredProcedure
  match executable with
  | Choice1Of3 sproc -> cmd.Parameters.AddWithValue("@tsql", sproc.SchemaName + "." + sproc.Name) |> ignore
  | Choice2Of3 script -> cmd.Parameters.AddWithValue("@tsql", script.Source) |> ignore
  | Choice3Of3 temp -> cmd.Parameters.AddWithValue("@tsql", $"SELECT * FROM #{temp.Name}") |> ignore
  use reader = cmd.ExecuteReader()
  let cols = ResizeArray()
  while reader.Read() do
    let typeInfo =
      reader.["system_type_id"]
      |> unbox<int>
      |> fun id ->
          sysTypeIdLookup.TryFind id
          |> Option.defaultWith (fun () -> failwithf "Unknown SQL system type ID: %i" id)
      |> fun typeName ->
          sqlDbTypeMap.TryFind typeName
          |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type: %s" typeName)
    cols.Add { 
        OutputColumn.Name =
          if reader.IsDBNull "name" then None
          else
            reader.["name"]
            |> unbox<string>
            |> Some
            |> Option.filter (not << String.IsNullOrEmpty)
        SortKey = reader.["column_ordinal"] |> unbox<int>
        IsNullable = reader.["is_nullable"] |> unbox<bool>
        TypeInfo = typeInfo
    }
  if cols.Count = 0 then None else Seq.toList cols |> List.sortBy (fun c -> c.SortKey) |> Some


let getColumnsFromSetFmtOnlyOn (executable: Choice<StoredProcedure, Script, TempTable>) (conn: SqlConnection) =
  use cmd = conn.CreateCommand()
  match executable with
  | Choice1Of3 sproc ->
      cmd.CommandText <- sproc.SchemaName + "." + sproc.Name
      cmd.CommandType <- CommandType.StoredProcedure
      for param in sproc.Parameters do
        match param.TypeInfo with
        | Scalar ti -> cmd.Parameters.Add(param.Name, ti.SqlDbType) |> ignore
        | Table tt -> cmd.Parameters.Add(param.Name, SqlDbType.Structured, TypeName = $"{tt.SchemaName}.{tt.Name}") |> ignore
  | Choice2Of3 script ->
      cmd.CommandText <- script.Source
      for param in script.Parameters do
        match param.TypeInfo with
        | Scalar ti -> cmd.Parameters.Add(param.Name, ti.SqlDbType) |> ignore
        | Table tt -> cmd.Parameters.Add(param.Name, SqlDbType.Structured, TypeName = $"{tt.SchemaName}.{tt.Name}") |> ignore
  | Choice3Of3 temp ->
      cmd.CommandText <- $"SELECT * FROM #{temp.Name}"
  use reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly)
  match reader.GetSchemaTable() with
  | null -> None
  | table ->
      let cols = ResizeArray()
      for row in table.Rows do
        let typeInfo =
          row.["DataTypeName"]
          |> unbox<string>
          |> fun typeName ->
              sqlDbTypeMap.TryFind typeName
              |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type: %s" typeName)
        cols.Add {
          OutputColumn.Name =
            if row.IsNull "ColumnName" then None
            else
              row.["ColumnName"]
              |> unbox<string>
              |> Some
              |> Option.filter (not << String.IsNullOrEmpty)
          SortKey = row.["ColumnOrdinal"] |> unbox<int>
          IsNullable = row.["AllowDBNull"] |> unbox<bool>
          TypeInfo = typeInfo
        }
      Seq.toList cols |> List.sortBy (fun c -> c.SortKey) |> Some


let getColumns (conn: SqlConnection) sysTypeIdLookup executable =
  try
    try getColumnsFromSpDescribeFirstResultSet sysTypeIdLookup executable conn
    with :? SqlException -> getColumnsFromSetFmtOnlyOn executable conn
  with ex ->
    let executableName =
      match executable with
      | Choice1Of3 sp -> $"stored procedure %s{sp.SchemaName}.%s{sp.Name}"
      | Choice2Of3 s -> $"script {s.GlobMatchOutput}"
      | Choice3Of3 t -> $"temp table {t.Name}"
    raise <| Exception($"Error getting output columns for {executableName}", ex)


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
      let typeInfo =
        reader.["ColumnTypeName"]
        |> unbox<string>
        |> fun typeName ->
            sqlDbTypeMap.TryFind typeName
            |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type: %s" typeName)
      tableTypes.Add { 
          UserTypeId = reader.["TableTypeUserTypeId"] |> unbox<int>
          SchemaName = reader.["TableTypeSchemaName"] |> unbox<string>
          Name = reader.["TableTypeName"] |> unbox<string>
          Columns = [
            {
              Name = reader.["ColumnName"] |> unbox<string>
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


let getStoredProcedures sysTypeIdLookup (tableTypesByUserId: Map<int, TableType>) (conn: SqlConnection) =

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
        sprocs.Add(
          { 
            ObjectId = reader.["object_id"] |> unbox<int>
            SchemaName = reader.["SchemaName"] |> unbox<string>
            Name = reader.["name"] |> unbox<string>
            Definition = reader.["Definition"] |> unbox<string>
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
              |> Option.defaultWith (fun () -> failwithf "Unknown user type ID '%i' for table type parameter '%s' in stored procedure '%s'" userTypeId paramName sprocName)
              |> Table
          | typeName ->
              sqlDbTypeMap.TryFind typeName
              |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type '%s' for parameter '%s' in stored procedure '%s'" typeName paramName sprocName)
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
      { sproc with ResultSet = getColumns conn sysTypeIdLookup (Choice1Of3 sproc) }
  )


let getTableDtos (sysTypeIdLookup: Map<int, string>) (conn: SqlConnection) =
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
    while reader.Read() do
      let typeInfo =
        reader.["system_type_id"]
        |> unbox<byte>
        |> int
        |> fun id ->
            sysTypeIdLookup.TryFind id
            |> Option.defaultWith (fun () -> failwithf "Unknown SQL system type ID: %i" id)
        |> fun typeName ->
            sqlDbTypeMap.TryFind typeName
            |> Option.defaultWith (fun () -> failwithf "Unsupported SQL type: %s" typeName)
      tableDtos.Add(
        { 
          SchemaName = reader.["SchemaName"] |> unbox<string>
          Name = reader.["TableName"] |> unbox<string>
          // Merged later
          Columns = [
            {
              OutputColumn.Name = reader.["ColName"] |> unbox<string> |> Some
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
        { 
          SchemaName = schemaName
          Name = tableName
          Columns = xs |> List.collect (fun x -> x.Columns) |> List.sortBy (fun c -> c.SortKey)
        }
    )
  with ex ->
    raise <| Exception("Error getting table DTOs", ex)


let getTempTable (fullYamlPath : string) (cfg: RuleSet) sysTypeIdLookup (script: Script) (conn: SqlConnection) =
  let rule = RuleSet.getEffectiveScriptRuleFor script.GlobMatchOutput cfg

  match rule.TempTable with
  | Some tempTablePath ->
    let tempTable =
      let projectDir = Path.GetDirectoryName(fullYamlPath)
      let path = Path.Combine(projectDir, tempTablePath.Replace("/", "\\"))
      let source = File.ReadAllText (path)

      { TempTable.Name = Regex("(#[a-z0-9\\-_]+)", RegexOptions.IgnoreCase).Match(source).Groups.[1].Value
        Source = source
        Columns = []}

    use cmd = conn.CreateCommand()
    cmd.CommandText <-
      let src = tempTable.Source.Replace(tempTable.Name, "#" + tempTable.Name)
      $"IF OBJECT_ID('tempdb.dbo.#{tempTable.Name}', 'U') IS NOT NULL DROP TABLE #{tempTable.Name};\n\r{src}"

    cmd.ExecuteNonQuery() |> ignore

    { tempTable with
        Columns = getColumns conn sysTypeIdLookup (Choice3Of3 tempTable) |> Option.defaultValue [] }

    |> Some

  | _ ->
    None

let getEverything (cfg: RuleSet) fullYamlPath (scriptsWithoutParamsOrResultSets: Script list) (conn: SqlConnection) =

  let sysTypeIdLookup = getSysTypeIdLookup conn

  let tableTypes = getTableTypes conn

  let tableTypesByUserId = tableTypes |> List.map (fun t -> t.UserTypeId, t) |> Map.ofList

  let scripts =
    scriptsWithoutParamsOrResultSets
    |> List.map(fun script ->
        match getTempTable fullYamlPath cfg sysTypeIdLookup script conn with
        | Some tempTable ->
          { script with
              TempTable = Some tempTable
              Source = script.Source.Replace(tempTable.Name, "#" + tempTable.Name) }
        | _ -> script
    )
    |> List.map (fun script ->
        let parameters = getScriptParameters cfg sysTypeIdLookup tableTypesByUserId script conn
        { script with Parameters = parameters }
    )
    |> List.map (fun script ->
        let resultSet = getColumns conn sysTypeIdLookup (Choice2Of3 script)
        { script with ResultSet = resultSet }
    )
    |> List.map(fun script ->
        // Clean up any temp tables name swapping.
        match script.TempTable with
        | Some tempTable ->
          { script with
              Source = script.Source.Replace( "#" + tempTable.Name, tempTable.Name) }
        | _ -> script
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
        not hasUnsupportedParameter
    )
  
  let sprocs =
    getStoredProcedures sysTypeIdLookup tableTypesByUserId conn
    |> List.filter (fun sp -> RuleSet.shouldIncludeProcedure sp.SchemaName sp.Name cfg)
    |> List.filter (fun sp ->

        let hasUnsupportedParameter =
          sp.Parameters |> List.exists (fun p ->
            if p.IsCursorRef then
              logWarning $"Parameter '%s{p.Name}' in stored procedure '%s{sp.SchemaName}.%s{sp.Name}' is a cursor reference, which is not supported. Ignoring stored procedure. To remove this warning, make sure this stored procedure is not included in any rules."
              true
            else false
          )

        let hasUnsupportedResultColumn =
          match sp.ResultSet with
          | None -> false
          | Some cols ->
              match cols |> List.tryFindIndex (fun c -> c.Name.IsNone) with
              | Some idx when idx > 0 || cols.Length > 1 ->
                  logWarning $"Column #{idx + 1} of {cols.Length} returned by stored procedure '{sp.SchemaName}.{sp.Name}' is missing a name. Columns without names are only supported if they are the only column in the result set. To remove this warning, make sure this stored procedure is not included in any rules."
                  true
              | _ -> false

        not hasUnsupportedParameter && not hasUnsupportedResultColumn
    )

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

  let tableDtos =
    getTableDtos sysTypeIdLookup conn
    |> List.filter (fun dto -> RuleSet.shouldIncludeTableDto dto.SchemaName dto.Name cfg)


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
    for paramName, _ in rule.Parameters |> Map.toList do
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
    for paramName, _ in rule.Parameters |> Map.toList do
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
