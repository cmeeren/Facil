module ScriptIncludeTests

open System
open System.IO

open Expecto

open Facil.Config


let private withScripts (files: (string * string) list) action =
    let root =
        Path.Combine(Path.GetTempPath(), "FacilIncludeTests", Guid.NewGuid().ToString("N"))

    Directory.CreateDirectory(root) |> ignore

    try
        for relativePath, source in files do
            let path = Path.Combine(root, relativePath)
            Directory.CreateDirectory(Path.GetDirectoryName(path)) |> ignore
            File.WriteAllText(path, source)

        action root
    finally
        Directory.Delete(root, true)


[<Tests>]
let tests =
    testSequenced
    <| testList "Script includes" [
        testCase "typed rows from a shared dynamic SQL body"
        <| fun () ->
            let rows =
                IncludeDbGen.Scripts.Rows
                    .WithConnection(Config.connStr)
                    .WithParameters(minId = 2)
                    .Execute()
                |> Seq.map (fun row -> row.Id, row.Name)
                |> Seq.toList

            Expect.equal rows [ 2, "Two" ] "The row entry script should return typed rows using the shared filter"

        testCase "typed count from the same shared dynamic SQL body"
        <| fun () ->
            let count =
                IncludeDbGen.Scripts.Count
                    .WithConnection(Config.connStr)
                    .WithParameters(minId = 2)
                    .ExecuteSingle()

            Expect.equal
                count
                (Some(Some 1))
                "The count entry script should return a typed count using the shared filter"

        for name, rules, expected in
            [
                "default is disabled", "- include: '*.sql'", false
                "explicit opt-in", "- include: '*.sql'\n  expandIncludes: true", true
                "omitted setting inherits",
                "- include: '*.sql'\n  expandIncludes: true\n- for: Query.sql\n  result: anonymous",
                true
                "later false overrides",
                "- include: '*.sql'\n  expandIncludes: true\n- for: Query.sql\n  expandIncludes: false",
                false
                "except excludes the setting", "- include: '*.sql'\n  except: Query.sql\n  expandIncludes: true", false
            ] do
            testCase $"configuration: %s{name}"
            <| fun () ->
                let indentedRules =
                    rules.Split('\n')
                    |> Array.map (fun line -> "      " + line)
                    |> String.concat "\n"

                let yaml = "rulesets:\n  - connectionString: unused\n    scripts:\n" + indentedRules

                withScripts [ "facil.yaml", yaml ]
                <| fun root ->
                    let _, ruleSets = FacilConfig.getRuleSets root (Path.Combine(root, "facil.yaml"))
                    let _, cfg = List.exactlyOne ruleSets
                    Expect.equal (RuleSet.getEffectiveScriptRuleFor "Query.sql" cfg).ExpandIncludes expected name

        for name, files, expected in
            [
                "inline replacement",
                [ "Query.sql", "SELECT {{include \"Value.sql\"}} AS Value"; "Value.sql", "42" ],
                "SELECT 42 AS Value"
                "replacement inside strings and comments",
                [
                    "Query.sql", "SELECT '{{include \"Value.sql\"}}'; -- {{include \"Value.sql\"}}"
                    "Value.sql", "a'b"
                ],
                "SELECT 'a'b'; -- a'b"
                "nested paths resolve relative to each including file",
                [
                    "Query.sql", "{{include \"Shared/First.sql\"}}"
                    "Shared/First.sql", "SELECT {{include \"../Values/Second.txt\"}}"
                    "Values/Second.txt", "42"
                ],
                "SELECT 42"
                "repeated includes expand independently",
                [
                    "Query.sql", "{{include \"Value.sql\"}} + {{include \"Value.sql\"}}"
                    "Value.sql", "42"
                ],
                "42 + 42"
                "fragment whitespace is preserved",
                [
                    "Query.sql", "SELECT {{include \"Value.sql\"}}AS Value"
                    "Value.sql", "42 -- comment\r\n"
                ],
                "SELECT 42 -- comment\nAS Value"
                "replacement text is literal",
                [ "Query.sql", "{{include \"Value.sql\"}}"; "Value.sql", "'$1 ${name} \\'" ],
                "'$1 ${name} \\'"
                "empty fragments", [ "Query.sql", "SELECT {{include \"Empty.sql\"}}42"; "Empty.sql", "" ], "SELECT 42"
                "paths containing spaces",
                [
                    "Query.sql", "{{include \"Shared/Some value.sql\"}}"
                    "Shared/Some value.sql", "42"
                ],
                "42"
            ] do
            testCase name
            <| fun () ->
                withScripts files
                <| fun root -> Expect.equal (Facil.ScriptIncludes.readSource root "Query.sql" true) expected name

        testCase "disabled expansion preserves literal tokens without reading fragments"
        <| fun () ->
            let source = "SELECT '{{include \"Missing.sql\"}}'\r\n-- {{include malformed}}\r\n"

            withScripts [ "Query.sql", source ]
            <| fun root ->
                Expect.equal
                    (Facil.ScriptIncludes.readSource root "Query.sql" false)
                    "SELECT '{{include \"Missing.sql\"}}'\n-- {{include malformed}}"
                    "Disabled scripts retain the existing source-loading behavior"

        testCase "absolute paths are rejected even inside scriptBasePath"
        <| fun () ->
            withScripts [ "Value.sql", "42" ]
            <| fun root ->
                let target = Path.Combine(root, "Value.sql")
                File.WriteAllText(Path.Combine(root, "Query.sql"), "{{include \"" + target + "\"}}")

                let error =
                    try
                        Facil.ScriptIncludes.readSource root "Query.sql" true |> ignore
                        "No error"
                    with ex ->
                        ex.Message

                Expect.stringContains error "must be relative" "Only relative include paths are supported"

        for linkKind, targetLocation in
            [
                "directory", "outside"
                "directory", "inside"
                "file", "outside"
                "file", "inside"
            ] do
            testCase $"rejects %s{linkKind} links %s{targetLocation} scriptBasePath"
            <| fun () ->
                withScripts [ "Shared/Value.sql", "42"; "SQL/Shared/Value.sql", "42" ]
                <| fun root ->
                    let scriptBasePath = Path.Combine(root, "SQL")

                    let targetDirectory =
                        Path.Combine((if targetLocation = "inside" then scriptBasePath else root), "Shared")

                    let relativePath, link =
                        if linkKind = "directory" then
                            "Linked/Value.sql",
                            Directory.CreateSymbolicLink(Path.Combine(scriptBasePath, "Linked"), targetDirectory)
                        else
                            "Linked.sql",
                            File.CreateSymbolicLink(
                                Path.Combine(scriptBasePath, "Linked.sql"),
                                Path.Combine(targetDirectory, "Value.sql")
                            )

                    try
                        File.WriteAllText(
                            Path.Combine(scriptBasePath, "Query.sql"),
                            "{{include \"" + relativePath + "\"}}"
                        )

                        let error =
                            try
                                Facil.ScriptIncludes.readSource scriptBasePath "Query.sql" true |> ignore
                                "No error"
                            with ex ->
                                ex.Message

                        Expect.stringContains
                            error
                            "reparse point"
                            "Linked files and path components should be rejected"
                    finally
                        link.Delete()

        for name, files, expectedError in
            [
                "missing nested file",
                [
                    "Query.sql", "{{include \"Shared/First.sql\"}}"
                    "Shared/First.sql", "SELECT\n  {{include \"Missing.sql\"}}"
                ],
                [
                    "First.sql(2,3)"
                    "Missing.sql"
                    "Query.sql -> Shared/First.sql -> Shared/Missing.sql"
                ]
                "direct cycle", [ "Query.sql", "{{include \"./Query.sql\"}}" ], [ "cycle"; "Query.sql -> Query.sql" ]
                "indirect cycle",
                [
                    "Query.sql", "{{include \"Shared/First.sql\"}}"
                    "Shared/First.sql", "{{include \"../Query.sql\"}}"
                ],
                [ "cycle"; "Query.sql -> Shared/First.sql -> Query.sql" ]
                "path outside scriptBasePath",
                [ "Query.sql", "{{include \"../outside.sql\"}}" ],
                [ "outside scriptBasePath"; "Query.sql(1,1)" ]
                "malformed directive",
                [ "Query.sql", "SELECT\n{{include Missing.sql}}" ],
                [ "Invalid include directive"; "Query.sql(2,1)" ]
                "unclosed directive",
                [ "Query.sql", "{{include \"Missing.sql\"" ],
                [ "Invalid include directive"; "Query.sql(1,1)" ]
            ] do
            for expectedPart in expectedError do
                testCase $"%s{name}: %s{expectedPart}"
                <| fun () ->
                    withScripts files
                    <| fun root ->
                        let error =
                            try
                                Facil.ScriptIncludes.readSource root "Query.sql" true |> ignore
                                "No error"
                            with ex ->
                                ex.Message.Replace("\\", "/")

                        Expect.stringContains error expectedPart "The expansion error should identify the failure"
    ]
