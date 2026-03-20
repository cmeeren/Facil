module PackageSmokeTests

open System
open System.Diagnostics
open System.IO
open System.Security
open System.Text.Json
open System.Xml.Linq
open Expecto


let rec private tryFindRepoRoot startDir =
    if String.IsNullOrWhiteSpace startDir || not (Directory.Exists startDir) then
        None
    else
        let dir = Path.GetFullPath startDir
        let propsPath = Path.Combine(dir, "Directory.Build.props")
        let fixtureRoot = Path.Combine(dir, "src", "PackageTests", "PackageSmokeFixture")

        if File.Exists propsPath && Directory.Exists fixtureRoot then
            Some dir
        else
            let parent = Directory.GetParent dir

            if isNull parent then
                None
            else
                tryFindRepoRoot parent.FullName


let private repoRoot =
    [
        Directory.GetCurrentDirectory()
        AppContext.BaseDirectory
        __SOURCE_DIRECTORY__
    ]
    |> Seq.choose tryFindRepoRoot
    |> Seq.tryHead
    |> Option.defaultWith (fun () ->
        failwith "Could not locate the repository root from the current PackageTests runtime context."
    )


let private packageTestsProjectRoot = Path.Combine(repoRoot, "src", "PackageTests")


let private packageProjectPath =
    Path.Combine(repoRoot, "src", "Facil.Package", "Facil.Package.fsproj")


let private packageFixtureRoot =
    Path.Combine(packageTestsProjectRoot, "PackageSmokeFixture")


let private packageSmokeTempRoot =
    Path.Combine(packageTestsProjectRoot, "obj", "PackageSmoke")


let private packageVersion =
    let propsPath = Path.Combine(repoRoot, "Directory.Build.props")
    let props = XDocument.Load propsPath

    props.Root.Descendants(XName.Get "Version") |> Seq.exactlyOne |> _.Value


let private packagePath =
    Path.Combine(repoRoot, "nupkg", $"Facil.%s{packageVersion}.nupkg")


let private localPackageSource = Path.Combine(repoRoot, "nupkg")


let private appSettingsPath =
    Path.Combine(repoRoot, "src", "DbTests.DbGen", "appsettings.json")


let private runProcess workingDirectory (envVars: (string * string option) list) fileName arguments =
    use proc = new Process()

    proc.StartInfo.FileName <- fileName
    proc.StartInfo.Arguments <- arguments
    proc.StartInfo.WorkingDirectory <- workingDirectory
    proc.StartInfo.UseShellExecute <- false
    proc.StartInfo.RedirectStandardOutput <- true
    proc.StartInfo.RedirectStandardError <- true

    for key, value in envVars do
        match value with
        | Some value -> proc.StartInfo.Environment[key] <- value
        | None -> proc.StartInfo.Environment.Remove key |> ignore

    if not (proc.Start()) then
        failwith $"Failed to start %s{fileName}"

    let stdoutTask = proc.StandardOutput.ReadToEndAsync()
    let stderrTask = proc.StandardError.ReadToEndAsync()

    proc.WaitForExit()

    let stdout = stdoutTask.Result.Trim()
    let stderr = stderrTask.Result.Trim()

    if proc.ExitCode <> 0 then
        failtest
            $"""Command failed: {fileName} {arguments}
Working directory: {workingDirectory}
Exit code: {proc.ExitCode}

stdout:
{stdout}

stderr:
{stderr}"""


let rec private copyDirectory sourceDir targetDir =
    Directory.CreateDirectory targetDir |> ignore

    for file in Directory.GetFiles sourceDir do
        File.Copy(file, Path.Combine(targetDir, Path.GetFileName file), true)

    for dir in Directory.GetDirectories sourceDir do
        copyDirectory dir (Path.Combine(targetDir, Path.GetFileName dir))


let private replaceTokens (filePath: string) (replacements: (string * string) list) =
    let updatedContent =
        (File.ReadAllText filePath, replacements)
        ||> List.fold (fun content (token, value) -> content.Replace(token, value))

    File.WriteAllText(filePath, updatedContent)


