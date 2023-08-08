module ConnectionAndDisposalTests

open Microsoft.Data.SqlClient
open Expecto
open Swensen.Unquote


[<Tests>]
let tests =
    testList "Connection and disposal tests" [

        testList "Can use existing open connection for multiple requests - query" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        use conn = new SqlConnection(Config.connStr)
                        conn.Open()

                        for _ in [ 1..3 ] do
                            do! Async.Sleep 200
                            let res = DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(conn) |> exec
                            test <@ res.Value.TableCol1 = "test1" @>
                            test <@ res.Value.TableCol2 = Some 1 @>
                    }
                )

        ]


        testList "Can use existing open connection for multiple requests - non-query" [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResults_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        use conn = new SqlConnection(Config.connStr)
                        conn.Open()

                        for _ in [ 1..3 ] do
                            do! Async.Sleep 200

                            DbGen.Procedures.dbo.ProcWithNoResults
                                .WithConnection(conn)
                                .WithParameters(foo = 2)
                            |> exec
                            |> ignore
                    }
                )

        ]


        testList "Disposes managed connections and commands - query" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let mutable connDisposed = false
                        let mutable cmdDisposed = false

                        DbGen.Procedures.dbo.ProcSelectFromTable
                            .WithConnection(
                                Config.connStr,
                                fun conn -> conn.Disposed.Add(fun _ -> connDisposed <- true)
                            )
                            .ConfigureCommand(fun cmd -> cmd.Disposed.Add(fun _ -> cmdDisposed <- true))
                        |> exec
                        |> ignore

                        do! Async.Sleep 100

                        let connDisposed = connDisposed
                        let cmdDisposed = cmdDisposed
                        test <@ connDisposed = true @>
                        test <@ cmdDisposed = true @>
                    }
                )
        ]


        testList "Disposes managed connections and commands - non-query" [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResults_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let mutable connDisposed = false
                        let mutable cmdDisposed = false

                        DbGen.Procedures.dbo.ProcWithNoResults
                            .WithConnection(
                                Config.connStr,
                                fun conn -> conn.Disposed.Add(fun _ -> connDisposed <- true)
                            )
                            .ConfigureCommand(fun cmd -> cmd.Disposed.Add(fun _ -> cmdDisposed <- true))
                            .WithParameters(foo = 2)
                        |> exec
                        |> ignore

                        do! Async.Sleep 100

                        let connDisposed = connDisposed
                        let cmdDisposed = cmdDisposed
                        test <@ connDisposed = true @>
                        test <@ cmdDisposed = true @>
                    }
                )
        ]


        testList "Disposes commands but not unmanaged connections - query" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let mutable connDisposed = false
                        let mutable cmdDisposed = false

                        use conn = new SqlConnection(Config.connStr)
                        conn.Disposed.Add(fun _ -> connDisposed <- true)
                        conn.Open()

                        DbGen.Procedures.dbo.ProcSelectFromTable
                            .WithConnection(conn)
                            .ConfigureCommand(fun cmd -> cmd.Disposed.Add(fun _ -> cmdDisposed <- true))
                        |> exec
                        |> ignore

                        do! Async.Sleep 100

                        let connDisposed = connDisposed
                        let cmdDisposed = cmdDisposed
                        test <@ connDisposed = false @>
                        test <@ cmdDisposed = true @>
                    }
                )
        ]


        testList "Disposes commands but not unmanaged connections - non-query" [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResults_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let mutable connDisposed = false
                        let mutable cmdDisposed = false

                        use conn = new SqlConnection(Config.connStr)
                        conn.Disposed.Add(fun _ -> connDisposed <- true)
                        conn.Open()

                        DbGen.Procedures.dbo.ProcWithNoResults
                            .WithConnection(conn)
                            .ConfigureCommand(fun cmd -> cmd.Disposed.Add(fun _ -> cmdDisposed <- true))
                            .WithParameters(foo = 2)
                        |> exec
                        |> ignore

                        do! Async.Sleep 100

                        let connDisposed = connDisposed
                        let cmdDisposed = cmdDisposed
                        test <@ connDisposed = false @>
                        test <@ cmdDisposed = true @>
                    }
                )
        ]


        testList "Can use transactions" [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcInsert, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let preTestCount =
                            DbGen.Procedures.dbo.ProcSelectFromTable
                                .WithConnection(Config.connStr)
                                .Execute()
                            |> Seq.length

                        let doTest () =
                            use conn = new SqlConnection(Config.connStr)
                            conn.Open()
                            use tran = conn.BeginTransaction()
                            DbGen.Procedures.dbo.ProcInsert.WithConnection(conn, tran) |> exec |> ignore
                            tran.Rollback()

                        doTest ()

                        let postTestCount =
                            DbGen.Procedures.dbo.ProcSelectFromTable
                                .WithConnection(Config.connStr)
                                .Execute()
                            |> Seq.length

                        test <@ preTestCount = postTestCount @>
                )
        ]

    ]
