module GeneratorValidationTests

open System
open System.IO
open System.Text.Json
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

    try
        let exitCode = Facil.Program.main [| projectDir |]
        exitCode, writer.ToString()
    finally
        Console.SetOut(originalOut)


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
    ]
