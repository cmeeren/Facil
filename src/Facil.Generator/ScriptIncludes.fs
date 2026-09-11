module Facil.ScriptIncludes

open System
open System.IO
open System.Text
open System.Text.RegularExpressions


let private opening = Regex(@"\{\{[ \t]*include\b")

let private directive =
    Regex("""\G\{\{[ \t]*include[ \t]+"(?<path>[^"\r\n]+)"[ \t]*\}\}""")


let readSource scriptBasePath relativePath expandIncludes =
    let root = Path.GetFullPath(scriptBasePath)
    let entryPath = Path.GetFullPath(relativePath, root)
    let source = File.ReadAllLines(entryPath) |> String.concat "\n"

    if not expandIncludes then
        source
    else
        let pathComparison =
            if OperatingSystem.IsWindows() then
                StringComparison.OrdinalIgnoreCase
            else
                StringComparison.Ordinal

        let rec expand path chain (source: string) : string =
            let result = StringBuilder()
            let mutable offset = 0
            let mutable token = opening.Match(source)

            while token.Success do
                let fail includeChain reason =
                    let beforeToken = source.Substring(0, token.Index)
                    let line = 1 + (beforeToken |> Seq.filter ((=) '\n') |> Seq.length)
                    let column = token.Index - beforeToken.LastIndexOf('\n')

                    let chainText =
                        includeChain
                        |> List.map (fun p -> Path.GetRelativePath(root, p).Replace('\\', '/'))
                        |> String.concat " -> "

                    failwith $"%s{path}(%i{line},%i{column}): %s{reason}\nInclude chain: %s{chainText}"

                let matched = directive.Match(source, token.Index)

                if not matched.Success then
                    fail chain "Invalid include directive. Expected {{include \"relative/path.sql\"}}"

                let includePath = matched.Groups["path"].Value

                if Path.IsPathRooted(includePath) then
                    fail chain $"Include path must be relative: '%s{includePath}'"

                let target =
                    try
                        Path.GetFullPath(includePath, Path.GetDirectoryName(path))
                    with :? ArgumentException as ex ->
                        fail chain $"Invalid include path '%s{includePath}': %s{ex.Message}"

                let targetRelativePath = Path.GetRelativePath(root, target)
                let targetChain = chain @ [ target ]

                if
                    Path.IsPathRooted(targetRelativePath)
                    || targetRelativePath = ".."
                    || targetRelativePath.StartsWith(
                        ".." + string Path.DirectorySeparatorChar,
                        StringComparison.Ordinal
                    )
                then
                    fail targetChain $"Include path is outside scriptBasePath: '%s{includePath}'"

                if
                    chain
                    |> List.exists (fun ancestor -> String.Equals(ancestor, target, pathComparison))
                then
                    fail targetChain "Include cycle detected"

                let includedSource =
                    try
                        let mutable pathToCheck = root

                        for segment in targetRelativePath.Split(Path.DirectorySeparatorChar) do
                            pathToCheck <- Path.Combine(pathToCheck, segment)

                            if File.GetAttributes(pathToCheck).HasFlag(FileAttributes.ReparsePoint) then
                                fail
                                    targetChain
                                    $"Include path contains a symbolic link or reparse point: '%s{pathToCheck}'"

                        File.ReadAllText(target).Replace("\r\n", "\n").Replace('\r', '\n')
                    with
                    | :? IOException as ex ->
                        fail targetChain $"Unable to read include '%s{includePath}': %s{ex.Message}"
                    | :? UnauthorizedAccessException as ex ->
                        fail targetChain $"Unable to read include '%s{includePath}': %s{ex.Message}"

                result.Append(source, offset, token.Index - offset) |> ignore
                result.Append(expand target targetChain includedSource) |> ignore
                offset <- matched.Index + matched.Length
                token <- opening.Match(source, offset)

            result.Append(source, offset, source.Length - offset).ToString()

        expand entryPath [ entryPath ] source
