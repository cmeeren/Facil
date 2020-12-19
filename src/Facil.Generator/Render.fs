module internal Facil.Render

open System


let private indent (lines: string list) =
  lines |> List.map (fun s -> "  " + s)


let private renderTableDto (cfg: RuleSet) (dto: TableDto) =
  let rule = RuleSet.getEffectiveTableDtoRuleFor dto.SchemaName dto.Name cfg
  let optionType = if rule.Voption then "voption" else "option"
  [
    ""
    ""
    $"type ``{dto.Name}`` ="
    yield! indent [
      "{"
      yield! indent [
        for c in dto.Columns do
          $"""{c.Name.Value}: {c.TypeInfo.FSharpTypeString}{if c.IsNullable then " " + optionType else ""}"""
      ]
      "}"
    ]
  ]


let private renderTableDtos (cfg: RuleSet) (dtos: TableDto list) =
  if dtos.IsEmpty then []
  else
    let dtosBySchemaName = dtos |> List.groupBy (fun t -> t.SchemaName)
    [
      "module TableDtos ="
      for schemaName, dtos in dtosBySchemaName do
        yield! indent [
          ""
          ""
          $"module ``{schemaName}`` ="
          for dto in dtos do
            yield! indent (renderTableDto cfg dto)
        ]
      ""
      ""
    ]


let private renderTableType cfg (t: TableType) =
  let rule = RuleSet.getEffectiveTableTypeRuleFor t.SchemaName t.Name cfg
  let optionType = if rule.Voption then "voption" else "option"
  let optionModule = if rule.Voption then "ValueOption" else "Option"

  [
    ""
    ""
    $"type ``{t.Name}`` (__: InternalUseOnly) ="
    yield! indent [
      $"inherit SqlDataRecord ([|"
      yield! indent [
        for c in t.Columns do
          if isPrecisionAndScaleRelevantForSqlMetaData c.TypeInfo.SqlDbType then
            $"""SqlMetaData("{c.StringEscapedName}", SqlDbType.{c.TypeInfo.SqlDbType}, {c.PrecisionForSqlMetaData}uy, {c.Scale}uy)"""
          elif isSizeRelevantForSqlMetaData c.TypeInfo.SqlDbType then
            $"""SqlMetaData("{c.StringEscapedName}", SqlDbType.{c.TypeInfo.SqlDbType}, {c.Size}L)"""
          else
            $"""SqlMetaData("{c.StringEscapedName}", SqlDbType.{c.TypeInfo.SqlDbType})"""
      ]
      "|])"
      ""
      $"static member create"
      yield! indent [
        "("
        yield! indent [
          yield!
            t.Columns
            |> List.map (fun c ->
                $"""``{c.Name}``: {c.TypeInfo.FSharpTypeString}{if c.IsNullable then " " + optionType else ""}"""
            )
            |> List.mapAllExceptLast (fun s -> s + ",")
        ]
        ") ="
        $"let x = ``{t.Name}``(internalUseOnlyValue)"
        "x.SetValues("
        yield! indent [
          yield!
            t.Columns
            |> List.map (fun c ->
                $"""{if c.IsNullable then $"{optionModule}.toDbNull " else ""}``{c.Name}``"""
            )
            |> List.mapAllExceptLast (fun s -> s + ",")
        ]
        ")"
        "|> ignore"
        "x"
      ]
      if not rule.SkipParamDto then
        ""
        $"static member inline create (dto: ^a) ="
        yield! indent [
          $"let x = ``{t.Name}``(internalUseOnlyValue)"
          "x.SetValues("
          yield! indent [
            yield!
              t.Columns
              |> List.map (fun c ->
                  $"""{if c.IsNullable then $"{optionModule}.toDbNull " else ""}(^a: (member ``{c.Name}``: {c.TypeInfo.FSharpTypeString}{if c.IsNullable then " " + optionType else ""}) dto)"""
              )
              |> List.mapAllExceptLast (fun s -> s + ",")
          ]
          ")"
          "|> ignore"
          "x"
        ]
    ]
  ]


let private renderTableTypes cfg (types: TableType list) =
  if types.IsEmpty then []
  else
    let tsBySchemaName = types |> List.groupBy (fun t -> t.SchemaName)
    [
      "module TableTypes ="
      for schemaName, ts in tsBySchemaName do
        yield! indent [
          ""
          ""
          $"module ``{schemaName}`` ="
          for t in ts do
            yield! indent (renderTableType cfg t)
        ]
      ""
      ""
    ]


