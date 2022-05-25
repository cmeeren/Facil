module OptionAndVoptionTests

open Expecto
open Swensen.Unquote


// The primary goal of tests in this file is to test Option and ValueOption functionality


[<Tests>]
let optionTests =
  testList "Option tests" [


    testList (nameof DbGen.Procedures.dbo.ProcOptionIn) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionIn_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionIn
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionIn + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionIn_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionIn
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionOut) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionOut
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionOut + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionOut
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionTableOutWithDto) [
      yield!
        allSeqExecuteMethods<DbGen.Procedures.dbo.ProcOptionTableOutWithDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionTableOutWithDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList

              let expected : DbGen.TableDtos.dbo.OptionTableWithDto list =
                [
                  {
                    Col1 = Some "test1"
                    Col2 = Some 1
                  }
                  {
                    Col1 = None
                    Col2 = None
                  }
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionTableOutWithoutDto) [
      yield!
        allSeqExecuteMethods<DbGen.Procedures.dbo.ProcOptionTableOutWithoutDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionTableOutWithoutDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList
                |> List.map (fun r -> {| r with Col1 = r.Col1 |})

              let expected =
                [
                  {|
                    Col1 = Some "test1"
                    Col2 = Some 1
                  |}
                  {|
                    Col1 = None
                    Col2 = None
                  |}
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionTvpInOut) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionTvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionTvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNull.create(Foo = Some 1, Bar = Some "test")])
                |> exec

              test <@ res.Value.Foo = Some 1 @>
              test <@ res.Value.Bar = Some "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcOptionTvpInOut + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcOptionTvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcOptionTvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNull.create(Foo = None, Bar = None)])
                |> exec

              test <@ res.Value.Foo = None @>
              test <@ res.Value.Bar = None @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.In) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.In_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.In
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.In + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.In_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.In
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.Out) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.Out_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.Out
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.Out + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.Out_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.Out
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.TableOutWithDto) [
      yield!
        allSeqExecuteMethods<DbGen.Scripts.Option.TableOutWithDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.TableOutWithDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList

              let expected : DbGen.TableDtos.dbo.OptionTableWithDto list =
                [
                  {
                    Col1 = Some "test1"
                    Col2 = Some 1
                  }
                  {
                    Col1 = None
                    Col2 = None
                  }
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.TableOutWithoutDto) [
      yield!
        allSeqExecuteMethods<DbGen.Scripts.Option.TableOutWithoutDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.TableOutWithoutDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList
                |> List.map (fun r -> {| r with Col1 = r.Col1 |})

              let expected =
                [
                  {|
                    Col1 = Some "test1"
                    Col2 = Some 1
                  |}
                  {|
                    Col1 = None
                    Col2 = None
                  |}
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.TvpInOut) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.TvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.TvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNull.create(Foo = Some 1, Bar = Some "test")])
                |> exec

              test <@ res.Value.Foo = Some 1 @>
              test <@ res.Value.Bar = Some "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Option.TvpInOut + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Option.TvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Option.TvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNull.create(Foo = None, Bar = None)])
                |> exec

              test <@ res.Value.Foo = None @>
              test <@ res.Value.Bar = None @>
        )
    ]

  ]