let private getConnectionString () =
    let envConnectionString =
        Environment.GetEnvironmentVariable("connectionString")
        |> Option.ofObj
        |> Option.filter (not << String.IsNullOrWhiteSpace)

    envConnectionString
    |> Option.defaultWith (fun () ->
        use stream = File.OpenRead appSettingsPath
        use doc = JsonDocument.Parse stream

        doc.RootElement.GetProperty("connectionString").GetString()
        |> Option.ofObj
        |> Option.filter (not << String.IsNullOrWhiteSpace)
        |> Option.defaultWith (fun () -> failwith $"Could not find a connection string in {appSettingsPath}")
    )


type private PackageSmokeFixture = {
    Root: string
    FixtureRoot: string
    ProjectDir: string
    ProjectPath: string
    NuGetConfigPath: string
    GeneratedFilePath: string
    PackagesPath: string
}


let private createPackageSmokeFixture root =
    let fixtureRoot = Path.Combine(root, "Fixture")
    let projectDir = Path.Combine(fixtureRoot, "PackageSmoke")

    {
        Root = root
        FixtureRoot = fixtureRoot
        ProjectDir = projectDir
        ProjectPath = Path.Combine(projectDir, "PackageSmoke.fsproj")
        NuGetConfigPath = Path.Combine(fixtureRoot, "nuget.config")
        GeneratedFilePath = Path.Combine(projectDir, "DbGen.fs")
        PackagesPath = Path.Combine(root, "packages")
    }


let private withTemporaryFixture action =
    let root = Path.Combine(packageSmokeTempRoot, Guid.NewGuid().ToString "N")
    let mutable keepArtifacts = true

    try
        try
            let result = action (createPackageSmokeFixture root)
            keepArtifacts <- false
            result
        with ex ->
            failtest $"Package smoke test failed. Temporary files kept at {root}.{Environment.NewLine}{ex.Message}"
    finally
        if not keepArtifacts && Directory.Exists root then
            Directory.Delete(root, true)


let private preparePackageSmokeFixture fixture =
    copyDirectory packageFixtureRoot fixture.FixtureRoot

    replaceTokens fixture.ProjectPath [ "__FACIL_VERSION__", packageVersion ]

    let xmlEscape value = SecurityElement.Escape(value)

    replaceTokens fixture.NuGetConfigPath [
        "__LOCAL_PACKAGE_SOURCE__", xmlEscape localPackageSource
        "__PACKAGES_PATH__", xmlEscape fixture.PackagesPath
    ]

    File.ReadAllText fixture.GeneratedFilePath


[<Tests>]
let tests =
    testSequenced
    <| testList "Package smoke tests" [

        testCase "Packed Facil package builds a clean consumer project and regenerates source"
        <| fun () ->
            withTemporaryFixture
            <| fun fixture ->
                let placeholderContent = preparePackageSmokeFixture fixture

                runProcess repoRoot [] "dotnet" $"pack \"{packageProjectPath}\" -c Release"

                Expect.isTrue (File.Exists packagePath) $"Expected package at {packagePath}"

                runProcess
                    fixture.ProjectDir
                    []
                    "dotnet"
                    $"restore \"{fixture.ProjectPath}\" --configfile \"{fixture.NuGetConfigPath}\""

                let envVars = [
                    "FACIL_FORCE_REGENERATE", Some "1"
                    "FACIL_FAIL_ON_CHANGED_OUTPUT", None
                    "connectionString", Some(getConnectionString ())
                ]

                runProcess
                    fixture.ProjectDir
                    envVars
                    "dotnet"
                    $"build \"{fixture.ProjectPath}\" -c Release --no-restore"

                let generatedContent = File.ReadAllText fixture.GeneratedFilePath

                Expect.notEqual generatedContent placeholderContent "Expected DbGen.fs to be regenerated"
                Expect.stringContains generatedContent "GeneratedCode(\"Facil\"" "Expected Facil-generated source"
    ]
