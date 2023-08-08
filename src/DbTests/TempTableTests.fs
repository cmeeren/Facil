module TempTableTests

open System
open Microsoft.Data.SqlClient
open Expecto
open Swensen.Unquote


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
                                Bigint = 1L,
                                Binary = Array.replicate 42 1uy,
                                Bit = true,
                                Char = String.replicate 42 "a",
                                Date = DateTime(2000, 1, 1),
                                Datetime = DateTime(2000, 1, 1),
                                Datetime2 = DateTime(2000, 1, 1),
                                Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                Decimal = 1M,
                                Float = 1.,
                                Image = [| 1uy |],
                                Int = 1,
                                Money = 1M,
                                Nchar = String.replicate 42 "a",
                                Ntext = "test",
                                Numeric = 1M,
                                Nvarchar = "test",
                                Real = 1.f,
                                Smalldatetime = DateTime(2000, 1, 1),
                                Smallint = 1s,
                                Smallmoney = 1M,
                                Text = "test",
                                Time = TimeSpan.FromSeconds 1.,
                                Tinyint = 1uy,
                                Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                Varbinary = [| 1uy |],
                                Varchar = "test",
                                Xml = "<tag />"
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
                                Bigint = Some 1L,
                                Binary = Some(Array.replicate 42 1uy),
                                Bit = Some true,
                                Char = Some(String.replicate 42 "a"),
                                Date = Some(DateTime(2000, 1, 1)),
                                Datetime = Some(DateTime(2000, 1, 1)),
                                Datetime2 = Some(DateTime(2000, 1, 1)),
                                Datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)),
                                Decimal = Some 1M,
                                Float = Some 1.,
                                Image = Some [| 1uy |],
                                Int = Some 1,
                                Money = Some 1M,
                                Nchar = Some(String.replicate 42 "a"),
                                Ntext = Some "test",
                                Numeric = Some 1M,
                                Nvarchar = Some "test",
                                Real = Some 1.f,
                                Smalldatetime = Some(DateTime(2000, 1, 1)),
                                Smallint = Some 1s,
                                Smallmoney = Some 1M,
                                Text = Some "test",
                                Time = Some(TimeSpan.FromSeconds 1.),
                                Tinyint = Some 1uy,
                                Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                Varbinary = Some [| 1uy |],
                                Varchar = Some "test",
                                Xml = Some "<tag />"
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
                                Bigint = None,
                                Binary = None,
                                Bit = None,
                                Char = None,
                                Date = None,
                                Datetime = None,
                                Datetime2 = None,
                                Datetimeoffset = None,
                                Decimal = None,
                                Float = None,
                                Image = None,
                                Int = None,
                                Money = None,
                                Nchar = None,
                                Ntext = None,
                                Numeric = None,
                                Nvarchar = None,
                                Real = None,
                                Smalldatetime = None,
                                Smallint = None,
                                Smallmoney = None,
                                Text = None,
                                Time = None,
                                Tinyint = None,
                                Uniqueidentifier = None,
                                Varbinary = None,
                                Varchar = None,
                                Xml = None
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
                                Bigint = ValueSome 1L,
                                Binary = ValueSome(Array.replicate 42 1uy),
                                Bit = ValueSome true,
                                Char = ValueSome(String.replicate 42 "a"),
                                Date = ValueSome(DateTime(2000, 1, 1)),
                                Datetime = ValueSome(DateTime(2000, 1, 1)),
                                Datetime2 = ValueSome(DateTime(2000, 1, 1)),
                                Datetimeoffset = ValueSome(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)),
                                Decimal = ValueSome 1M,
                                Float = ValueSome 1.,
                                Image = ValueSome [| 1uy |],
                                Int = ValueSome 1,
                                Money = ValueSome 1M,
                                Nchar = ValueSome(String.replicate 42 "a"),
                                Ntext = ValueSome "test",
                                Numeric = ValueSome 1M,
                                Nvarchar = ValueSome "test",
                                Real = ValueSome 1.f,
                                Smalldatetime = ValueSome(DateTime(2000, 1, 1)),
                                Smallint = ValueSome 1s,
                                Smallmoney = ValueSome 1M,
                                Text = ValueSome "test",
                                Time = ValueSome(TimeSpan.FromSeconds 1.),
                                Tinyint = ValueSome 1uy,
                                Uniqueidentifier = ValueSome(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                Varbinary = ValueSome [| 1uy |],
                                Varchar = ValueSome "test",
                                Xml = ValueSome "<tag />"
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
                                Bigint = ValueNone,
                                Binary = ValueNone,
                                Bit = ValueNone,
                                Char = ValueNone,
                                Date = ValueNone,
                                Datetime = ValueNone,
                                Datetime2 = ValueNone,
                                Datetimeoffset = ValueNone,
                                Decimal = ValueNone,
                                Float = ValueNone,
                                Image = ValueNone,
                                Int = ValueNone,
                                Money = ValueNone,
                                Nchar = ValueNone,
                                Ntext = ValueNone,
                                Numeric = ValueNone,
                                Nvarchar = ValueNone,
                                Real = ValueNone,
                                Smalldatetime = ValueNone,
                                Smallint = ValueNone,
                                Smallmoney = ValueNone,
                                Text = ValueNone,
                                Time = ValueNone,
                                Tinyint = ValueNone,
                                Uniqueidentifier = ValueNone,
                                Varbinary = ValueNone,
                                Varchar = ValueNone,
                                Xml = ValueNone
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
                                            Col1 = 1,
                                            Col2 = Some "test"
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
                                            Col1 = 1,
                                            Col2 = Some "test"
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
                                            Col1 = 1,
                                            Col2 = Some "test"
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
                                            Col1 = 1,
                                            Col2 = Some "test"
                                        )
                                    ],
                                    tempTable2 = [
                                        DbGen.Scripts.MultipleTempTables.tempTable2.create (Col1 = 1, Col3 = "foobar")
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
                            DbGen.Scripts.TempTableInlined.tempTableInlined.create (Col1 = 1, Col2 = Some "test")

                        DbGen.Scripts.TempTableInlined
                            .WithConnection(Config.connStr)
                            .ConfigureBulkCopy(fun bc ->
                                bc.NotifyAfter <- 1
                                bc.SqlRowsCopied.Add(fun _ -> rowsCopied <- rowsCopied + 1)
                            )
                            .WithParameters(tempTableInlined = [ createRow (); createRow (); createRow () ])
                        |> exec
                        |> ignore

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
                                Bigint = 1L,
                                Binary = Array.replicate 42 1uy,
                                Bit = true,
                                Char = String.replicate 42 "a",
                                Date = DateTime(2000, 1, 1),
                                Datetime = DateTime(2000, 1, 1),
                                Datetime2 = DateTime(2000, 1, 1),
                                Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
                                Decimal = 1M,
                                Float = 1.,
                                Image = [| 1uy |],
                                Int = 1,
                                Money = 1M,
                                Nchar = String.replicate 42 "a",
                                Ntext = "test",
                                Numeric = 1M,
                                Nvarchar = "test",
                                Real = 1.f,
                                Smalldatetime = DateTime(2000, 1, 1),
                                Smallint = 1s,
                                Smallmoney = 1M,
                                Text = "test",
                                Time = TimeSpan.FromSeconds 1.,
                                Tinyint = 1uy,
                                Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                Varbinary = [| 1uy |],
                                Varchar = "test",
                                Xml = "<tag />"
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


    ]