[<Tests>]
let voptionTests =
  testList "ValueOption tests" [


    testList (nameof DbGen.Procedures.dbo.ProcVoptionIn) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcVoptionIn_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionIn
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = ValueSome "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionIn + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcVoptionIn_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionIn
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = ValueNone)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionOut) [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Procedures.dbo.ProcVoptionOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionOut
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              let res = res.Value
              test <@ res = ValueSome "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionOut + "_null") [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Procedures.dbo.ProcVoptionOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionOut
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              let res = res.Value
              test <@ res = ValueNone @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionTableOutWithDto) [
      yield!
        allSeqExecuteMethods<DbGen.Procedures.dbo.ProcVoptionTableOutWithDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionTableOutWithDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList

              let expected : DbGen.TableDtos.dbo.VoptionTableWithDto list =
                [
                  {
                    Col1 = ValueSome "test1"
                    Col2 = ValueSome 1
                  }
                  {
                    Col1 = ValueNone
                    Col2 = ValueNone
                  }
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionTableOutWithoutDto) [
      yield!
        allSeqExecuteMethods<DbGen.Procedures.dbo.ProcVoptionTableOutWithoutDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionTableOutWithoutDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList
                |> List.map (fun r -> {| r with Col1 = r.Col1 |})

              let expected =
                [
                  {|
                    Col1 = ValueSome "test1"
                    Col2 = ValueSome 1
                  |}
                  {|
                    Col1 = ValueNone
                    Col2 = ValueNone
                  |}
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionTvpInOut) [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Procedures.dbo.ProcVoptionTvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionTvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNullVoption.create(Foo = ValueSome 1, Bar = ValueSome "test")])
                |> exec

              let res = res.Value
              test <@ res.Foo = ValueSome 1 @>
              test <@ res.Bar = ValueSome "test" @>
        )
    ]


    testList (nameof DbGen.Procedures.dbo.ProcVoptionTvpInOut + "_null") [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Procedures.dbo.ProcVoptionTvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Procedures.dbo.ProcVoptionTvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNullVoption.create(Foo = ValueNone, Bar = ValueNone)])
                |> exec

              let res = res.Value
              test <@ res.Foo = ValueNone @>
              test <@ res.Bar = ValueNone @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.In) [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Voption.In_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.In
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = ValueSome "test")
                |> exec
              test <@ res.Value = Some "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.In + "_null") [
      yield!
        allExecuteMethodsAsSingle<DbGen.Scripts.Voption.In_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.In
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = ValueNone)
                |> exec
              test <@ res.Value = None @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.Out) [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Scripts.Voption.Out_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.Out
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = Some "test")
                |> exec
              let res = res.Value
              test <@ res = ValueSome "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.Out + "_null") [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Scripts.Voption.Out_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.Out
                  .WithConnection(Config.connStr)
                  .WithParameters(param1 = None)
                |> exec
              let res = res.Value
              test <@ res = ValueNone @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.TableOutWithDto) [
      yield!
        allSeqExecuteMethods<DbGen.Scripts.Voption.TableOutWithDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.TableOutWithDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList

              let expected : DbGen.TableDtos.dbo.VoptionTableWithDto list =
                [
                  {
                    Col1 = ValueSome "test1"
                    Col2 = ValueSome 1
                  }
                  {
                    Col1 = ValueNone
                    Col2 = ValueNone
                  }
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.TableOutWithoutDto) [
      yield!
        allSeqExecuteMethods<DbGen.Scripts.Voption.TableOutWithoutDto, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.TableOutWithoutDto
                  .WithConnection(Config.connStr)
                |> exec
                |> Seq.toList
                |> List.map (fun r -> {| r with Col1 = r.Col1 |})

              let expected =
                [
                  {|
                    Col1 = ValueSome "test1"
                    Col2 = ValueSome 1
                  |}
                  {|
                    Col1 = ValueNone
                    Col2 = ValueNone
                  |}
                ]

              test <@ res = expected @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.TvpInOut) [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Scripts.Voption.TvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.TvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNullVoption.create(Foo = ValueSome 1, Bar = ValueSome "test")])
                |> exec

              let res = res.Value
              test <@ res.Foo = ValueSome 1 @>
              test <@ res.Bar = ValueSome "test" @>
        )
    ]


    testList (nameof DbGen.Scripts.Voption.TvpInOut + "_null") [
      yield!
        allExecuteMethodsAsSingleVoption<DbGen.Scripts.Voption.TvpInOut_Executable, _>
        |> List.map (fun (name, exec) ->
            testCase name <| fun () ->
              let res =
                DbGen.Scripts.Voption.TvpInOut
                  .WithConnection(Config.connStr)
                  .WithParameters(tvp = [DbGen.TableTypes.dbo.MultiColNullVoption.create(Foo = ValueNone, Bar = ValueNone)])
                |> exec

              let res = res.Value
              test <@ res.Foo = ValueNone @>
              test <@ res.Bar = ValueNone @>
        )
    ]

  ]
