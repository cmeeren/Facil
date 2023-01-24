namespace Facil

open System
open System.IO
open System.Text.Json
open Microsoft.Data.SqlClient
open GlobExpressions


module Program =


    let envvar_force_regenerate = "FACIL_FORCE_REGENERATE"
    let envvar_fail_on_changed_output = "FACIL_FAIL_ON_CHANGED_OUTPUT"


    let serializerOptionsForHash = JsonSerializerOptions()
    serializerOptionsForHash.Converters.Add(System.Text.Json.Serialization.JsonFSharpConverter())

    let serializeForHash (x: 'a) =
        JsonSerializer.Serialize<'a>(x, serializerOptionsForHash)


    [<EntryPoint>]
    let main argv =
        try

            let projectDir =
                if argv.Length = 0 then
                    @"C:\Users\cmeer\Source\Repos\Vbit.Api.Watcher\src\DbGen"
                else
                    argv[0]
                |> Path.GetFullPath

            let yamlFile1 = FileInfo(Path.Combine(projectDir, "facil.yaml"))
            let yamlFile2 = FileInfo(Path.Combine(projectDir, "facil.yml"))

            let yamlFilePath =
                match yamlFile1.Exists, yamlFile2.Exists with
                | true, true ->
                    logWarning $"Found both %s{yamlFile1.Name} and %s{yamlFile2.Name}; using %s{yamlFile1.Name}"
                    yamlFile1.FullName
                | true, false -> yamlFile1.FullName
                | false, true -> yamlFile2.FullName
                | false, false ->
                    let execDir =
                        Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)

                    File.Copy(Path.Combine(execDir, "facil_minimal.yaml"), yamlFile1.FullName)

                    failwithError
                        $"No config file found. A minimal config file has been placed in the project directory ({yamlFile1.FullName}). Re-build after editing the config."


            let assemblyHash =
                File.ReadAllBytes(Reflection.Assembly.GetExecutingAssembly().Location)
                |> System.Security.Cryptography.SHA256.HashData
                |> BitConverter.ToString

            let configsDto, rulesetDtosAndConfigs =
                FacilConfig.getRuleSets projectDir yamlFilePath

            for rulesetsDto, cfg in rulesetDtosAndConfigs do

                let scriptsWithoutParamsOrResultSetsOrTempTables =
                    cfg.Scripts
                    |> List.collect (fun rule ->
                        match rule.IncludeOrFor with
                        | For _ -> []
                        | Include pattern ->
                            let exceptMatches =
                                match rule.Except with
                                | Some pattern ->
                                    let matches = Glob.Files(cfg.ScriptBasePath, pattern) |> set

                                    if matches.IsEmpty then
                                        logYamlWarning
                                            yamlFilePath
                                            0
                                            0
                                            $"The 'except' glob pattern '{pattern}' does not match any files"

                                    matches
                                | None -> Set.empty

                            let includeMatches = Glob.Files(cfg.ScriptBasePath, pattern) |> set

                            if includeMatches.IsEmpty then
                                logYamlWarning
                                    yamlFilePath
                                    0
                                    0
                                    $"The 'include' glob pattern '{pattern}' does not match any files"

                            let finalMatches = includeMatches - exceptMatches

                            if not includeMatches.IsEmpty && finalMatches.IsEmpty then
                                logYamlWarning
                                    yamlFilePath
                                    0
                                    0
                                    $"The 'include' glob pattern '{pattern}' does not match any files after subtracting the corresponding 'except' pattern '{rule.Except.Value}'"

                            Set.toList finalMatches)
                    |> List.map (fun globOutput -> {
                        GlobMatchOutput = globOutput
                        RelativePathSegments =
                            let segmentsWithName =
                                globOutput.Split([| '/'; '\\' |], StringSplitOptions.RemoveEmptyEntries)

                            segmentsWithName[0 .. segmentsWithName.Length - 2] |> Array.toList
                        NameWithoutExtension = Path.GetFileNameWithoutExtension globOutput
                        Source =
                            File.ReadAllLines(Path.Combine(cfg.ScriptBasePath, globOutput))
                            |> String.concat "\n"
                        Parameters = []
                        ResultSet = None
                        TempTables = []
                        GeneratedByFacil = false
                    })

                let hash =
                    [
                        assemblyHash
                        serializeForHash configsDto
                        serializeForHash rulesetsDto
                        yield!
                            scriptsWithoutParamsOrResultSetsOrTempTables
                            |> List.map (fun s -> s.GlobMatchOutput.Replace("\\", "/"))
                        yield! scriptsWithoutParamsOrResultSetsOrTempTables |> List.map (fun s -> s.Source)
                    ]
                    |> String.concat ""
                    |> Text.Encoding.UTF8.GetBytes
                    |> System.Security.Cryptography.SHA256.HashData
                    |> BitConverter.ToString
                    |> fun s -> s.Replace("-", "").ToLowerInvariant()

                let outFile = Path.Combine(projectDir, cfg.Filename)

                let regenerate () =
                    Console.WriteLine($"Facil : Regenerating {outFile}")
                    let sw = Diagnostics.Stopwatch.StartNew()

                    use conn =
                        try
                            new SqlConnection(cfg.ConnectionString.Value)
                        with :? ArgumentException ->
                            failwithError "Invalid connection string"

                    conn.Open()

                    let everything =
                        Db.getEverything
                            cfg
                            yamlFilePath
                            scriptsWithoutParamsOrResultSetsOrTempTables
                            cfg.ConnectionString.Value
                            conn

                    let lines = Render.renderDocument cfg hash everything

                    if
                        Environment.GetEnvironmentVariable(envvar_fail_on_changed_output)
                        |> isNull
                        |> not
                    then
                        let existingLines =
                            if File.Exists outFile then
                                File.ReadAllLines(outFile) |> Array.toList
                            else
                                []

                        let shouldCheckLine (line: string) =
                            not <| line.Trim().StartsWith("//", StringComparison.Ordinal)

                        let linesToCheck =
                            lines |> List.toArray |> Array.collect (fun s -> s.Split Environment.NewLine)

                        let existingLinesToCheck = existingLines |> List.toArray

                        for i in [ 0 .. (max linesToCheck.Length existingLinesToCheck.Length) - 1 ] do
                            let lNew = linesToCheck |> Array.tryItem i
                            let lOld = existingLinesToCheck |> Array.tryItem i

                            let shouldFail =
                                match lNew, lOld with
                                | Some lNew, Some lOld ->
                                    if not (shouldCheckLine lNew) && not (shouldCheckLine lOld) then
                                        false
                                    else
                                        lNew <> lOld
                                | None, Some _
                                | Some _, None -> true
                                | None, None -> false // Should never happen

                            if shouldFail then
                                failwithError
                                    $"""The generated code has changed and the environment variable %s{envvar_fail_on_changed_output} is set. Failing build.\n\nIndex of first different line:%i{i}\n\nExisting line:\n%s{defaultArg lOld "<missing line>"}\n\nNew line:\n{defaultArg lNew "<missing line>"}"""

                    // Writing the file may fail if the target projects has multiple target
                    // frameworks that are built in parallel, such as is the case with Facil's own
                    // unit test project. A simple wait-and-retry with jitter fixes that.
                    let rec tryWrite retriesLeft =
                        try
                            File.WriteAllLines(outFile, lines, Text.Encoding.UTF8)
                        with :? IOException when retriesLeft > 0 ->
                            Console.WriteLine(
                                $"Facil : Unable to access file {outFile}, retrying {retriesLeft} more times"
                            )

                            let msToWait = Random().Next(300, 700)
                            Threading.Thread.Sleep msToWait
                            tryWrite (retriesLeft - 1)

                    tryWrite 5

                    sw.Stop()
                    Console.WriteLine($"Facil : Completed regeneration of {outFile} in %.3f{sw.Elapsed.TotalSeconds}s")

                if Environment.GetEnvironmentVariable(envvar_force_regenerate) |> isNull |> not then
                    Console.WriteLine($"Facil : Found environment variable {envvar_force_regenerate}")
                    regenerate ()
                elif File.Exists(outFile) then
                    let lines = File.ReadAllLines(outFile)
                    let firstLineOk = lines |> Array.tryItem 0 = Some Render.firstLine
                    let hashOk = lines |> Array.tryItem 1 = Some(Render.secondLineWithHash hash)

                    if firstLineOk && hashOk then
                        Console.WriteLine($"Facil : Skipping regeneration of up-to-date file {outFile}")
                    else
                        regenerate ()
                else
                    regenerate ()

            0

        with
        | FacilException logError ->
            logError ()
            1
        | ex ->
            logError (string ex)
            1