let private renderProcOrScript (cfg: RuleSet) (tableDtos: TableDto list) (executable: Choice<StoredProcedure, Script>) =
  let rule =
    match executable with
    | Choice1Of2 sp ->
        RuleSet.getEffectiveProcedureRuleFor sp.SchemaName sp.Name cfg
        |> EffectiveProcedureOrScriptRule.fromEffectiveProcedureRule
    | Choice2Of2 s ->
        RuleSet.getEffectiveScriptRuleFor s.GlobMatchOutput cfg
        |> EffectiveProcedureOrScriptRule.fromEffectiveScriptRule

  let nameForLogs =
    match executable with
    | Choice1Of2 sp -> $"Stored procedure {sp.SchemaName}.{sp.Name}"
    | Choice2Of2 s -> $"Script {s.GlobMatchOutput}"

  let className =
    match executable with
    | Choice1Of2 sp -> sp.Name
    | Choice2Of2 s -> s.NameWithoutExtension

  let resultSet =
    match executable with
    | Choice1Of2 sp -> sp.ResultSet
    | Choice2Of2 s -> s.ResultSet

  let parameters =
    match executable with
    | Choice1Of2 sp -> sp.Parameters
    | Choice2Of2 s -> s.Parameters

  let tempTables =
    match executable with
    | Choice1Of2 _ -> []
    | Choice2Of2 s -> s.TempTables

  let inOptionModule = if rule.VoptionIn then "ValueOption" else "Option"
  let inOptionType = if rule.VoptionIn then "voption" else "option"
  let outOptionSome = if rule.VoptionOut then "ValueSome" else "Some"
  let outOptionNone = if rule.VoptionOut then "ValueNone" else "None"

  let getItemReturnTypeExpr, getItemRecordStart, getItemRecordEnd =
    match rule.Result with
    | Auto ->
        match tableDtos |> List.filter (TableDto.canBeUsedBy resultSet rule cfg) with
        | [] -> "", "{|", "|}"
        | [dto] -> $" : TableDtos.{dto.SchemaName}.{dto.Name}", "{", "}"
        | dtos ->
            let matchingTableDtoStr = dtos |> List.map (fun dto -> $"{dto.SchemaName}.{dto.Name}") |> String.concat ", "
            logWarning $"Output of {nameForLogs |> String.firstLower} matches more than one table DTO. Falling back to anonymous record. To remove this warning, specify a matching rule that uses a specific table type or anonymous record. The matching table DTOs are: %s{matchingTableDtoStr}"
            "", "{|", "|}"
    | AnonymousRecord -> "", "{|", "|}"
    | Custom name ->
        match tableDtos |> List.tryFind (fun dto -> $"{dto.SchemaName}.{dto.Name}" = name) with
        | None -> $" : %s{name}", "{", "}"
        | Some dto when dto |> TableDto.canBeUsedBy resultSet rule cfg -> $" : TableDtos.{dto.SchemaName}.{dto.Name}", "{", "}"
        | Some dto -> failwithError $"{nameForLogs} specifies result table DTO {dto.SchemaName}.{dto.Name}, but the result set does not match the DTO."

  let hasOutParams = parameters |> List.exists (fun p -> p.IsOutput)
  let useRetVal = rule.UseReturnValue
  let wrapResult = hasOutParams || useRetVal

  let wrapResultDef =
    [
      "let wrapResultWithOutParams result ="
      yield! indent [
        "{|"
        yield! indent [
          "Result = result"
          if hasOutParams then
            "Out ="
            yield! indent [
              "{|"
              yield! indent [
                yield!
                  parameters
                  |> List.indexed
                  |> List.filter (fun (_, p) -> p.IsOutput)
                  |> List.map (fun (i, p) ->
                      let typeInfo =
                        match p.TypeInfo with
                        | Scalar ti -> ti
                        | Table _ -> failwith $"Parsed parameter '{p.Name}' as both table and output, which is impossible"
                      $"``{p.FSharpParamName}`` = if sqlParams.[{i}].Value = box DBNull.Value then {outOptionNone} else sqlParams.[{i}].Value |> unbox<{typeInfo.FSharpTypeString}> |> {outOptionSome}"
                  )
              ]
              "|}"
            ]
          if useRetVal then $"ReturnValue = sqlParams.[{parameters.Length}].Value |> unbox<int>"
        ]
        "|}"
      ]
      ""
    ]

  if parameters.IsEmpty && tempTables.IsEmpty then
    [
      ""
      ""
      $"type ``{className}`` private (connStr: string, conn: SqlConnection) ="
      ""
      yield! indent [
        "let configureCmd userConfigureCmd (cmd: SqlCommand) ="
        yield! indent [
          match executable with
          | Choice1Of2 sp ->
              "cmd.CommandType <- CommandType.StoredProcedure"
              $"cmd.CommandText <- \"{sp.SchemaName}.{sp.Name}\""
          | Choice2Of2 s ->
              "cmd.CommandText <- \"\"\""
              yield! indent (s.Source.Split "\n" |> Array.map (fun s -> s.TrimEnd '\r') |> Array.toList)
              "\"\"\""
          "userConfigureCmd cmd"
        ]
        ""
        match resultSet with
        | None -> ()
        | Some [c] when c.Name.IsNone || not rule.RecordIfSingleCol ->
            "let initOrdinals = ignore<SqlDataReader>"
            ""
            "let getItem (reader: SqlDataReader) ="
            yield! indent [
              if c.IsNullable then
                $"if reader.IsDBNull 0 then {outOptionNone} else reader.{c.TypeInfo.SqlDataReaderGetMethodName} 0 |> {outOptionSome}"
              else
                $"reader.{c.TypeInfo.SqlDataReaderGetMethodName} 0"
            ]
            ""
        | Some cols ->
            for c in cols do
              $"let mutable ``ordinal_{c.Name.Value}`` = 0"
            ""
            "let initOrdinals (reader: SqlDataReader) ="
            yield! indent [
              for c in cols do
                $"``ordinal_{c.Name.Value}`` <- reader.GetOrdinal \"{c.StringEscapedName.Value}\""
            ]
            ""

            $"let getItem (reader: SqlDataReader){getItemReturnTypeExpr} ="
            yield! indent [
              getItemRecordStart
              yield! indent [
                for c in cols do
                  if c.IsNullable then
                    $"``{c.Name.Value}`` = if reader.IsDBNull ``ordinal_{c.Name.Value}`` then {outOptionNone} else reader.{c.TypeInfo.SqlDataReaderGetMethodName} ``ordinal_{c.Name.Value}`` |> {outOptionSome}"
                  else
                    $"``{c.Name.Value}`` = reader.{c.TypeInfo.SqlDataReaderGetMethodName} ``ordinal_{c.Name.Value}``"
              ]
              getItemRecordEnd
            ]
            ""

        if wrapResult then yield! wrapResultDef

        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val configureConn : SqlConnection -> unit = ignore with get, set"
        ""
        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val userConfigureCmd : SqlCommand -> unit = ignore with get, set"
        ""
        "member this.ConfigureCommand(configureCommand: SqlCommand -> unit) ="
        yield! indent [
          "this.userConfigureCmd <- configureCommand"
          "this"
        ]
        ""

        // Static constructors

        "static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) ="
        yield! indent [
          $"``{className}``(connectionString, null).ConfigureConnection(?configureConnection=configureConnection)"
        ]
        ""
        $"static member WithConnection(connection) = ``{className}``(null, connection)"
        ""
        "member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) ="
        yield! indent [
          "match configureConnection with"
          "| None -> ()"
          "| Some config -> this.configureConn <- config"
          "this"
        ]
        ""

        // Execute methods

        let asyncOverTaskFor (taskMethodName: string) (asyncMethodName: string) =
          [
            $"member this.{asyncMethodName}() ="
            yield! indent [
              "async {"
              yield! indent [
                "let! ct = Async.CancellationToken"
                $"return! this.{taskMethodName}(ct) |> Async.AwaitTask"
              ]
              "}"
            ]
          ]

        match resultSet with
        | None ->
            "member this.ExecuteAsync(?cancellationToken) ="
            yield! indent [
              $"executeNonQueryAsync connStr conn this.configureConn (configureCmd this.userConfigureCmd) [] (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsync" "AsyncExecute"
            ""
            "member this.Execute() ="
            yield! indent [
              $"executeNonQuery connStr conn this.configureConn (configureCmd this.userConfigureCmd) []"
              if wrapResult then "|> wrapResultWithOutParams"
            ]

        | Some _ ->
            "member this.ExecuteAsync(?cancellationToken) ="
            yield! indent [
              $"executeQueryEagerAsync connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem [] (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsync" "AsyncExecute"
            ""
            "member this.ExecuteAsyncWithSyncRead(?cancellationToken) ="
            yield! indent [
              $"executeQueryEagerAsyncWithSyncRead connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem [] (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsyncWithSyncRead" "AsyncExecuteWithSyncRead"
            ""
            "member this.Execute() ="
            yield! indent [
              $"executeQueryEager connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem []"
              if wrapResult then "|> wrapResultWithOutParams"
            ]
            ""
            "#if (!NETFRAMEWORK && !NET461 && !NET462 && !NET47 && !NET471 && !NET472 && !NET48 && !NETSTANDARD2_0 && !NETCOREAPP2_0 && !NETCOREAPP2_1 && !NETCOREAPP2_2)"
            ""
            "member this.LazyExecuteAsync(?cancellationToken) ="
            yield! indent [
              $"executeQueryLazyAsync connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem [] (defaultArg cancellationToken CancellationToken.None)"
            ]
            ""
            "member this.LazyExecuteAsyncWithSyncRead(?cancellationToken) ="
            yield! indent [
              $"executeQueryLazyAsyncWithSyncRead connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem [] (defaultArg cancellationToken CancellationToken.None)"
            ]
            ""
            "#endif"
            ""
            "member this.LazyExecute() ="
            yield! indent [
              $"executeQueryLazy connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem []"
            ]
            ""
            "member this.ExecuteSingleAsync(?cancellationToken) ="
            yield! indent [
              $"""executeQuerySingleAsync{if rule.VoptionOut then "Voption" else ""} connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem [] (defaultArg cancellationToken CancellationToken.None)"""
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteSingleAsync" "AsyncExecuteSingle"
            ""
            "member this.ExecuteSingle() ="
            yield! indent [
              $"""executeQuerySingle{if rule.VoptionOut then "Voption" else ""} connStr conn this.configureConn (configureCmd this.userConfigureCmd) initOrdinals getItem []"""
              if wrapResult then "|> wrapResultWithOutParams"
            ]
      ]
    ]

  else  // Has parameters or temp tables
    [
      ""
      ""
      "[<EditorBrowsable(EditorBrowsableState.Never)>]"
      $"type ``{className}_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, sqlParams: SqlParameter [], tempTableData: seq<TempTableData>) ="
      ""
      yield! indent [
        "let configureCmd (cmd: SqlCommand) ="
        yield! indent [
          match executable with
          | Choice1Of2 sp ->
              "cmd.CommandType <- CommandType.StoredProcedure"
              $"cmd.CommandText <- \"{sp.SchemaName}.{sp.Name}\""
          | Choice2Of2 s ->
              "cmd.CommandText <- \"\"\""
              yield! indent (s.Source.Split "\n" |> Array.map (fun s -> s.TrimEnd '\r') |> Array.toList)
              "\"\"\""
          "cmd.Parameters.AddRange sqlParams"
          "userConfigureCmd cmd"
        ]
        ""
        match resultSet with
        | None -> ()
        | Some [c] when c.Name.IsNone || not rule.RecordIfSingleCol ->
            "let initOrdinals = ignore<SqlDataReader>"
            ""
            "let getItem (reader: SqlDataReader) ="
            yield! indent [
              if c.IsNullable then
                $"if reader.IsDBNull 0 then {outOptionNone} else reader.{c.TypeInfo.SqlDataReaderGetMethodName} 0 |> {outOptionSome}"
              else
                $"reader.{c.TypeInfo.SqlDataReaderGetMethodName} 0"
            ]
            ""
        | Some cols ->
            for c in cols do
              $"let mutable ``ordinal_{c.Name.Value}`` = 0"
            ""
            "let initOrdinals (reader: SqlDataReader) ="
            yield! indent [
              for c in cols do
                $"``ordinal_{c.Name.Value}`` <- reader.GetOrdinal \"{c.StringEscapedName.Value}\""
            ]
            ""

            $"let getItem (reader: SqlDataReader){getItemReturnTypeExpr} ="
            yield! indent [
              getItemRecordStart
              yield! indent [
                for c in cols do
                  if c.IsNullable then
                    $"``{c.Name.Value}`` = if reader.IsDBNull ``ordinal_{c.Name.Value}`` then {outOptionNone} else reader.{c.TypeInfo.SqlDataReaderGetMethodName} ``ordinal_{c.Name.Value}`` |> {outOptionSome}"
                  else
                    $"``{c.Name.Value}`` = reader.{c.TypeInfo.SqlDataReaderGetMethodName} ``ordinal_{c.Name.Value}``"
              ]
              getItemRecordEnd
            ]
            ""

        if wrapResult then yield! wrapResultDef

        // Execute methods

        let asyncOverTaskFor (taskMethodName: string) (asyncMethodName: string) =
          [
            $"member this.{asyncMethodName}() ="
            yield! indent [
              "async {"
              yield! indent [
                "let! ct = Async.CancellationToken"
                $"return! this.{taskMethodName}(ct) |> Async.AwaitTask"
              ]
              "}"
            ]
          ]

        match resultSet with
        | None ->
            "member _.ExecuteAsync(?cancellationToken) ="
            yield! indent [
              "executeNonQueryAsync connStr conn configureConn configureCmd tempTableData (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsync" "AsyncExecute"
            ""
            "member _.Execute() ="
            yield! indent [
              "executeNonQuery connStr conn configureConn configureCmd tempTableData"
              if wrapResult then "|> wrapResultWithOutParams"
            ]

        | Some _ ->
            "member _.ExecuteAsync(?cancellationToken) ="
            yield! indent [
              "executeQueryEagerAsync connStr conn configureConn configureCmd initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsync" "AsyncExecute"
            ""
            "member _.ExecuteAsyncWithSyncRead(?cancellationToken) ="
            yield! indent [
              "executeQueryEagerAsyncWithSyncRead connStr conn configureConn configureCmd initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)"
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteAsyncWithSyncRead" "AsyncExecuteWithSyncRead"
            ""
            "member _.Execute() ="
            yield! indent [
              "executeQueryEager connStr conn configureConn configureCmd initOrdinals getItem tempTableData"
              if wrapResult then "|> wrapResultWithOutParams"
            ]
            ""
            "#if (!NETFRAMEWORK && !NET461 && !NET462 && !NET47 && !NET471 && !NET472 && !NET48 && !NETSTANDARD2_0 && !NETCOREAPP2_0 && !NETCOREAPP2_1 && !NETCOREAPP2_2)"
            ""
            "member _.LazyExecuteAsync(?cancellationToken) ="
            yield! indent [
              "executeQueryLazyAsync connStr conn configureConn configureCmd initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)"
            ]
            ""
            "member _.LazyExecuteAsyncWithSyncRead(?cancellationToken) ="
            yield! indent [
              "executeQueryLazyAsyncWithSyncRead connStr conn configureConn configureCmd initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)"
            ]
            ""
            "#endif"
            ""
            "member _.LazyExecute() ="
            yield! indent [
              "executeQueryLazy connStr conn configureConn configureCmd initOrdinals getItem tempTableData"
            ]
            ""
            "member _.ExecuteSingleAsync(?cancellationToken) ="
            yield! indent [
              $"""executeQuerySingleAsync{if rule.VoptionOut then "Voption" else ""} connStr conn configureConn configureCmd initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)"""
              if wrapResult then "|> Task.map wrapResultWithOutParams"
            ]
            ""
            yield! asyncOverTaskFor "ExecuteSingleAsync" "AsyncExecuteSingle"
            ""
            "member _.ExecuteSingle() ="
            yield! indent [
              $"""executeQuerySingle{if rule.VoptionOut then "Voption" else ""} connStr conn configureConn configureCmd initOrdinals getItem tempTableData"""
              if wrapResult then "|> wrapResultWithOutParams"
            ]
      ]

      if not tempTables.IsEmpty then
        ""
        ""
        $"module ``{className}`` ="

      for tt in tempTables do
        ""
        ""
        yield! indent [
          $"type ``{tt.FSharpName}`` (__: InternalUseOnly, fields: obj []) ="
          yield! indent [
            ""
            "[<EditorBrowsable(EditorBrowsableState.Never)>]"
            "member _.Fields = fields"
            ""
            "static member create"
            yield! indent [
              "("
              yield! indent [
                yield!
                  tt.Columns
                  |> List.map (fun c ->
                      $"""``{c.Name.Value}``: {c.TypeInfo.FSharpTypeString}{if c.IsNullable then " " + inOptionType else ""}"""
                  )
                  |> List.mapAllExceptLast (fun s -> s + ",")
              ]
              $") : ``{tt.FSharpName}`` ="
              "[|"
              yield! indent [
                yield!
                  tt.Columns
                  |> List.map (fun c ->
                      $"""{if c.IsNullable then $"{inOptionModule}.toDbNull " else ""}``{c.Name.Value}`` |> box"""
                  )
              ]
              "|]"
              $"|> fun fields -> ``{tt.FSharpName}``(internalUseOnlyValue, fields)"
            ]
            if not rule.SkipParamDto then
              ""
              $"static member inline create (dto: ^a) : ``{tt.FSharpName}`` ="
              yield! indent [
                "[|"
                yield! indent [
                  yield!
                    tt.Columns
                    |> List.map (fun c ->
                        $"""{if c.IsNullable then $"{inOptionModule}.toDbNull " else ""}(^a: (member ``{c.Name.Value}``: {c.TypeInfo.FSharpTypeString}{if c.IsNullable then " " + inOptionType else ""}) dto) |> box"""
                    )
                ]
                "|]"
                $"|> fun fields -> ``{tt.FSharpName}``(internalUseOnlyValue, fields)"
              ]
          ]
        ]


      ""
      ""
      $"type ``{className}`` private (connStr: string, conn: SqlConnection) ="
      ""
      yield! indent [
        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val connStr = connStr"
        ""
        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val conn = conn"
        ""
        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val configureConn : SqlConnection -> unit = ignore with get, set"
        ""
        "[<EditorBrowsable(EditorBrowsableState.Never)>]"
        "member val userConfigureCmd : SqlCommand -> unit = ignore with get, set"
        ""

        if not tempTables.IsEmpty then
          "[<EditorBrowsable(EditorBrowsableState.Never)>]"
          "member val userConfigureBulkCopy : SqlBulkCopy -> unit = ignore with get, set"
          ""


        "member this.ConfigureCommand(configureCommand: SqlCommand -> unit) ="
        yield! indent [
          "this.userConfigureCmd <- configureCommand"
          "this"
        ]
        ""

        if not tempTables.IsEmpty then
          "member this.ConfigureBulkCopy(configureBulkCopy: SqlBulkCopy -> unit) ="
          yield! indent [
            "this.userConfigureBulkCopy <- configureBulkCopy"
            "this"
          ]
          ""

        // Static constructors

        "static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) ="
        yield! indent [
          $"``{className}``(connectionString, null).ConfigureConnection(?configureConnection=configureConnection)"
        ]
        ""
        $"static member WithConnection(connection) = ``{className}``(null, connection)"
        ""
        "member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) ="
        yield! indent [
          "match configureConnection with"
          "| None -> ()"
          "| Some config -> this.configureConn <- config"
          "this"
        ]
        ""

        if not tempTables.IsEmpty then
          "[<EditorBrowsable(EditorBrowsableState.Never)>]"
          "member this.CreateTempTableData"
          yield! indent [
            "("
            yield! indent [
              yield!
                tempTables
                |> List.map (fun tt ->
                    $"``{tt.FSharpName}``: seq<``{className}``.``{tt.FSharpName}``>"
                )
                |> List.mapAllExceptLast (fun s -> s + ",")
            ]
            ") ="
            "["
            yield! indent [
              yield!
                tempTables
                |> List.collect (fun tt ->
                    [
                      "TempTableData"
                      yield! indent [
                        "("
                        yield! indent [
                          $"\"{tt.Name}\","
                          "\"\"\""
                          yield! String.getDeindentedLines tt.Source
                          "\"\"\","
                          $"(``{tt.FSharpName}`` |> Seq.map (fun x -> x.Fields)),"
                          $"{tt.Columns.Length},"
                          "Action<_> this.userConfigureBulkCopy"
                        ]
                        ")"
                      ]
                    ]
                )
            ]
            "]"

          ]

        // Parameter methods

        //  - Individual params

        "member this.WithParameters"
        yield! indent [
          "("
          yield! indent [
            let tempTableParams =
              tempTables |> List.map (fun tt ->
                $"``{tt.FSharpName |> String.firstLower}``: seq<``{className}``.``{tt.FSharpName}``>"
              )

            let normalParams =
              parameters |> List.sortBy (fun p -> p.IsOutput) |> List.map (fun p -> 
                match p.TypeInfo with
                | Scalar ti ->
                    $"""{if p.IsOutput then "?" else ""}``{p.FSharpParamName}``: {ti.FSharpTypeString}{if p.FSharpDefaultValueString = Some "null" then $" {inOptionType}" else ""}"""
                | Table tt ->
                    $"``{p.FSharpParamName}``: seq<TableTypes.``{tt.SchemaName}``.``{tt.Name}``>"
              )

            yield! tempTableParams @ normalParams |> List.mapAllExceptLast (fun s -> s + ",")
          ]
          ") ="
        ]
        yield! indent [
          "let sqlParams ="
          yield! indent [
            "[|"
            yield! indent [
              for p in parameters do
                let scalarParamValueExpr =
                  match p.FSharpDefaultValueString = Some "null", p.IsOutput with
                  | false, false -> 
                      $"``{p.FSharpParamName}``"
                  | true, false ->
                      $"{inOptionModule}.toDbNull ``{p.FSharpParamName}``"
                  | false, true -> 
                      $"(``{p.FSharpParamName}`` |> Option.map box |> Option.defaultValue (box DBNull.Value))"
                  | true, true ->
                      $"(``{p.FSharpParamName}`` |> Option.map {inOptionModule}.toDbNull |> Option.defaultValue (box DBNull.Value))"
                match p.TypeInfo with
                | Scalar ti when isSizeRelevantForSqlParameter ti.SqlDbType ->
                    $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, Size = {p.Size}, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr})"""
                | Scalar ti when isPrecisionAndScaleRelevantForSqlParameter ti.SqlDbType ->
                    $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, Precision = {p.Precision}uy, Scale = {p.Scale}uy, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr})"""
                | Scalar ti ->
                    $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr})"""
                | Table tt ->
                    $"""SqlParameter("{p.Name}", SqlDbType.Structured, TypeName = "{tt.SchemaName}.{tt.Name}", Value = ``{p.FSharpParamName}``)"""

              if useRetVal then
                "SqlParameter(\"ReturnValue\", SqlDbType.Int, Direction = ParameterDirection.ReturnValue)"
            ]
            "|]"
          ]
          if not tempTables.IsEmpty then
            "let tempTableData ="
            yield! indent [
              "this.CreateTempTableData("
              yield! indent [
                yield!
                  tempTables
                  |> List.map (fun tt -> $"``{tt.FSharpName |> String.firstLower}``")
                  |> List.mapAllExceptLast (fun s -> s + ",")
              ]
              ")"
            ]
          $"""``{className}_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, sqlParams, {if tempTables.IsEmpty then "[]" else "tempTableData"})"""
        ]
        ""

        //  - DTO params
        if not rule.SkipParamDto then
          $"member inline this.WithParameters(dto: ^a) ="
          yield! indent [
            "let sqlParams ="
            yield! indent [
              "[|"
              yield! indent [
                for p in parameters do
                  let dtoName =
                    rule.Parameters.TryFind p.FSharpParamName
                    |> Option.bind (fun p -> p.DtoName)
                    |> Option.defaultValue (p.FSharpParamName |> String.firstUpper)

                  let scalarParamValueExpr (ti: SqlTypeInfo) =
                    if p.FSharpDefaultValueString = Some "null" || p.IsOutput then
                      $"{inOptionModule}.toDbNull (^a: (member ``{dtoName}``: {ti.FSharpTypeString} {inOptionType}) dto)"
                    else
                      $"(^a: (member ``{dtoName}``: {ti.FSharpTypeString}) dto)"
                  match p.TypeInfo with
                  | Scalar ti when isSizeRelevantForSqlParameter ti.SqlDbType ->
                      $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, Size = {p.Size}, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr ti})"""
                  | Scalar ti when isPrecisionAndScaleRelevantForSqlParameter ti.SqlDbType ->
                      $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, Precision = {p.Precision}uy, Scale = {p.Scale}uy, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr ti})"""
                  | Scalar ti ->
                      $"""SqlParameter("{p.Name}", SqlDbType.{ti.SqlDbType}, {if p.IsOutput then "Direction = ParameterDirection.InputOutput, " else ""}Value = {scalarParamValueExpr ti})"""
                  | Table tt ->
                      $"""SqlParameter("{p.Name}", SqlDbType.Structured, TypeName = "{tt.SchemaName}.{tt.Name}", Value = (^a: (member ``{dtoName}``: #seq<TableTypes.``{tt.SchemaName}``.``{tt.Name}``>) dto))"""

                if useRetVal then
                  "SqlParameter(\"ReturnValue\", SqlDbType.Int, Direction = ParameterDirection.ReturnValue)"
              ]
              "|]"
            ]
            if not tempTables.IsEmpty then
              "let tempTableData ="
              yield! indent [
                "this.CreateTempTableData("
                yield! indent [
                  yield!
                    tempTables
                    |> List.map (fun tt -> $"(^a: (member ``{tt.FSharpName |> String.firstUpper}``: #seq<``{className}``.``{tt.FSharpName}``>) dto)")
                    |> List.mapAllExceptLast (fun s -> s + ",")
                ]
                ")"
              ]
            $"""``{className}_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, sqlParams, {if tempTables.IsEmpty then "[]" else "tempTableData"})"""
          ]

      ]
  ]


let private renderProcs (cfg: RuleSet) tableDtos (procs: StoredProcedure list) =
  if procs.IsEmpty then []
  else
    let psBySchemaName =
      procs
      |> List.sortBy (fun t -> t.Name)
      |> List.groupBy (fun t -> t.SchemaName)
    [
      "module Procedures ="
      for schemaName, sps in psBySchemaName do
        yield! indent [
          ""
          ""
          $"module ``{schemaName}`` ="
          for sp in sps do
            yield! indent (renderProcOrScript cfg tableDtos (Choice1Of2 sp))
        ]
      ""
      ""
    ]


let private renderScripts (cfg: RuleSet) tableDtos (scripts: Script list) =

  let rec renderModuleForPath (scriptWithRemainingPathSegments: (Script * string list) list) =
    let renderHere =
      scriptWithRemainingPathSegments
      |> List.choose (fun (s, segments) ->
          match segments with
          | [] -> Some s
          | _ -> None
      )
      |> List.sortBy (fun s -> s.NameWithoutExtension.ToLowerInvariant())

    [
      for script in renderHere do
        yield! indent (renderProcOrScript cfg tableDtos (Choice2Of2 script))

      yield!
        scriptWithRemainingPathSegments
        |> List.choose (fun (s, segments) ->
            match segments with
            | [] -> None
            | hd :: tl -> Some (hd, (s, tl))
        )
        |> List.groupBy fst
        |> List.map (fun (hd, xs) -> hd, xs |> List.map snd)
        |> List.collect (fun (hd, remaining) ->
            [
              ""
              ""
              $"module ``{hd}`` ="
              yield! indent [
                yield! renderModuleForPath remaining
              ]
            ]
        )
        |> indent
    ]


  if scripts.IsEmpty then []
  else
    [
      "module Scripts ="
      yield! scripts |> List.map (fun s -> s, s.RelativePathSegments) |> renderModuleForPath
    ]


let firstLine = "// Edit or remove this or the below line to regenerate on next build"

let secondLineWithHash hash = $"// Hash: %s{hash}"


let renderDocument (cfg: RuleSet) hash (everything: Everything) =
  let version =
    Reflection
      .Assembly
      .GetEntryAssembly()
      .GetCustomAttributes(typeof<Reflection.AssemblyInformationalVersionAttribute>, true)
      |> Array.map (unbox<Reflection.AssemblyInformationalVersionAttribute>)
      |> Array.head
      |> fun x -> x.InformationalVersion
  [
    firstLine
    secondLineWithHash hash
    ""
    "//////////////////////////////////////////"
    "//"
    "// THIS FILE IS AUTOMATICALLY GENERATED"
    "//"
    $"// Facil {version}"
    "//"
    "//////////////////////////////////////////"
    ""
    cfg.NamespaceOrModuleDeclaration
    ""
    "#nowarn \"49\""
    ""
    "open System"
    "open System.ComponentModel"
    "open System.Data"
    "open System.Threading"
    "open Microsoft.Data.SqlClient"
    "open Microsoft.Data.SqlClient.Server"
    "open Facil.Runtime.CSharp"
    "open Facil.Runtime.GeneratedCodeUtils"
    ""
    ""
    match cfg.Prelude with
    | None -> ()
    | Some lines ->
        yield! lines
        ""
        ""
    "[<EditorBrowsable(EditorBrowsableState.Never)>]"
    "type InternalUseOnly = private | InternalUseOnly"
    "[<EditorBrowsable(EditorBrowsableState.Never)>]"
    "let internalUseOnlyValue = InternalUseOnly"
    ""
    ""
    yield! renderTableDtos cfg everything.TableDtos
    yield! renderTableTypes cfg everything.TableTypes
    yield! renderProcs cfg everything.TableDtos everything.StoredProcedures
    yield! renderScripts cfg everything.TableDtos everything.Scripts
  ]
  |> List.map (fun s -> if String.IsNullOrWhiteSpace s then "" else s)
