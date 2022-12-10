[<AutoOpen>]
module internal Facil.Utils


open System


exception FacilException of printMsg: (unit -> unit)

// Format:
// https://docs.microsoft.com/en-us/cpp/build/formatting-the-output-of-a-custom-build-step-or-build-event?view=msvc-160
// Potentially also relevant:
// https://docs.microsoft.com/en-us/visualstudio/msbuild/exec-task?view=vs-2019

let private log warnOrErr msg =
  Console.WriteLine($"Facil : %s{warnOrErr} : %s{msg}")


let private logYaml fullPath lineNo colNo warnOrErr msg =
  Console.WriteLine($"%s{fullPath}(%i{lineNo} , %i{colNo}) : %s{warnOrErr} : %s{msg}")

let logWarning = log "warning"

let logError = log "error"

let logYamlWarning fullPath lineNo colNo msg = logYaml fullPath lineNo colNo "warning" msg

let logYamlError fullPath lineNo colNo msg = logYaml fullPath lineNo colNo "error" msg

let failwithError<'a> msg : 'a = raise (FacilException (fun () -> logError msg))

let failwithYamlError<'a> fullPath lineNo colNo msg : 'a =
  raise (FacilException (fun () -> logYamlError fullPath lineNo colNo msg))



module String =

  let firstUpper (s: string) =
    (s |> Seq.head).ToString().ToUpperInvariant() + s.Substring(1)

  let firstLower (s: string) =
    (s |> Seq.head).ToString().ToLowerInvariant() + s.Substring(1)

  let toUpper (s: string) =
    s.ToUpperInvariant()

  let trimStart (trim: char) (s: string) =
    s.TrimStart(trim)

  let getDeindentedLines (s: string) =
    let s =
      s.Split('\n')
      |> Array.toList
      |> List.map (fun s -> s.TrimEnd('\r').Replace("\t", "  "))
    let minIndent =
      s
      |> List.map (fun s -> s |> Seq.takeWhile ((=) ' ') |> Seq.length)
      |> List.min
    s |> List.map (fun s -> s.Substring(minIndent))



module List =

  let mapAllExceptFirst f list =
    list |> List.mapi (fun i x -> if i <> 0 then f x else x)

  let mapAllExceptLast f list =
    let length = List.length list
    list |> List.mapi (fun i x -> if i <> length - 1 then f x else x)



module Option =

  let teeNone f (opt: 'a option) =
    if opt.IsNone then f ()
    opt



module Map =

  /// Merges the two maps, calling the specified function for keys found in both maps to compute the new value for that
  /// key. The function receives the value from 'a' as the first parameter and from 'b' as the second parameter.
  let merge f a b =
    (a, b)
    ||> Map.fold (fun s k valueB ->
      match Map.tryFind k s with
      | Some valueA -> Map.add k (f valueA valueB) s
      | None -> Map.add k valueB s
    )
