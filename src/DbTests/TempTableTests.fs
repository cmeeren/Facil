module TempTableTests

open System
open System.Collections
open System.Collections.Generic
open Microsoft.Data.SqlClient
open Expecto
open Swensen.Unquote


type FailingTempTableRows<'a>(item: 'a) =
    let mutable wasDisposed = false

    member _.WasDisposed = wasDisposed

    interface IEnumerable<'a> with
        member _.GetEnumerator() =
            let mutable state = 0

            { new IEnumerator<'a> with
                member _.Current =
                    match state with
                    | 1 -> item
                    | _ -> invalidOp "Enumerator is not positioned on an item"

                member _.Dispose() = wasDisposed <- true

              interface IEnumerator with
                  member _.Current =
                      box
                      <| match state with
                         | 1 -> item
                         | _ -> invalidOp "Enumerator is not positioned on an item"

                  member _.MoveNext() =
                      match state with
                      | 0 ->
                          state <- 1
                          true
                      | 1 ->
                          state <- 2
                          raise (InvalidOperationException("Boom"))
                      | _ -> false

                  member _.Reset() = raise (NotSupportedException())
            }

    interface IEnumerable with
        member this.GetEnumerator() =
            (this :> IEnumerable<'a>).GetEnumerator() :> IEnumerator


[<Tests>]
let tests =
    testList "Temp table tests" [


        testList (nameof DbGen.Scripts.TempTableAllTypesNonNull) [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNonNull.AllTypesNonNull.create (
                                bigint = 1L,
                                binary = Array.replicate 42 1uy,
                                bit = true,
                                char = String.replicate 42 "a",
                                date = DateTime(2000, 1, 1),
                                datetime = DateTime(2000, 1, 1),
                                datetime2 = DateTime(2000, 1, 1),
                                datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                decimal = 1M,
                                float = 1.,
                                image = [| 1uy |],
                                int = 1,
                                money = 1M,
                                nchar = String.replicate 42 "a",
                                ntext = "test",
                                numeric = 1M,
                                nvarchar = "test",
                                real = 1.f,
                                smalldatetime = DateTime(2000, 1, 1),
                                smallint = 1s,
                                smallmoney = 1M,
                                text = "test",
                                time = TimeSpan.FromSeconds 1.,
                                tinyint = 1uy,
                                uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                varbinary = [| 1uy |],
                                varchar = "test",
                                xml = "<tag />"
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    allTypesNonNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = 1L @>
                            test <@ row.Binary = Array.replicate 42 1uy @>
                            test <@ row.Bit = true @>
                            test <@ row.Char = String.replicate 42 "a" @>
                            test <@ row.Date = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime2 = DateTime(2000, 1, 1) @>
                            test <@ row.Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ row.Decimal = 1M @>
                            test <@ row.Float = 1. @>
                            test <@ row.Image = [| 1uy |] @>
                            test <@ row.Int = 1 @>
                            test <@ row.Money = 1M @>
                            test <@ row.Nchar = String.replicate 42 "a" @>
                            test <@ row.Ntext = "test" @>
                            test <@ row.Numeric = 1M @>
                            test <@ row.Nvarchar = "test" @>
                            test <@ row.Real = 1.f @>
                            test <@ row.Smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ row.Smallint = 1s @>
                            test <@ row.Smallmoney = 1M @>
                            test <@ row.Text = "test" @>
                            test <@ row.Time = TimeSpan.FromSeconds 1. @>
                            test <@ row.Tinyint = 1uy @>
                            test <@ row.Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ row.Varbinary = [| 1uy |] @>
                            test <@ row.Varchar = "test" @>
                            test <@ row.Xml = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNonNull + "_paramsFromDto") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNonNull.AllTypesNonNull.create (
                                {|
                                    Bigint = 1L
                                    Binary = Array.replicate 42 1uy
                                    Bit = true
                                    Char = String.replicate 42 "a"
                                    Date = DateTime(2000, 1, 1)
                                    Datetime = DateTime(2000, 1, 1)
                                    Datetime2 = DateTime(2000, 1, 1)
                                    Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
                                    Decimal = 1M
                                    Float = 1.
                                    Image = [| 1uy |]
                                    Int = 1
                                    Money = 1M
                                    Nchar = String.replicate 42 "a"
                                    Ntext = "test"
                                    Numeric = 1M
                                    Nvarchar = "test"
                                    Real = 1.f
                                    Smalldatetime = DateTime(2000, 1, 1)
                                    Smallint = 1s
                                    Smallmoney = 1M
                                    Text = "test"
                                    Time = TimeSpan.FromSeconds 1.
                                    Tinyint = 1uy
                                    Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")
                                    Varbinary = [| 1uy |]
                                    Varchar = "test"
                                    Xml = "<tag />"
                                |}
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        AllTypesNonNull = [
                                            createTempTableRow ()
                                            createTempTableRow ()
                                            createTempTableRow ()
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = 1L @>
                            test <@ row.Binary = Array.replicate 42 1uy @>
                            test <@ row.Bit = true @>
                            test <@ row.Char = String.replicate 42 "a" @>
                            test <@ row.Date = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime2 = DateTime(2000, 1, 1) @>
                            test <@ row.Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ row.Decimal = 1M @>
                            test <@ row.Float = 1. @>
                            test <@ row.Image = [| 1uy |] @>
                            test <@ row.Int = 1 @>
                            test <@ row.Money = 1M @>
                            test <@ row.Nchar = String.replicate 42 "a" @>
                            test <@ row.Ntext = "test" @>
                            test <@ row.Numeric = 1M @>
                            test <@ row.Nvarchar = "test" @>
                            test <@ row.Real = 1.f @>
                            test <@ row.Smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ row.Smallint = 1s @>
                            test <@ row.Smallmoney = 1M @>
                            test <@ row.Text = "test" @>
                            test <@ row.Time = TimeSpan.FromSeconds 1. @>
                            test <@ row.Tinyint = 1uy @>
                            test <@ row.Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ row.Varbinary = [| 1uy |] @>
                            test <@ row.Varchar = "test" @>
                            test <@ row.Xml = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNull) [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNull.AllTypesNull.create (
                                bigint = Some 1L,
                                binary = Some(Array.replicate 42 1uy),
                                bit = Some true,
                                char = Some(String.replicate 42 "a"),
                                date = Some(DateTime(2000, 1, 1)),
                                datetime = Some(DateTime(2000, 1, 1)),
                                datetime2 = Some(DateTime(2000, 1, 1)),
                                datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)),
                                decimal = Some 1M,
                                float = Some 1.,
                                image = Some [| 1uy |],
                                int = Some 1,
                                money = Some 1M,
                                nchar = Some(String.replicate 42 "a"),
                                ntext = Some "test",
                                numeric = Some 1M,
                                nvarchar = Some "test",
                                real = Some 1.f,
                                smalldatetime = Some(DateTime(2000, 1, 1)),
                                smallint = Some 1s,
                                smallmoney = Some 1M,
                                text = Some "test",
                                time = Some(TimeSpan.FromSeconds 1.),
                                tinyint = Some 1uy,
                                uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                varbinary = Some [| 1uy |],
                                varchar = Some "test",
                                xml = Some "<tag />"
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    allTypesNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = Some 1L @>
                            test <@ row.Binary = Some(Array.replicate 42 1uy) @>
                            test <@ row.Bit = Some true @>
                            test <@ row.Char = Some(String.replicate 42 "a") @>
                            test <@ row.Date = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime2 = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                            test <@ row.Decimal = Some 1M @>
                            test <@ row.Float = Some 1. @>
                            test <@ row.Image = Some [| 1uy |] @>
                            test <@ row.Int = Some 1 @>
                            test <@ row.Money = Some 1M @>
                            test <@ row.Nchar = Some(String.replicate 42 "a") @>
                            test <@ row.Ntext = Some "test" @>
                            test <@ row.Numeric = Some 1M @>
                            test <@ row.Nvarchar = Some "test" @>
                            test <@ row.Real = Some 1.f @>
                            test <@ row.Smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Smallint = Some 1s @>
                            test <@ row.Smallmoney = Some 1M @>
                            test <@ row.Text = Some "test" @>
                            test <@ row.Time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ row.Tinyint = Some 1uy @>
                            test <@ row.Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ row.Varbinary = Some [| 1uy |] @>
                            test <@ row.Varchar = Some "test" @>
                            test <@ row.Xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNull + "_paramsFromDto") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNull.AllTypesNull.create (
                                {|
                                    Bigint = Some 1L
                                    Binary = Some(Array.replicate 42 1uy)
                                    Bit = Some true
                                    Char = Some(String.replicate 42 "a")
                                    Date = Some(DateTime(2000, 1, 1))
                                    Datetime = Some(DateTime(2000, 1, 1))
                                    Datetime2 = Some(DateTime(2000, 1, 1))
                                    Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                    Decimal = Some 1M
                                    Float = Some 1.
                                    Image = Some [| 1uy |]
                                    Int = Some 1
                                    Money = Some 1M
                                    Nchar = Some(String.replicate 42 "a")
                                    Ntext = Some "test"
                                    Numeric = Some 1M
                                    Nvarchar = Some "test"
                                    Real = Some 1.f
                                    Smalldatetime = Some(DateTime(2000, 1, 1))
                                    Smallint = Some 1s
                                    Smallmoney = Some 1M
                                    Text = Some "test"
                                    Time = Some(TimeSpan.FromSeconds 1.)
                                    Tinyint = Some 1uy
                                    Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                    Varbinary = Some [| 1uy |]
                                    Varchar = Some "test"
                                    Xml = Some "<tag />"
                                |}
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        AllTypesNull = [
                                            createTempTableRow ()
                                            createTempTableRow ()
                                            createTempTableRow ()
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = Some 1L @>
                            test <@ row.Binary = Some(Array.replicate 42 1uy) @>
                            test <@ row.Bit = Some true @>
                            test <@ row.Char = Some(String.replicate 42 "a") @>
                            test <@ row.Date = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime2 = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                            test <@ row.Decimal = Some 1M @>
                            test <@ row.Float = Some 1. @>
                            test <@ row.Image = Some [| 1uy |] @>
                            test <@ row.Int = Some 1 @>
                            test <@ row.Money = Some 1M @>
                            test <@ row.Nchar = Some(String.replicate 42 "a") @>
                            test <@ row.Ntext = Some "test" @>
                            test <@ row.Numeric = Some 1M @>
                            test <@ row.Nvarchar = Some "test" @>
                            test <@ row.Real = Some 1.f @>
                            test <@ row.Smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Smallint = Some 1s @>
                            test <@ row.Smallmoney = Some 1M @>
                            test <@ row.Text = Some "test" @>
                            test <@ row.Time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ row.Tinyint = Some 1uy @>
                            test <@ row.Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ row.Varbinary = Some [| 1uy |] @>
                            test <@ row.Varchar = Some "test" @>
                            test <@ row.Xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNull + "_null") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNull.AllTypesNull.create (
                                bigint = None,
                                binary = None,
                                bit = None,
                                char = None,
                                date = None,
                                datetime = None,
                                datetime2 = None,
                                datetimeoffset = None,
                                decimal = None,
                                float = None,
                                image = None,
                                int = None,
                                money = None,
                                nchar = None,
                                ntext = None,
                                numeric = None,
                                nvarchar = None,
                                real = None,
                                smalldatetime = None,
                                smallint = None,
                                smallmoney = None,
                                text = None,
                                time = None,
                                tinyint = None,
                                uniqueidentifier = None,
                                varbinary = None,
                                varchar = None,
                                xml = None
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    allTypesNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = None @>
                            test <@ row.Binary = None @>
                            test <@ row.Bit = None @>
                            test <@ row.Char = None @>
                            test <@ row.Date = None @>
                            test <@ row.Datetime = None @>
                            test <@ row.Datetime2 = None @>
                            test <@ row.Datetimeoffset = None @>
                            test <@ row.Decimal = None @>
                            test <@ row.Float = None @>
                            test <@ row.Image = None @>
                            test <@ row.Int = None @>
                            test <@ row.Money = None @>
                            test <@ row.Nchar = None @>
                            test <@ row.Ntext = None @>
                            test <@ row.Numeric = None @>
                            test <@ row.Nvarchar = None @>
                            test <@ row.Real = None @>
                            test <@ row.Smalldatetime = None @>
                            test <@ row.Smallint = None @>
                            test <@ row.Smallmoney = None @>
                            test <@ row.Text = None @>
                            test <@ row.Time = None @>
                            test <@ row.Tinyint = None @>
                            test <@ row.Uniqueidentifier = None @>
                            test <@ row.Varbinary = None @>
                            test <@ row.Varchar = None @>
                            test <@ row.Xml = None @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNull + "_null_paramsFromDto") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNull.AllTypesNull.create (
                                {|
                                    Bigint = None
                                    Binary = None
                                    Bit = None
                                    Char = None
                                    Date = None
                                    Datetime = None
                                    Datetime2 = None
                                    Datetimeoffset = None
                                    Decimal = None
                                    Float = None
                                    Image = None
                                    Int = None
                                    Money = None
                                    Nchar = None
                                    Ntext = None
                                    Numeric = None
                                    Nvarchar = None
                                    Real = None
                                    Smalldatetime = None
                                    Smallint = None
                                    Smallmoney = None
                                    Text = None
                                    Time = None
                                    Tinyint = None
                                    Uniqueidentifier = None
                                    Varbinary = None
                                    Varchar = None
                                    Xml = None
                                |}
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        AllTypesNull = [
                                            createTempTableRow ()
                                            createTempTableRow ()
                                            createTempTableRow ()
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = None @>
                            test <@ row.Binary = None @>
                            test <@ row.Bit = None @>
                            test <@ row.Char = None @>
                            test <@ row.Date = None @>
                            test <@ row.Datetime = None @>
                            test <@ row.Datetime2 = None @>
                            test <@ row.Datetimeoffset = None @>
                            test <@ row.Decimal = None @>
                            test <@ row.Float = None @>
                            test <@ row.Image = None @>
                            test <@ row.Int = None @>
                            test <@ row.Money = None @>
                            test <@ row.Nchar = None @>
                            test <@ row.Ntext = None @>
                            test <@ row.Numeric = None @>
                            test <@ row.Nvarchar = None @>
                            test <@ row.Real = None @>
                            test <@ row.Smalldatetime = None @>
                            test <@ row.Smallint = None @>
                            test <@ row.Smallmoney = None @>
                            test <@ row.Text = None @>
                            test <@ row.Time = None @>
                            test <@ row.Tinyint = None @>
                            test <@ row.Uniqueidentifier = None @>
                            test <@ row.Varbinary = None @>
                            test <@ row.Varchar = None @>
                            test <@ row.Xml = None @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNullVoption) [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNullVoption_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNullVoption.AllTypesNull.create (
                                bigint = ValueSome 1L,
                                binary = ValueSome(Array.replicate 42 1uy),
                                bit = ValueSome true,
                                char = ValueSome(String.replicate 42 "a"),
                                date = ValueSome(DateTime(2000, 1, 1)),
                                datetime = ValueSome(DateTime(2000, 1, 1)),
                                datetime2 = ValueSome(DateTime(2000, 1, 1)),
                                datetimeoffset = ValueSome(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)),
                                decimal = ValueSome 1M,
                                float = ValueSome 1.,
                                image = ValueSome [| 1uy |],
                                int = ValueSome 1,
                                money = ValueSome 1M,
                                nchar = ValueSome(String.replicate 42 "a"),
                                ntext = ValueSome "test",
                                numeric = ValueSome 1M,
                                nvarchar = ValueSome "test",
                                real = ValueSome 1.f,
                                smalldatetime = ValueSome(DateTime(2000, 1, 1)),
                                smallint = ValueSome 1s,
                                smallmoney = ValueSome 1M,
                                text = ValueSome "test",
                                time = ValueSome(TimeSpan.FromSeconds 1.),
                                tinyint = ValueSome 1uy,
                                uniqueidentifier = ValueSome(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                varbinary = ValueSome [| 1uy |],
                                varchar = ValueSome "test",
                                xml = ValueSome "<tag />"
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNullVoption
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    allTypesNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = Some 1L @>
                            test <@ row.Binary = Some(Array.replicate 42 1uy) @>
                            test <@ row.Bit = Some true @>
                            test <@ row.Char = Some(String.replicate 42 "a") @>
                            test <@ row.Date = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime2 = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                            test <@ row.Decimal = Some 1M @>
                            test <@ row.Float = Some 1. @>
                            test <@ row.Image = Some [| 1uy |] @>
                            test <@ row.Int = Some 1 @>
                            test <@ row.Money = Some 1M @>
                            test <@ row.Nchar = Some(String.replicate 42 "a") @>
                            test <@ row.Ntext = Some "test" @>
                            test <@ row.Numeric = Some 1M @>
                            test <@ row.Nvarchar = Some "test" @>
                            test <@ row.Real = Some 1.f @>
                            test <@ row.Smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Smallint = Some 1s @>
                            test <@ row.Smallmoney = Some 1M @>
                            test <@ row.Text = Some "test" @>
                            test <@ row.Time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ row.Tinyint = Some 1uy @>
                            test <@ row.Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ row.Varbinary = Some [| 1uy |] @>
                            test <@ row.Varchar = Some "test" @>
                            test <@ row.Xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNullVoption + "_paramsFromDto") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNullVoption_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNullVoption.AllTypesNull.create (
                                {|
                                    Bigint = ValueSome 1L
                                    Binary = ValueSome(Array.replicate 42 1uy)
                                    Bit = ValueSome true
                                    Char = ValueSome(String.replicate 42 "a")
                                    Date = ValueSome(DateTime(2000, 1, 1))
                                    Datetime = ValueSome(DateTime(2000, 1, 1))
                                    Datetime2 = ValueSome(DateTime(2000, 1, 1))
                                    Datetimeoffset = ValueSome(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                    Decimal = ValueSome 1M
                                    Float = ValueSome 1.
                                    Image = ValueSome [| 1uy |]
                                    Int = ValueSome 1
                                    Money = ValueSome 1M
                                    Nchar = ValueSome(String.replicate 42 "a")
                                    Ntext = ValueSome "test"
                                    Numeric = ValueSome 1M
                                    Nvarchar = ValueSome "test"
                                    Real = ValueSome 1.f
                                    Smalldatetime = ValueSome(DateTime(2000, 1, 1))
                                    Smallint = ValueSome 1s
                                    Smallmoney = ValueSome 1M
                                    Text = ValueSome "test"
                                    Time = ValueSome(TimeSpan.FromSeconds 1.)
                                    Tinyint = ValueSome 1uy
                                    Uniqueidentifier = ValueSome(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                    Varbinary = ValueSome [| 1uy |]
                                    Varchar = ValueSome "test"
                                    Xml = ValueSome "<tag />"
                                |}
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNullVoption
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        AllTypesNull = [
                                            createTempTableRow ()
                                            createTempTableRow ()
                                            createTempTableRow ()
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = Some 1L @>
                            test <@ row.Binary = Some(Array.replicate 42 1uy) @>
                            test <@ row.Bit = Some true @>
                            test <@ row.Char = Some(String.replicate 42 "a") @>
                            test <@ row.Date = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetime2 = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                            test <@ row.Decimal = Some 1M @>
                            test <@ row.Float = Some 1. @>
                            test <@ row.Image = Some [| 1uy |] @>
                            test <@ row.Int = Some 1 @>
                            test <@ row.Money = Some 1M @>
                            test <@ row.Nchar = Some(String.replicate 42 "a") @>
                            test <@ row.Ntext = Some "test" @>
                            test <@ row.Numeric = Some 1M @>
                            test <@ row.Nvarchar = Some "test" @>
                            test <@ row.Real = Some 1.f @>
                            test <@ row.Smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.Smallint = Some 1s @>
                            test <@ row.Smallmoney = Some 1M @>
                            test <@ row.Text = Some "test" @>
                            test <@ row.Time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ row.Tinyint = Some 1uy @>
                            test <@ row.Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ row.Varbinary = Some [| 1uy |] @>
                            test <@ row.Varchar = Some "test" @>
                            test <@ row.Xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNullVoption + "_null") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNullVoption_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNullVoption.AllTypesNull.create (
                                bigint = ValueNone,
                                binary = ValueNone,
                                bit = ValueNone,
                                char = ValueNone,
                                date = ValueNone,
                                datetime = ValueNone,
                                datetime2 = ValueNone,
                                datetimeoffset = ValueNone,
                                decimal = ValueNone,
                                float = ValueNone,
                                image = ValueNone,
                                int = ValueNone,
                                money = ValueNone,
                                nchar = ValueNone,
                                ntext = ValueNone,
                                numeric = ValueNone,
                                nvarchar = ValueNone,
                                real = ValueNone,
                                smalldatetime = ValueNone,
                                smallint = ValueNone,
                                smallmoney = ValueNone,
                                text = ValueNone,
                                time = ValueNone,
                                tinyint = ValueNone,
                                uniqueidentifier = ValueNone,
                                varbinary = ValueNone,
                                varchar = ValueNone,
                                xml = ValueNone
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNullVoption
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    allTypesNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = None @>
                            test <@ row.Binary = None @>
                            test <@ row.Bit = None @>
                            test <@ row.Char = None @>
                            test <@ row.Date = None @>
                            test <@ row.Datetime = None @>
                            test <@ row.Datetime2 = None @>
                            test <@ row.Datetimeoffset = None @>
                            test <@ row.Decimal = None @>
                            test <@ row.Float = None @>
                            test <@ row.Image = None @>
                            test <@ row.Int = None @>
                            test <@ row.Money = None @>
                            test <@ row.Nchar = None @>
                            test <@ row.Ntext = None @>
                            test <@ row.Numeric = None @>
                            test <@ row.Nvarchar = None @>
                            test <@ row.Real = None @>
                            test <@ row.Smalldatetime = None @>
                            test <@ row.Smallint = None @>
                            test <@ row.Smallmoney = None @>
                            test <@ row.Text = None @>
                            test <@ row.Time = None @>
                            test <@ row.Tinyint = None @>
                            test <@ row.Uniqueidentifier = None @>
                            test <@ row.Varbinary = None @>
                            test <@ row.Varchar = None @>
                            test <@ row.Xml = None @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableAllTypesNullVoption + "_null_paramsFromDto") [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNullVoption_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNullVoption.AllTypesNull.create (
                                {|
                                    Bigint = ValueNone
                                    Binary = ValueNone
                                    Bit = ValueNone
                                    Char = ValueNone
                                    Date = ValueNone
                                    Datetime = ValueNone
                                    Datetime2 = ValueNone
                                    Datetimeoffset = ValueNone
                                    Decimal = ValueNone
                                    Float = ValueNone
                                    Image = ValueNone
                                    Int = ValueNone
                                    Money = ValueNone
                                    Nchar = ValueNone
                                    Ntext = ValueNone
                                    Numeric = ValueNone
                                    Nvarchar = ValueNone
                                    Real = ValueNone
                                    Smalldatetime = ValueNone
                                    Smallint = ValueNone
                                    Smallmoney = ValueNone
                                    Text = ValueNone
                                    Time = ValueNone
                                    Tinyint = ValueNone
                                    Uniqueidentifier = ValueNone
                                    Varbinary = ValueNone
                                    Varchar = ValueNone
                                    Xml = ValueNone
                                |}
                            )

                        let res =
                            DbGen.Scripts.TempTableAllTypesNullVoption
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        AllTypesNull = [
                                            createTempTableRow ()
                                            createTempTableRow ()
                                            createTempTableRow ()
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = None @>
                            test <@ row.Binary = None @>
                            test <@ row.Bit = None @>
                            test <@ row.Char = None @>
                            test <@ row.Date = None @>
                            test <@ row.Datetime = None @>
                            test <@ row.Datetime2 = None @>
                            test <@ row.Datetimeoffset = None @>
                            test <@ row.Decimal = None @>
                            test <@ row.Float = None @>
                            test <@ row.Image = None @>
                            test <@ row.Int = None @>
                            test <@ row.Money = None @>
                            test <@ row.Nchar = None @>
                            test <@ row.Ntext = None @>
                            test <@ row.Numeric = None @>
                            test <@ row.Nvarchar = None @>
                            test <@ row.Real = None @>
                            test <@ row.Smalldatetime = None @>
                            test <@ row.Smallint = None @>
                            test <@ row.Smallmoney = None @>
                            test <@ row.Text = None @>
                            test <@ row.Time = None @>
                            test <@ row.Tinyint = None @>
                            test <@ row.Uniqueidentifier = None @>
                            test <@ row.Varbinary = None @>
                            test <@ row.Varchar = None @>
                            test <@ row.Xml = None @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlined) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlined_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlined
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tempTableInlined = [
                                        DbGen.Scripts.TempTableInlined.tempTableInlined.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlined + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlined_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlined
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        TempTableInlined = [
                                            DbGen.Scripts.TempTableInlined.tempTableInlined.create (
                                                {| Col1 = 1; Col2 = Some "test" |}
                                            )
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlinedDynamic) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlinedDynamic_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlinedDynamic
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tempTableInlined = [
                                        DbGen.Scripts.TempTableInlinedDynamic.tempTableInlined.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlinedDynamic + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlinedDynamic_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlinedDynamic
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        TempTableInlined = [
                                            DbGen.Scripts.TempTableInlinedDynamic.tempTableInlined.create (
                                                {| Col1 = 1; Col2 = Some "test" |}
                                            )
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlinedWithOtherParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlinedWithOtherParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlinedWithOtherParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tempTableInlined = [
                                        DbGen.Scripts.TempTableInlinedWithOtherParams.tempTableInlined.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ],
                                    someParam = 2
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.TempTableInlinedWithOtherParams + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlinedWithOtherParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.TempTableInlinedWithOtherParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        TempTableInlined = [
                                            DbGen.Scripts.TempTableInlinedWithOtherParams.tempTableInlined.create (
                                                {| Col1 = 1; Col2 = Some "test" |}
                                            )
                                        ]
                                        SomeParam = 2
                                    |}
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithTempTable) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithTempTable_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Procedures.dbo.ProcWithTempTable
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tempTable = [
                                        DbGen.Procedures.dbo.ProcWithTempTable.tempTable.create (
                                            {| Col1 = 1; Col2 = Some "test" |}
                                        )
                                    ],
                                    param = 2
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.MultipleTempTables) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.MultipleTempTables_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.MultipleTempTables
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tempTable1 = [
                                        DbGen.Scripts.MultipleTempTables.tempTable1.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ],
                                    tempTable2 = [
                                        DbGen.Scripts.MultipleTempTables.tempTable2.create (col1 = 1, col3 = "foobar")
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                        test <@ res.Value.Col3 = "foobar" @>
                )
        ]


        testList (nameof DbGen.Scripts.MultipleTempTables + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.MultipleTempTables_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let res =
                            DbGen.Scripts.MultipleTempTables
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        TempTable1 = [
                                            DbGen.Scripts.MultipleTempTables.tempTable1.create (
                                                {| Col1 = 1; Col2 = Some "test" |}
                                            )
                                        ]
                                        TempTable2 = [
                                            DbGen.Scripts.MultipleTempTables.tempTable2.create (
                                                {| Col1 = 1; Col3 = "foobar" |}
                                            )
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.Col1 = 1 @>
                        test <@ res.Value.Col2 = Some "test" @>
                        test <@ res.Value.Col3 = "foobar" @>
                )
        ]


        testList "Can configure SqlBulkCopy" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlined_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let mutable rowsCopied = 0

                        let createRow () =
                            DbGen.Scripts.TempTableInlined.tempTableInlined.create (col1 = 1, col2 = Some "test")

                        DbGen.Scripts.TempTableInlined
                            .WithConnection(Config.connStr)
                            .ConfigureBulkCopy(fun bc ->
                                bc.NotifyAfter <- 1
                                bc.ColumnMappings.Add("Col1", "Col1") |> ignore
                                bc.ColumnMappings.Add("Col2", "Col2") |> ignore
                                bc.SqlRowsCopied.Add(fun _ -> rowsCopied <- rowsCopied + 1)
                            )
                            .WithParameters(tempTableInlined = [ createRow (); createRow (); createRow () ])
                        |> exec
                        |> function
                            | None -> failtest "Expected a result row"
                            | Some res ->
                                test <@ res.Col1 = 1 @>
                                test <@ res.Col2 = Some "test" @>

                        let rowsCopied = rowsCopied
                        test <@ rowsCopied = 3 @>
                    }
                )
        ]

        testList "Can use transactions" [
            yield!
                allSeqExecuteMethods<DbGen.Scripts.TempTableAllTypesNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let createTempTableRow () =
                            DbGen.Scripts.TempTableAllTypesNonNull.AllTypesNonNull.create (
                                bigint = 1L,
                                binary = Array.replicate 42 1uy,
                                bit = true,
                                char = String.replicate 42 "a",
                                date = DateTime(2000, 1, 1),
                                datetime = DateTime(2000, 1, 1),
                                datetime2 = DateTime(2000, 1, 1),
                                datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                decimal = 1M,
                                float = 1.,
                                image = [| 1uy |],
                                int = 1,
                                money = 1M,
                                nchar = String.replicate 42 "a",
                                ntext = "test",
                                numeric = 1M,
                                nvarchar = "test",
                                real = 1.f,
                                smalldatetime = DateTime(2000, 1, 1),
                                smallint = 1s,
                                smallmoney = 1M,
                                text = "test",
                                time = TimeSpan.FromSeconds 1.,
                                tinyint = 1uy,
                                uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                varbinary = [| 1uy |],
                                varchar = "test",
                                xml = "<tag />"
                            )

                        use conn = new SqlConnection(Config.connStr)
                        conn.Open()
                        use tran = conn.BeginTransaction()

                        let res =
                            DbGen.Scripts.TempTableAllTypesNonNull
                                .WithConnection(conn, tran)
                                .WithParameters(
                                    allTypesNonNull = [
                                        createTempTableRow ()
                                        createTempTableRow ()
                                        createTempTableRow ()
                                    ]
                                )
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.Bigint = 1L @>
                            test <@ row.Binary = Array.replicate 42 1uy @>
                            test <@ row.Bit = true @>
                            test <@ row.Char = String.replicate 42 "a" @>
                            test <@ row.Date = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime = DateTime(2000, 1, 1) @>
                            test <@ row.Datetime2 = DateTime(2000, 1, 1) @>
                            test <@ row.Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ row.Decimal = 1M @>
                            test <@ row.Float = 1. @>
                            test <@ row.Image = [| 1uy |] @>
                            test <@ row.Int = 1 @>
                            test <@ row.Money = 1M @>
                            test <@ row.Nchar = String.replicate 42 "a" @>
                            test <@ row.Ntext = "test" @>
                            test <@ row.Numeric = 1M @>
                            test <@ row.Nvarchar = "test" @>
                            test <@ row.Real = 1.f @>
                            test <@ row.Smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ row.Smallint = 1s @>
                            test <@ row.Smallmoney = 1M @>
                            test <@ row.Text = "test" @>
                            test <@ row.Time = TimeSpan.FromSeconds 1. @>
                            test <@ row.Tinyint = 1uy @>
                            test <@ row.Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ row.Varbinary = [| 1uy |] @>
                            test <@ row.Varchar = "test" @>
                            test <@ row.Xml = "<tag />" @>

                        tran.Commit()
                )
        ]


        testCase "Temp-table rows are disposed after sync load failure"
        <| fun () ->
            let row =
                DbGen.Scripts.TempTableInlined.tempTableInlined.create (col1 = 1, col2 = Some "test")

            let rows = FailingTempTableRows row

            try
                DbGen.Scripts.TempTableInlined
                    .WithConnection(Config.connStr)
                    .WithParameters(tempTableInlined = (rows :> seq<_>))
                    .ExecuteSingle()
                |> ignore

                failtest "Expected temp-table loading to fail"
            with _ ->
                test <@ rows.WasDisposed @>


        testCaseAsync "Temp-table rows are disposed after async load failure"
        <| async {
            let row =
                DbGen.Scripts.TempTableInlined.tempTableInlined.create (col1 = 1, col2 = Some "test")

            let rows = FailingTempTableRows row

            try
                do!
                    DbGen.Scripts.TempTableInlined
                        .WithConnection(Config.connStr)
                        .WithParameters(tempTableInlined = (rows :> seq<_>))
                        .ExecuteSingleAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                failtest "Expected temp-table loading to fail"
            with _ ->
                test <@ rows.WasDisposed @>
        }

        testCase "Sync temp-table load failures do not poison caller-owned connections"
        <| fun () ->
            let createBadRow () =
                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create (
                    binary = [| 1uy; 2uy; 3uy; 4uy |],
                    char = "1234",
                    nchar = "1234",
                    nvarchar = "1234",
                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                    varchar = "1234"
                )

            let createGoodRow () =
                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create (
                    binary = [| 1uy; 2uy; 3uy |],
                    char = "123",
                    nchar = "123",
                    nvarchar = "123",
                    varbinary = [| 1uy; 2uy; 3uy |],
                    varchar = "123"
                )

            use conn = new SqlConnection(Config.connStr)
            conn.Open()

            let runBad () =
                DbGen.Scripts.TempTableWithLengthTypes
                    .WithConnection(conn)
                    .WithParameters([ createBadRow () ])
                    .ExecuteSingle()
                |> ignore

            Expect.throws runBad "Should throw when bulk copy hits length limits"

            let res =
                DbGen.Scripts.TempTableWithLengthTypes
                    .WithConnection(conn)
                    .WithParameters([ createGoodRow () ])
                    .ExecuteSingle()

            test <@ res.Value.varchar = "123" @>

        testCaseAsync "Async temp-table load failures do not poison caller-owned connections"
        <| async {
            let createBadRow () =
                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create (
                    binary = [| 1uy; 2uy; 3uy; 4uy |],
                    char = "1234",
                    nchar = "1234",
                    nvarchar = "1234",
                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                    varchar = "1234"
                )

            let createGoodRow () =
                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create (
                    binary = [| 1uy; 2uy; 3uy |],
                    char = "123",
                    nchar = "123",
                    nvarchar = "123",
                    varbinary = [| 1uy; 2uy; 3uy |],
                    varchar = "123"
                )

            use conn = new SqlConnection(Config.connStr)
            conn.Open()

            let runBad () =
                DbGen.Scripts.TempTableWithLengthTypes
                    .WithConnection(conn)
                    .WithParameters([ createBadRow () ])
                    .ExecuteSingleAsync()
                |> Async.AwaitTask

            try
                do! runBad () |> Async.Ignore
                failtest "Expected temp-table loading to fail"
            with _ ->
                ()

            let! res =
                DbGen.Scripts.TempTableWithLengthTypes
                    .WithConnection(conn)
                    .WithParameters([ createGoodRow () ])
                    .ExecuteSingleAsync()
                |> Async.AwaitTask

            test <@ res.Value.varchar = "123" @>
        }

        testList "Successful temp-table executions do not poison caller-owned connections" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.TempTableInlined_Executable, _>
                |> List.filter (fun (name, _) -> name <> "LazyExecuteAsync" && name <> "LazyExecuteAsyncWithSyncRead")
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        use conn = new SqlConnection(Config.connStr)
                        conn.Open()

                        let createExecutable (conn: SqlConnection) =
                            DbGen.Scripts.TempTableInlined
                                .WithConnection(conn)
                                .WithParameters(
                                    tempTableInlined = [
                                        DbGen.Scripts.TempTableInlined.tempTableInlined.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ]
                                )

                        let first = createExecutable conn |> exec
                        let second = createExecutable conn |> exec

                        test <@ first.Value.Col1 = 1 @>
                        test <@ first.Value.Col2 = Some "test" @>
                        test <@ second.Value.Col1 = 1 @>
                        test <@ second.Value.Col2 = Some "test" @>
                )
        ]

        testList "Successful async lazy temp-table executions do not poison caller-owned connections" [
            testCase "LazyExecuteAsync"
            <| fun () ->
                use conn = new SqlConnection(Config.connStr)
                conn.Open()

                let getItems () =
                    DbGen.Scripts.TempTableInlined
                        .WithConnection(conn)
                        .WithParameters(
                            tempTableInlined = [
                                DbGen.Scripts.TempTableInlined.tempTableInlined.create (col1 = 1, col2 = Some "test")
                            ]
                        )
                        .LazyExecuteAsync()

                let readSingleRow () =
                    let items = getItems ()
                    let enumerator = items.GetAsyncEnumerator()

                    try
                        let hasItem =
                            enumerator.MoveNextAsync().AsTask() |> Async.AwaitTask |> Async.RunSynchronously

                        test <@ hasItem @>
                        test <@ enumerator.Current.Col1 = 1 @>
                        test <@ enumerator.Current.Col2 = Some "test" @>
                    finally
                        enumerator.DisposeAsync().AsTask() |> Async.AwaitTask |> Async.RunSynchronously

                readSingleRow ()
                readSingleRow ()

            testCase "LazyExecuteAsyncWithSyncRead"
            <| fun () ->
                use conn = new SqlConnection(Config.connStr)
                conn.Open()

                let getItems () =
                    DbGen.Scripts.TempTableInlined
                        .WithConnection(conn)
                        .WithParameters(
                            tempTableInlined = [
                                DbGen.Scripts.TempTableInlined.tempTableInlined.create (col1 = 1, col2 = Some "test")
                            ]
                        )
                        .LazyExecuteAsyncWithSyncRead()

                let readSingleRow () =
                    let items = getItems ()
                    let enumerator = items.GetAsyncEnumerator()

                    try
                        let hasItem =
                            enumerator.MoveNextAsync().AsTask() |> Async.AwaitTask |> Async.RunSynchronously

                        test <@ hasItem @>
                        test <@ enumerator.Current.Col1 = 1 @>
                        test <@ enumerator.Current.Col2 = Some "test" @>
                    finally
                        enumerator.DisposeAsync().AsTask() |> Async.AwaitTask |> Async.RunSynchronously

                readSingleRow ()
                readSingleRow ()
        ]

        testList "Successful temp-table reader executions do not poison caller-owned connections" [
            yield!
                allSingleReaderExecuteMethods<DbGen.Scripts.TempTableInlined_Executable>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        use conn = new SqlConnection(Config.connStr)
                        conn.Open()

                        let createExecutable (conn: SqlConnection) =
                            DbGen.Scripts.TempTableInlined
                                .WithConnection(conn)
                                .WithParameters(
                                    tempTableInlined = [
                                        DbGen.Scripts.TempTableInlined.tempTableInlined.create (
                                            col1 = 1,
                                            col2 = Some "test"
                                        )
                                    ]
                                )

                        let readSingleRow () =
                            use readerDisposer = createExecutable conn |> exec
                            let reader = readerDisposer.Reader
                            test <@ reader.Read() @>
                            test <@ reader.GetInt32(0) = 1 @>
                            test <@ reader.GetString(1) = "test" @>
                            test <@ not (reader.Read()) @>

                        readSingleRow ()
                        readSingleRow ()
                )
        ]


    ]
