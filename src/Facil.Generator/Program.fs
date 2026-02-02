namespace Facil

open System
open System.IO
open System.Text.Json
open Microsoft.Data.SqlClient
open GlobExpressions


module private Md5 =

    let hashBytes (bytes: byte[]) =
        System.Security.Cryptography.MD5.HashData bytes
        |> Convert.ToHexString
        |> _.ToLowerInvariant()

    let hashString (value: string) =
        value |> Text.Encoding.UTF8.GetBytes |> hashBytes


module Program =


    let envvar_force_regenerate = "FACIL_FORCE_REGENERATE"
    let envvar_fail_on_changed_output = "FACIL_FAIL_ON_CHANGED_OUTPUT"
    let envvar_fail_on_regenerate = "FACIL_FAIL_ON_REGENERATE"


    let serializerOptionsForHash = JsonSerializerOptions()
    serializerOptionsForHash.Converters.Add(System.Text.Json.Serialization.JsonFSharpConverter())

    let serializeForHash (x: 'a) =
        JsonSerializer.Serialize<'a>(x, serializerOptionsForHash)


    [<EntryPoint>]
    let main argv =
        try

            let projectDir =
                if argv.Length = 0 then
                    @"..\..\..\..\DbTests.DbGen"
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
                |> Md5.hashBytes

            let configsDto, rulesetDtosAndConfigs =
                FacilConfig.getRuleSets projectDir yamlFilePath

            let configsHash = serializeForHash configsDto |> Md5.hashString

            for rulesetsDto, cfg in rulesetDtosAndConfigs do

                let scriptsWithoutParamsOrResultSetsOrTempTables =
                    cfg.Scripts
                    |> List.collect (fun rule ->
                        match rule.IncludeOrFor with
                        | For _ -> []
                        | Include pattern ->
                            let exceptMatches =
                                match rule.Except with
                                | Some pattern -> Glob.Files(cfg.ScriptBasePath, pattern) |> set
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

                            Set.toList finalMatches
                    )
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

                let rulesetsHash = serializeForHash rulesetsDto |> Md5.hashString

                let scriptsForManifest =
                    scriptsWithoutParamsOrResultSetsOrTempTables
                    |> List.map (fun s ->
                        let path = s.GlobMatchOutput.Replace("\\", "/")
                        let hash = Md5.hashString s.Source
                        path, hash
                    )
                    |> List.sortBy (fun (path, _) -> path.ToUpperInvariant(), path)

                let headerLines =
                    Render.renderManifestHeader
                        (Render.getFacilVersion ())
                        assemblyHash
                        (Path.GetFileName yamlFilePath)
                        configsHash
                        rulesetsHash
                        scriptsForManifest

                let outFile = Path.Combine(projectDir, cfg.Filename)

                let regenerate () =
                    if Environment.GetEnvironmentVariable(envvar_fail_on_regenerate) |> isNull |> not then
                        failwithError
                            $"Regeneration was triggered and the environment variable %s{envvar_fail_on_regenerate} is set. Failing build."

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

                    let lines = Render.renderDocument cfg headerLines everything

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

                        // This is needed for Facil's own CI pipeline, since the assembly version/hash used for the
                        // checked-in DbGen.fs may be different than the version/hash used on CI.
                        let ignoreLinesWithPrefix = [
                            "\"assemblyVersion\": "
                            "\"assemblyHash\": "
                            "[<System.CodeDom.Compiler.GeneratedCode("
                            "//"
                        ]

                        let shouldCheckLine (line: string) =
                            ignoreLinesWithPrefix |> List.forall (not << line.Trim().StartsWith)

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
                    Console.WriteLine(
                        $"Facil : Found environment variable {envvar_force_regenerate}, regenerating {outFile}"
                    )

                    regenerate ()
                elif File.Exists(outFile) then
                    let existingLines = File.ReadAllLines(outFile)

                    let headerMatches =
                        headerLines
                        |> List.mapi (fun index expectedLine ->
                            existingLines |> Array.tryItem index = Some expectedLine
                        )
                        |> List.forall id

                    if headerMatches then
                        Console.WriteLine($"Facil : Skipping regeneration of up-to-date file %s{outFile}")
                    else
                        Console.WriteLine($"Facil : Regenerating %s{outFile}")

                        if Environment.GetEnvironmentVariable(envvar_fail_on_regenerate) |> isNull |> not then
                            let actualHeaderLines =
                                match existingLines |> Array.tryFindIndex (fun line -> line.Trim() = "*)") with
                                | Some index -> existingLines |> Array.take (index + 1)
                                | None -> existingLines |> Array.truncate headerLines.Length

                            Console.WriteLine("Facil : Expected manifest header:")
                            headerLines |> List.iter Console.WriteLine
                            Console.WriteLine("Facil : Actual manifest header:")
                            actualHeaderLines |> Array.iter Console.WriteLine

                        regenerate ()
                else
                    Console.WriteLine($"Facil : File not found; regenerating %s{outFile}")
                    regenerate ()

            0

        with
        | FacilException logError ->
            logError ()
            1
        | ex ->
            logError (string ex)
            1
