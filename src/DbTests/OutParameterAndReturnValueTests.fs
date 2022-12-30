module OutParameterAndReturnValueTests

open Expecto
open Swensen.Unquote


[<Tests>]
let tests =
    testList "Output parameter and return value tests" [


        testList (nameof DbGen.Procedures.dbo.ProcWithRetVal) [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.ReturnValue = 4 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithRetValExtended) [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithRetValExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithRetValExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.ReturnValue = 4 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_unset_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_unset_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_unset_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_unset_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_unset_passedSome_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = false
                                        SetOut2 = false
                                        Out1 = Some 42
                                        Out2 = Some "foo"
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_unset_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_unset_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_unset_passedNone_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = false
                                        SetOut2 = false
                                        Out1 = None
                                        Out2 = None
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_set_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_set_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_set_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_set_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_set_passedSome_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = Some 42
                                        Out2 = Some "foo"
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_set_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsExtended + "_set_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParams + "_set_passedNone_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = None
                                        Out2 = None
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_unset_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>
                        test <@ res.ReturnValue = 3 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_unset_notPassed")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, baseRetVal = 3)
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_unset_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    setOut1 = false,
                                    setOut2 = false,
                                    out1 = 42,
                                    out2 = Some "foo",
                                    baseRetVal = 3
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>
                        test <@ res.ReturnValue = 45 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_unset_passedSome")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        out1 = 42,
                                        out2 = Some "foo",
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_unset_passedSome_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = false
                                        SetOut2 = false
                                        Out1 = Some 42
                                        Out2 = Some "foo"
                                        BaseRetVal = 3
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>
                        test <@ res.ReturnValue = 45 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_unset_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    setOut1 = false,
                                    setOut2 = false,
                                    out1 = 42,
                                    out2 = None,
                                    baseRetVal = 3
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = None @>
                        test <@ res.ReturnValue = 45 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_unset_passedNone")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        out1 = 42,
                                        out2 = None,
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_unset_passedNone_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = false
                                        SetOut2 = false
                                        Out1 = None
                                        Out2 = None
                                        BaseRetVal = 3
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>
                        test <@ res.ReturnValue = 3 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_set_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_set_notPassed")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true, baseRetVal = 3)
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_set_passedSome") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    setOut1 = true,
                                    setOut2 = true,
                                    out1 = 42,
                                    out2 = Some "foo",
                                    baseRetVal = 3
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_set_passedSome")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        out1 = 42,
                                        out2 = Some "foo",
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_set_passedSome_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = Some 42
                                        Out2 = Some "foo"
                                        BaseRetVal = 3
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_set_passedNone") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = None, baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended
             + "_set_passedNone")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        out1 = 42,
                                        out2 = None,
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal + "_set_passedNone_dto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetVal_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = None
                                        Out2 = None
                                        BaseRetVal = 3
                                    |}
                                )
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_unset_notPassed")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, baseRetVal = 3)
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueNone @>
                            test <@ res.Out.out2 = ValueNone @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_unset_passedSome")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        out1 = 42,
                                        out2 = ValueSome "foo",
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 42 @>
                            test <@ res.Out.out2 = ValueSome "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_unset_passedSome_dto")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            Out1 = ValueSome 42
                                            Out2 = ValueSome "foo"
                                            BaseRetVal = 3
                                        |}
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 42 @>
                            test <@ res.Out.out2 = ValueSome "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_unset_passedNone")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        out1 = 42,
                                        out2 = ValueNone,
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 42 @>
                            test <@ res.Out.out2 = ValueNone @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_unset_passedNone_dto")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            Out1 = ValueNone
                                            Out2 = ValueNone
                                            BaseRetVal = 3
                                        |}
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueNone @>
                            test <@ res.Out.out2 = ValueNone @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption + "_set_notPassed") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithOutParamsAndRetValVoption
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, baseRetVal = 3)
                            |> exec

                        test <@ res.Result = -1 @>
                        test <@ res.Out.out1 = ValueSome 123 @>
                        test <@ res.Out.out2 = ValueSome "test" @>
                        test <@ res.ReturnValue = 126 @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_set_passedSome")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        out1 = 42,
                                        out2 = ValueSome "foo",
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 123 @>
                            test <@ res.Out.out2 = ValueSome "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_set_passedSome_dto")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = true
                                            SetOut2 = true
                                            Out1 = ValueSome 42
                                            Out2 = ValueSome "foo"
                                            BaseRetVal = 3
                                        |}
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 123 @>
                            test <@ res.Out.out2 = ValueSome "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_set_passedNone")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        out1 = 42,
                                        out2 = ValueNone,
                                        baseRetVal = 3
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 123 @>
                            test <@ res.Out.out2 = ValueSome "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption
             + "_set_passedNone_dto")
            [
                yield!
                    allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithOutParamsAndRetValVoption_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithOutParamsAndRetValVoption
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = true
                                            SetOut2 = true
                                            Out1 = ValueNone
                                            Out2 = ValueNone
                                            BaseRetVal = 3
                                        |}
                                    )
                                |> exec

                            test <@ res.Result = -1 @>
                            test <@ res.Out.out1 = ValueSome 123 @>
                            test <@ res.Out.out2 = ValueSome "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndRetVal) [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndRetVal_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndRetVal_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndRetVal
                                .WithConnection(Config.connStr)
                                .WithParameters(baseRetVal = 3)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.ReturnValue = 4 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndRetValExtended) [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndRetValExtended_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndRetValExtended_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndRetValExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(baseRetVal = 3)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.ReturnValue = 4 @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_unset_notPassed") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = None @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_unset_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_unset_passedSome") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = Some "foo" @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_unset_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = Some "foo")
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams
             + "_unset_passedSome_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            Out1 = Some 42
                                            Out2 = Some "foo"
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_unset_passedNone") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 42 @>
                        test <@ res.Out.out2 = None @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_unset_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, out1 = 42, out2 = None)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = None @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams
             + "_unset_passedNone_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            Out1 = None
                                            Out2 = None
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_set_notPassed") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_set_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_set_passedSome") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = Some "foo")
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_set_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = Some "foo")
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_set_passedSome_dto") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = Some 42
                                        Out2 = Some "foo"
                                    |}
                                )
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_set_passedNone") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = None)
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended
             + "_set_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true, out1 = 42, out2 = None)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>)
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParams + "_set_passedNone_dto") [
            yield!
                [
                    yield!
                        allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x
                                {| res with Result = res.Result |})
                    yield!
                        allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParams_Executable, _>
                        |> List.map (fun (name, exec) ->
                            name,
                            fun x ->
                                let res = exec x

                                {| res with
                                    Result = res.Result |> Seq.tryHead
                                |})
                ]
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen
                                .Procedures
                                .dbo
                                .ProcWithResultsAndOutParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        SetOut1 = true
                                        SetOut2 = true
                                        Out1 = None
                                        Out2 = None
                                    |}
                                )
                            |> exec

                        test <@ res.Result.Value.Foo = 1 @>
                        test <@ res.Result.Value.Bar = 2 @>
                        test <@ res.Out.out1 = Some 123 @>
                        test <@ res.Out.out2 = Some "test" @>)
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_unset_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, baseRetVal = 3)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_unset_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = false, setOut2 = false, baseRetVal = 3)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_unset_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = Some "foo"
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_unset_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = Some "foo"
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_unset_passedSome_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            BaseRetVal = 3
                                            Out1 = Some 42
                                            Out2 = Some "foo"
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = Some "foo" @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_unset_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = None
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_unset_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = false,
                                        setOut2 = false,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = None
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 42 @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 45 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_unset_passedNone_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = false
                                            SetOut2 = false
                                            BaseRetVal = 3
                                            Out1 = None
                                            Out2 = None
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = None @>
                            test <@ res.Out.out2 = None @>
                            test <@ res.ReturnValue = 3 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_set_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true, baseRetVal = 3)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_set_notPassed")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(setOut1 = true, setOut2 = true, baseRetVal = 3)
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_set_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = Some "foo"
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_set_passedSome")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = Some "foo"
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_set_passedSome_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = true
                                            SetOut2 = true
                                            BaseRetVal = 3
                                            Out1 = Some 42
                                            Out2 = Some "foo"
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_set_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = None
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended
             + "_set_passedNone")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetValExtended_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetValExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        setOut1 = true,
                                        setOut2 = true,
                                        baseRetVal = 3,
                                        out1 = 42,
                                        out2 = None
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal
             + "_set_passedNone_dto")
            [
                yield!
                    [
                        yield!
                            allEagerSingleExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            // Map to anonymous type in this assembly for compabilitity with the mapping of the seq executables
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x
                                    {| res with Result = res.Result |})
                        yield!
                            allEagerSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithResultsAndOutParamsAndRetVal_Executable, _>
                            |> List.map (fun (name, exec) ->
                                name,
                                fun x ->
                                    let res = exec x

                                    {| res with
                                        Result = res.Result |> Seq.tryHead
                                    |})
                    ]
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen
                                    .Procedures
                                    .dbo
                                    .ProcWithResultsAndOutParamsAndRetVal
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            SetOut1 = true
                                            SetOut2 = true
                                            BaseRetVal = 3
                                            Out1 = None
                                            Out2 = None
                                        |}
                                    )
                                |> exec

                            test <@ res.Result.Value.Foo = 1 @>
                            test <@ res.Result.Value.Bar = 2 @>
                            test <@ res.Out.out1 = Some 123 @>
                            test <@ res.Out.out2 = Some "test" @>
                            test <@ res.ReturnValue = 126 @>)
            ]


    ]
