module ConfigMergeTests

open Expecto
open Swensen.Unquote
open Facil.Config


[<Tests>]
let tests =
    testList "Config merge tests" [

        testCase "Procedure/script parameter merge preserves buildValue fallback"
        <| fun () ->
            let merged =
                ProcedureOrScriptParameter.merge
                    {
                        DtoName = Some "dtoName"
                        BuildValue = Some "buildValue"
                    }
                    { DtoName = None; BuildValue = None }

            test <@ merged.BuildValue = Some "buildValue" @>
    ]
