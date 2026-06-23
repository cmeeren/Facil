module GeneratorValidationTests

open System
open System.IO
open System.Text.Json
open System.Text.RegularExpressions
open Expecto


let private withTemporaryGeneratorProject (yaml: string) (scripts: (string * string) list) action =
    let root =
        Path.Combine(Path.GetTempPath(), "FacilGeneratorValidationTests", Guid.NewGuid().ToString("N"))

    Directory.CreateDirectory(root) |> ignore

    try
        Directory.CreateDirectory(Path.Combine(root, "SQL")) |> ignore

        let appSettings = JsonSerializer.Serialize({| connectionString = Config.connStr |})

        File.WriteAllText(Path.Combine(root, "appsettings.json"), appSettings)
        File.WriteAllText(Path.Combine(root, "facil.yaml"), yaml)

        for path, content in scripts do
            let fullPath = Path.Combine(root, "SQL", path)
            let dir = Path.GetDirectoryName(fullPath)

            if not (String.IsNullOrWhiteSpace dir) then
                Directory.CreateDirectory(dir) |> ignore

            File.WriteAllText(fullPath, content)

        action root
    finally
        if Directory.Exists(root) then
            Directory.Delete(root, true)


let private runGenerator projectDir =
    let originalOut = Console.Out
    use writer = new StringWriter()

    Console.SetOut(writer)

    let generatorEnvVars = [
        Facil.Program.envvar_force_regenerate
        Facil.Program.envvar_fail_on_changed_output
        Facil.Program.envvar_fail_on_regenerate
    ]

    let originalEnvVars =
        generatorEnvVars
        |> List.map (fun name -> name, Environment.GetEnvironmentVariable name)

    try
        for name in generatorEnvVars do
            Environment.SetEnvironmentVariable(name, null)

        let exitCode = Facil.Program.main [| projectDir |]
        exitCode, writer.ToString()
    finally
        for name, value in originalEnvVars do
            Environment.SetEnvironmentVariable(name, value)

        Console.SetOut(originalOut)


let private dateOnlyTableTypeRow date dateTime =
    DateOnlyDbGen.TableTypes.dbo.AllTypesNonNull.create (
        bigint = 1L,
        binary = Array.replicate 42 1uy,
        bit = true,
        char = String.replicate 42 "a",
        date = date,
        datetime = dateTime,
        datetime2 = dateTime,
        datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
        decimal = 1M,
        float = 1.,
        image = [| 1uy |],
        int = 1,
        money = 1M,
        nchar = String.replicate 42 "a",
        ntext = "test",
        numeric = 1M,
        nvarchar = "test",
        real = 1.f,
        smalldatetime = dateTime,
        smallint = 1s,
        smallmoney = 1M,
        text = "test",
        time = TimeSpan.FromSeconds 1.,
        tinyint = 1uy,
        uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
        varbinary = [| 1uy |],
        varchar = "test",
        xml = "<tag />"
    )


let private dateOnlyTempTableRow date dateTime =
    DateOnlyDbGen.Scripts.TempTableAllTypesNonNull.AllTypesNonNull.create (
        bigint = 1L,
        binary = Array.replicate 42 1uy,
        bit = true,
        char = String.replicate 42 "a",
        date = date,
        datetime = dateTime,
        datetime2 = dateTime,
        datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
        decimal = 1M,
        float = 1.,
        image = [| 1uy |],
        int = 1,
        money = 1M,
        nchar = String.replicate 42 "a",
        ntext = "test",
        numeric = 1M,
        nvarchar = "test",
        real = 1.f,
        smalldatetime = dateTime,
        smallint = 1s,
        smallmoney = 1M,
        text = "test",
        time = TimeSpan.FromSeconds 1.,
        tinyint = 1uy,
        uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
        varbinary = [| 1uy |],
        varchar = "test",
        xml = "<tag />"
    )

