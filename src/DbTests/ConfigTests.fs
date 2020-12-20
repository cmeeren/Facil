module ConfigTests

open Expecto
open Swensen.Unquote


[<Tests>]
let tests =
  testList "Config tests" [
    
      testList "Single-col record result set - procedure" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleRecordCol, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Procedures.dbo.ProcWithSingleRecordCol
                    .WithConnection(Config.connStr)
                  |> exec
                test <@ res.Value.Test = 1 @>
          )
      ]


      testList "Single-col record result set - script" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Scripts.SingleRecordCol, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let res =
                  DbGen.Scripts.SingleRecordCol
                    .WithConnection(Config.connStr)
                  |> exec
                test <@ res.Value.Test = 1 @>
          )
      ]


      testCase "Prelude works" <| fun () ->
        test <@ DbGen.MyPreludeModule.x = 2 @>


      testList "Parameter DTO name overrides - procedure" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithOverriddenDtoParamName_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let normalParamRes =
                  DbGen.Procedures.dbo.ProcWithOverriddenDtoParamName
                    .WithConnection(Config.connStr)
                    .WithParameters(nameToBeOverridden = 3)
                  |> exec
                test <@ normalParamRes.Value = Some 3 @>

                let dtoParamRes =
                  DbGen.Procedures.dbo.ProcWithOverriddenDtoParamName
                    .WithConnection(Config.connStr)
                    .WithParameters({| newDtoParamName = 3 |})
                  |> exec
                test <@ dtoParamRes.Value = Some 3 @>
          )
      ]


      testList "Parameter DTO name overrides - script" [
        yield!
          allExecuteMethodsAsSingle<DbGen.Scripts.OverriddenDtoParamName_Executable, _>
          |> List.map (fun (name, exec) ->
              testCase name <| fun () ->
                let normalParamRes =
                  DbGen.Scripts.OverriddenDtoParamName
                    .WithConnection(Config.connStr)
                    .WithParameters(nameToBeOverridden = 3)
                  |> exec
                test <@ normalParamRes.Value = Some 3 @>

                let dtoParamRes =
                  DbGen.Scripts.OverriddenDtoParamName
                    .WithConnection(Config.connStr)
                    .WithParameters({| newDtoParamName = 3 |})
                  |> exec
                test <@ dtoParamRes.Value = Some 3 @>
          )
      ]

  ]