[<Tests>]
let tests =
    testSequenced
    <| testList "Generator validation tests" [

        testCase "Duplicate output column names are ignored with a clear warning"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: DuplicateColumns.sql
                """

            withTemporaryGeneratorProject yaml [ "DuplicateColumns.sql", "SELECT Foo = 1, Foo = 2" ]
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 0 "Generation should succeed and ignore the script"
                Expect.stringContains output "returns 2 columns named 'Foo'" "Expected a clear warning"
                Expect.stringContains output "Ignoring script" "Expected the script to be ignored"


        testCase "Unnamed columns in multi-column result sets are ignored with a clear warning"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: UnnamedMultiColumn.sql
                """

            withTemporaryGeneratorProject yaml [ "UnnamedMultiColumn.sql", "SELECT 1, Foo = 2" ]
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 0 "Generation should succeed and ignore the script"
                Expect.stringContains output "is missing a name" "Expected a clear warning"
                Expect.stringContains output "Ignoring script" "Expected the script to be ignored"


        testCase "Temp tables that collapse to the same generated name fail generation"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: TempTableNameCollision.sql
                        tempTables:
                          - definition: 'CREATE TABLE #args (Col1 INT NOT NULL)'
                          - definition: 'CREATE TABLE ##args (Col1 INT NOT NULL)'
                """

            withTemporaryGeneratorProject yaml [ "TempTableNameCollision.sql", "SELECT Foo = 1" ]
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 1 "Generation should fail for temp-table generated-name collisions"
                Expect.stringContains output "same generated type name" "Expected a clear error"


        testCase "Temp table row factory create overload parameters are camelCased"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: TempTableRows.sql
                        tempTables:
                          - definition: 'CREATE TABLE #Rows (Id INT NOT NULL, Foo BIGINT NOT NULL)'
                """

            let script =
                """
                SELECT Id, Foo
                FROM #Rows
                """

            withTemporaryGeneratorProject yaml [ "TempTableRows.sql", script ]
            <| fun projectDir ->
                let exitCode, _ = runGenerator projectDir
                let generated = File.ReadAllText(Path.Combine(projectDir, "DbGen.fs"))

                Expect.equal exitCode 0 "Generation should succeed"

                Expect.isTrue
                    (Regex
                        .Match(
                            generated,
                            "static member create\\s+\\(\\s+``id``: int,\\s+``foo``: int64\\s+\\) : ``Rows`` =",
                            RegexOptions.Singleline
                        )
                        .Success)
                    "The individual-parameter create overload should use camelCased parameter names"

                Expect.stringContains
                    generated
                    "(^a: (member ``Id``: int) dto) |> box"
                    "The DTO overload should still use column-shaped member names"


        testCase "Nullable table-type columns can be used for getByIdBatch scripts"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    tableScripts:
                      - include: '^dbo\.TableWithIdentityCol$'
                        scripts:
                          - type: getByIdBatch
                            tableType: dbo.SingleColNull
                """

            withTemporaryGeneratorProject yaml []
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir
                let generated = File.ReadAllText(Path.Combine(projectDir, "DbGen.fs"))

                Expect.equal exitCode 0 "Generation should succeed for nullable batch table types"

                Expect.stringContains
                    generated
                    "TypeName = \"dbo.SingleColNull\""
                    "Expected the configured table type to be used"

                Expect.isFalse
                    (output.Contains("can not be used", StringComparison.Ordinal))
                    "Generation should not reject the nullable table type"


        testCase "Table scripts with the same final name are generated once"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    tableScripts:
                      - include: '^dbo\.TableWithIdentityCol$'
                        scripts:
                          - type: getById

                      - for: '^dbo\.TableWithIdentityCol$'
                        scripts:
                          - type: getById
                            name: '{TableName}_ById'
                            selectColumns:
                              - Id
                """

            withTemporaryGeneratorProject yaml []
            <| fun projectDir ->
                let exitCode, _ = runGenerator projectDir
                let generated = File.ReadAllText(Path.Combine(projectDir, "DbGen.fs"))

                let scriptTypeOccurrences =
                    Regex.Matches(generated, "type ``TableWithIdentityCol_ById`` private").Count

                let executableTypeOccurrences =
                    Regex.Matches(generated, "type ``TableWithIdentityCol_ById_Executable``").Count

                Expect.equal exitCode 0 "Generation should succeed when table script rules resolve to the same name"
                Expect.equal scriptTypeOccurrences 1 "The script type should be generated once"
                Expect.equal executableTypeOccurrences 1 "The executable type should be generated once"

                Expect.isFalse
                    (generated.Contains("let mutable ``ordinal_Foo``", StringComparison.Ordinal))
                    "Column rules from the resolved-name rule should be preserved"


        testCase "Table scripts with the same final name and different types fail generation"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    tableScripts:
                      - include: '^dbo\.TableWithIdentityCol$'
                        scripts:
                          - type: getAll
                            name: ConflictingName

                          - type: getById
                            name: ConflictingName
                """

            withTemporaryGeneratorProject yaml []
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 1 "Generation should fail before duplicate generated type names can be written"

                Expect.stringContains
                    output
                    "resolve to the same script name 'ConflictingName'"
                    "Expected a clear duplicate table-script name error"


        testCase "Rulesets that resolve to the same output file fail generation"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen

                  - connectionString: $(connectionString)
                    filename: ./DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen2
                """

            withTemporaryGeneratorProject yaml []
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 1 "Generation should fail before duplicate output files can be written"

                Expect.stringContains
                    output
                    "resolve to the same output file"
                    "Expected a clear duplicate output file error"

                Expect.isFalse
                    (File.Exists(Path.Combine(projectDir, "DbGen.fs")))
                    "Generation should fail before writing the duplicate output file"


        testCase "Scripts matched by multiple include rules are generated once"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: DuplicateInclude.sql
                      - include: '*.sql'
                """

            withTemporaryGeneratorProject yaml [ "DuplicateInclude.sql", "SELECT Foo = 1" ]
            <| fun projectDir ->
                let exitCode, _ = runGenerator projectDir
                let generated = File.ReadAllText(Path.Combine(projectDir, "DbGen.fs"))

                let scriptOccurrences =
                    Regex.Matches(generated, "\"path\": \"DuplicateInclude.sql\"").Count

                let typeOccurrences =
                    Regex.Matches(generated, "type ``DuplicateInclude`` private").Count

                Expect.equal exitCode 0 "Generation should succeed when a script matches multiple include rules"
                Expect.equal scriptOccurrences 1 "The manifest should contain the script once"
                Expect.equal typeOccurrences 1 "The script should be generated once"


        testCase "Named sp_executesql dynamic parameters are accepted during script analysis"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: NamedSysSpExecuteSql.sql
                        params:
                          col1Filter:
                            type: NVARCHAR(42)

                      - include: NamedBareSpExecuteSql.sql
                        params:
                          col1Filter:
                            type: NVARCHAR(42)

                      - include: FullyNamedSpExecuteSql.sql
                        params:
                          col1Filter:
                            type: NVARCHAR(42)
                """

            let sysScript =
                """
                DECLARE @sql NVARCHAR(MAX) =
                  N'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

                EXEC sys.sp_executesql @sql, N'@col1Filter NVARCHAR(42)', @col1Filter = @col1Filter
                """

            let bareScript =
                """
                DECLARE @sql NVARCHAR(MAX) =
                  N'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

                EXEC sp_executesql @sql, N'@col1Filter NVARCHAR(42)', @col1Filter = @col1Filter
                """

            let fullyNamedScript =
                """
                DECLARE @sql NVARCHAR(MAX) =
                  N'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

                EXEC sys.sp_executesql @stmt = @sql, @params = N'@col1Filter NVARCHAR(42)', @col1Filter = @col1Filter
                """

            let scripts = [
                "NamedSysSpExecuteSql.sql", sysScript
                "NamedBareSpExecuteSql.sql", bareScript
                "FullyNamedSpExecuteSql.sql", fullyNamedScript
            ]

            withTemporaryGeneratorProject yaml scripts
            <| fun projectDir ->
                let exitCode, _ = runGenerator projectDir
                let generated = File.ReadAllText(Path.Combine(projectDir, "DbGen.fs"))

                Expect.equal exitCode 0 "Generation should succeed for named sp_executesql dynamic parameters"

                Expect.stringContains
                    generated
                    "@col1Filter = @col1Filter"
                    "The generated source should preserve the original SQL"


        testCase "Named sp_executesql parameter names do not count as script parameter usage"
        <| fun () ->
            let yaml =
                """
                configs:
                  - appSettings: appsettings.json

                rulesets:
                  - connectionString: $(connectionString)
                    filename: DbGen.fs
                    namespaceOrModuleDeclaration: module DbGen
                    scriptBasePath: SQL
                    scripts:
                      - include: NamedSpExecuteSqlLiteral.sql
                        params:
                          col1Filter:
                            type: INT
                """

            let script =
                """
                DECLARE @sql NVARCHAR(MAX) =
                  N'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

                EXEC sys.sp_executesql @sql, N'@col1Filter INT', @col1Filter = 42
                """

            withTemporaryGeneratorProject yaml [ "NamedSpExecuteSqlLiteral.sql", script ]
            <| fun projectDir ->
                let exitCode, output = runGenerator projectDir

                Expect.equal exitCode 0 "Generation should succeed when a named dynamic argument uses a literal"

                Expect.stringContains
                    output
                    "parameter '@col1Filter' that is not used"
                    "The dynamic parameter name should not count as outer script parameter usage"


        testCase "SQL date generates DateOnly by default"
        <| fun () ->
            let expectedDate = DateOnly(2000, 1, 2)
            let expectedRowDate = DateOnly(2001, 2, 3)
            let expectedDateTime = DateTime(2000, 1, 3)

            let procRes =
                DateOnlyDbGen.Procedures.dbo.ProcWithAllTypes
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        bigint = 1L,
                        binary = Array.replicate 42 1uy,
                        bit = true,
                        char = String.replicate 42 "a",
                        date = expectedDate,
                        datetime = expectedDateTime,
                        datetime2 = expectedDateTime,
                        datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                        decimal = 1M,
                        float = 1.,
                        image = [| 1uy |],
                        int = 1,
                        money = 1M,
                        nchar = String.replicate 42 "a",
                        ntext = "test",
                        numeric = 1M,
                        nvarchar = "test",
                        real = 1.f,
                        rowversion = Array.replicate 8 1uy,
                        smalldatetime = expectedDateTime,
                        smallint = 1s,
                        smallmoney = 1M,
                        text = "test",
                        time = TimeSpan.FromSeconds 1.,
                        timestamp = Array.replicate 8 1uy,
                        tinyint = 1uy,
                        uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                        varbinary = [| 1uy |],
                        varchar = "test",
                        xml = "<tag />"
                    )
                    .ExecuteSingle()
                    .Value

            Expect.equal procRes.date.Value expectedDate "Procedure DATE parameters and results should use DateOnly"
            Expect.equal procRes.datetime.Value expectedDateTime "DATETIME should remain DateTime"

            let tvpRes =
                DateOnlyDbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                    .WithConnection(Config.connStr)
                    .WithParameters(``params`` = [ dateOnlyTableTypeRow expectedDate expectedDateTime ])
                    .ExecuteSingle()
                    .Value

            Expect.equal tvpRes.date expectedDate "Table-type DATE values should use DateOnly"
            Expect.equal tvpRes.datetime expectedDateTime "Table-type DATETIME values should remain DateTime"

            let tempTableRes =
                DateOnlyDbGen.Scripts.TempTableAllTypesNonNull
                    .WithConnection(Config.connStr)
                    .WithParameters(allTypesNonNull = [ dateOnlyTempTableRow expectedRowDate expectedDateTime ])
                    .ExecuteSingle()
                    .Value

            Expect.equal tempTableRes.Date expectedRowDate "Temp-table DATE rows should use DateOnly"
            Expect.equal tempTableRes.Datetime expectedDateTime "Temp-table DATETIME rows should remain DateTime"

            let outRes =
                DateOnlyDbGen.Procedures.dbo.ProcDateOnlyOut
                    .WithConnection(Config.connStr)
                    .WithParameters(dateParam = expectedDate)
                    .ExecuteSingle()

            Expect.equal outRes.Result (Some(Some expectedDate)) "SQL date reader code should use DateOnly"
            Expect.equal outRes.Out.dateOut (Some expectedDate) "SQL date output parameters should use DateOnly"


        testCase "SQL date can opt out to DateTime"
        <| fun () ->
            let expectedDate = DateTime(2000, 1, 2)
            let expectedDateTime = DateTime(2000, 1, 3)

            let tvpRes =
                DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        ``params`` = [
                            DbGen.TableTypes.dbo.AllTypesNonNull.create (
                                bigint = 1L,
                                binary = Array.replicate 42 1uy,
                                bit = true,
                                char = String.replicate 42 "a",
                                date = expectedDate,
                                datetime = expectedDateTime,
                                datetime2 = expectedDateTime,
                                datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                decimal = 1M,
                                float = 1.,
                                image = [| 1uy |],
                                int = 1,
                                money = 1M,
                                nchar = String.replicate 42 "a",
                                ntext = "test",
                                numeric = 1M,
                                nvarchar = "test",
                                real = 1.f,
                                smalldatetime = expectedDateTime,
                                smallint = 1s,
                                smallmoney = 1M,
                                text = "test",
                                time = TimeSpan.FromSeconds 1.,
                                tinyint = 1uy,
                                uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                varbinary = [| 1uy |],
                                varchar = "test",
                                xml = "<tag />"
                            )
                        ]
                    )
                    .ExecuteSingle()
                    .Value

            Expect.equal tvpRes.date expectedDate "dateType: dateTime should keep table-type DATE values as DateTime"
            Expect.equal tvpRes.datetime expectedDateTime "dateType: dateTime should not affect DATETIME"

    ]
