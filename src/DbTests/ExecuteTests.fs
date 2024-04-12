module ExecuteTests

open System
open System.Threading
open System.Threading.Tasks
open Expecto
open Facil.Runtime
open Swensen.Unquote


// The primary goal of tests in this file is to exercise basic input/output functionality
// (which incidentally also covers some other stuff, too)


[<Tests>]
let execTests =
    testList "Execute tests" [


        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTable) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(Config.connStr) |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTableExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcSelectFromTableExtended, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcSelectFromTableExtended.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypes) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypes_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypes
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = Array.replicate 8 1uy,
                                    smalldatetime = DateTime(2000, 1, 1),
                                    smallint = 1s,
                                    smallmoney = 1M,
                                    text = "test",
                                    time = TimeSpan.FromSeconds 1.,
                                    timestamp = Array.replicate 8 1uy,
                                    tinyint = 1uy,
                                    uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                    varbinary = [| 1uy |],
                                    varchar = "test",
                                    xml = "<tag />"
                                )
                            |> exec

                        test <@ res.Value.bigint.Value = 1L @>
                        test <@ res.Value.binary.Value = Array.replicate 42 1uy @>
                        test <@ res.Value.bit.Value = true @>
                        test <@ res.Value.char.Value = String.replicate 42 "a" @>
                        test <@ res.Value.date.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset.Value = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal.Value = 1M @>
                        test <@ res.Value.float.Value = 1. @>
                        test <@ res.Value.image.Value = [| 1uy |] @>
                        test <@ res.Value.int.Value = 1 @>
                        test <@ res.Value.money.Value = 1M @>
                        test <@ res.Value.nchar.Value = String.replicate 42 "a" @>
                        test <@ res.Value.ntext.Value = "test" @>
                        test <@ res.Value.numeric.Value = 1M @>
                        test <@ res.Value.nvarchar.Value = "test" @>
                        test <@ res.Value.real.Value = 1.f @>
                        test <@ res.Value.rowversion.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.smalldatetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint.Value = 1s @>
                        test <@ res.Value.smallmoney.Value = 1M @>
                        test <@ res.Value.text.Value = "test" @>
                        test <@ res.Value.time.Value = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.timestamp.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.tinyint.Value = 1uy @>
                        test <@ res.Value.uniqueidentifier.Value = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary.Value = [| 1uy |] @>
                        test <@ res.Value.varchar.Value = "test" @>
                        test <@ res.Value.xml.Value = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = Array.replicate 8 1uy,
                                    smalldatetime = DateTime(2000, 1, 1),
                                    smallint = 1s,
                                    smallmoney = 1M,
                                    text = "test",
                                    time = TimeSpan.FromSeconds 1.,
                                    timestamp = Array.replicate 8 1uy,
                                    tinyint = 1uy,
                                    uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"),
                                    varbinary = [| 1uy |],
                                    varchar = "test",
                                    xml = "<tag />"
                                )
                            |> exec

                        test <@ res.Value.bigint.Value = 1L @>
                        test <@ res.Value.binary.Value = Array.replicate 42 1uy @>
                        test <@ res.Value.bit.Value = true @>
                        test <@ res.Value.char.Value = String.replicate 42 "a" @>
                        test <@ res.Value.date.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset.Value = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal.Value = 1M @>
                        test <@ res.Value.float.Value = 1. @>
                        test <@ res.Value.image.Value = [| 1uy |] @>
                        test <@ res.Value.int.Value = 1 @>
                        test <@ res.Value.money.Value = 1M @>
                        test <@ res.Value.nchar.Value = String.replicate 42 "a" @>
                        test <@ res.Value.ntext.Value = "test" @>
                        test <@ res.Value.numeric.Value = 1M @>
                        test <@ res.Value.nvarchar.Value = "test" @>
                        test <@ res.Value.real.Value = 1.f @>
                        test <@ res.Value.rowversion.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.smalldatetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint.Value = 1s @>
                        test <@ res.Value.smallmoney.Value = 1M @>
                        test <@ res.Value.text.Value = "test" @>
                        test <@ res.Value.time.Value = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.timestamp.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.tinyint.Value = 1uy @>
                        test <@ res.Value.uniqueidentifier.Value = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary.Value = [| 1uy |] @>
                        test <@ res.Value.varchar.Value = "test" @>
                        test <@ res.Value.xml.Value = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypes + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypes_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypes
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                        Rowversion = Array.replicate 8 1uy
                                        Smalldatetime = DateTime(2000, 1, 1)
                                        Smallint = 1s
                                        Smallmoney = 1M
                                        Text = "test"
                                        Time = TimeSpan.FromSeconds 1.
                                        Timestamp = Array.replicate 8 1uy
                                        Tinyint = 1uy
                                        Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")
                                        Varbinary = [| 1uy |]
                                        Varchar = "test"
                                        Xml = "<tag />"
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint.Value = 1L @>
                        test <@ res.Value.binary.Value = Array.replicate 42 1uy @>
                        test <@ res.Value.bit.Value = true @>
                        test <@ res.Value.char.Value = String.replicate 42 "a" @>
                        test <@ res.Value.date.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset.Value = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal.Value = 1M @>
                        test <@ res.Value.float.Value = 1. @>
                        test <@ res.Value.image.Value = [| 1uy |] @>
                        test <@ res.Value.int.Value = 1 @>
                        test <@ res.Value.money.Value = 1M @>
                        test <@ res.Value.nchar.Value = String.replicate 42 "a" @>
                        test <@ res.Value.ntext.Value = "test" @>
                        test <@ res.Value.numeric.Value = 1M @>
                        test <@ res.Value.nvarchar.Value = "test" @>
                        test <@ res.Value.real.Value = 1.f @>
                        test <@ res.Value.rowversion.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.smalldatetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint.Value = 1s @>
                        test <@ res.Value.smallmoney.Value = 1M @>
                        test <@ res.Value.text.Value = "test" @>
                        test <@ res.Value.time.Value = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.timestamp.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.tinyint.Value = 1uy @>
                        test <@ res.Value.uniqueidentifier.Value = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary.Value = [| 1uy |] @>
                        test <@ res.Value.varchar.Value = "test" @>
                        test <@ res.Value.xml.Value = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesExtended + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                        Rowversion = Array.replicate 8 1uy
                                        Smalldatetime = DateTime(2000, 1, 1)
                                        Smallint = 1s
                                        Smallmoney = 1M
                                        Text = "test"
                                        Time = TimeSpan.FromSeconds 1.
                                        Timestamp = Array.replicate 8 1uy
                                        Tinyint = 1uy
                                        Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")
                                        Varbinary = [| 1uy |]
                                        Varchar = "test"
                                        Xml = "<tag />"
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint.Value = 1L @>
                        test <@ res.Value.binary.Value = Array.replicate 42 1uy @>
                        test <@ res.Value.bit.Value = true @>
                        test <@ res.Value.char.Value = String.replicate 42 "a" @>
                        test <@ res.Value.date.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset.Value = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal.Value = 1M @>
                        test <@ res.Value.float.Value = 1. @>
                        test <@ res.Value.image.Value = [| 1uy |] @>
                        test <@ res.Value.int.Value = 1 @>
                        test <@ res.Value.money.Value = 1M @>
                        test <@ res.Value.nchar.Value = String.replicate 42 "a" @>
                        test <@ res.Value.ntext.Value = "test" @>
                        test <@ res.Value.numeric.Value = 1M @>
                        test <@ res.Value.nvarchar.Value = "test" @>
                        test <@ res.Value.real.Value = 1.f @>
                        test <@ res.Value.rowversion.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.smalldatetime.Value = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint.Value = 1s @>
                        test <@ res.Value.smallmoney.Value = 1M @>
                        test <@ res.Value.text.Value = "test" @>
                        test <@ res.Value.time.Value = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.timestamp.Value = Array.replicate 8 1uy @>
                        test <@ res.Value.tinyint.Value = 1uy @>
                        test <@ res.Value.uniqueidentifier.Value = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary.Value = [| 1uy |] @>
                        test <@ res.Value.varchar.Value = "test" @>
                        test <@ res.Value.xml.Value = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNull) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = Some(Array.replicate 8 1uy),
                                    smalldatetime = Some(DateTime(2000, 1, 1)),
                                    smallint = Some 1s,
                                    smallmoney = Some 1M,
                                    text = Some "test",
                                    time = Some(TimeSpan.FromSeconds 1.),
                                    timestamp = Some(Array.replicate 8 1uy),
                                    tinyint = Some 1uy,
                                    uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                    varbinary = Some [| 1uy |],
                                    varchar = Some "test",
                                    xml = Some "<tag />"
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.rowversion = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.timestamp = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNullExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = Some(Array.replicate 8 1uy),
                                    smalldatetime = Some(DateTime(2000, 1, 1)),
                                    smallint = Some 1s,
                                    smallmoney = Some 1M,
                                    text = Some "test",
                                    time = Some(TimeSpan.FromSeconds 1.),
                                    timestamp = Some(Array.replicate 8 1uy),
                                    tinyint = Some 1uy,
                                    uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")),
                                    varbinary = Some [| 1uy |],
                                    varchar = Some "test",
                                    xml = Some "<tag />"
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.rowversion = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.timestamp = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNull + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                        Rowversion = Some(Array.replicate 8 1uy)
                                        Smalldatetime = Some(DateTime(2000, 1, 1))
                                        Smallint = Some 1s
                                        Smallmoney = Some 1M
                                        Text = Some "test"
                                        Time = Some(TimeSpan.FromSeconds 1.)
                                        Timestamp = Some(Array.replicate 8 1uy)
                                        Tinyint = Some 1uy
                                        Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                        Varbinary = Some [| 1uy |]
                                        Varchar = Some "test"
                                        Xml = Some "<tag />"
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.rowversion = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.timestamp = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNullExtended + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                        Rowversion = Some(Array.replicate 8 1uy)
                                        Smalldatetime = Some(DateTime(2000, 1, 1))
                                        Smallint = Some 1s
                                        Smallmoney = Some 1M
                                        Text = Some "test"
                                        Time = Some(TimeSpan.FromSeconds 1.)
                                        Timestamp = Some(Array.replicate 8 1uy)
                                        Tinyint = Some 1uy
                                        Uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                        Varbinary = Some [| 1uy |]
                                        Varchar = Some "test"
                                        Xml = Some "<tag />"
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.rowversion = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.timestamp = Some(Array.replicate 8 1uy) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNull + "_allNull") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = None,
                                    smalldatetime = None,
                                    smallint = None,
                                    smallmoney = None,
                                    text = None,
                                    time = None,
                                    timestamp = None,
                                    tinyint = None,
                                    uniqueidentifier = None,
                                    varbinary = None,
                                    varchar = None,
                                    xml = None
                                )
                            |> exec

                        test <@ res.Value.bigint = None @>
                        test <@ res.Value.binary = None @>
                        test <@ res.Value.bit = None @>
                        test <@ res.Value.char = None @>
                        test <@ res.Value.date = None @>
                        test <@ res.Value.datetime = None @>
                        test <@ res.Value.datetime2 = None @>
                        test <@ res.Value.datetimeoffset = None @>
                        test <@ res.Value.decimal = None @>
                        test <@ res.Value.float = None @>
                        test <@ res.Value.image = None @>
                        test <@ res.Value.int = None @>
                        test <@ res.Value.money = None @>
                        test <@ res.Value.nchar = None @>
                        test <@ res.Value.ntext = None @>
                        test <@ res.Value.numeric = None @>
                        test <@ res.Value.nvarchar = None @>
                        test <@ res.Value.real = None @>
                        test <@ res.Value.rowversion = None @>
                        test <@ res.Value.smalldatetime = None @>
                        test <@ res.Value.smallint = None @>
                        test <@ res.Value.smallmoney = None @>
                        test <@ res.Value.text = None @>
                        test <@ res.Value.time = None @>
                        test <@ res.Value.timestamp = None @>
                        test <@ res.Value.tinyint = None @>
                        test <@ res.Value.uniqueidentifier = None @>
                        test <@ res.Value.varbinary = None @>
                        test <@ res.Value.varchar = None @>
                        test <@ res.Value.xml = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNullExtended + "_allNull") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                    rowversion = None,
                                    smalldatetime = None,
                                    smallint = None,
                                    smallmoney = None,
                                    text = None,
                                    time = None,
                                    timestamp = None,
                                    tinyint = None,
                                    uniqueidentifier = None,
                                    varbinary = None,
                                    varchar = None,
                                    xml = None
                                )
                            |> exec

                        test <@ res.Value.bigint = None @>
                        test <@ res.Value.binary = None @>
                        test <@ res.Value.bit = None @>
                        test <@ res.Value.char = None @>
                        test <@ res.Value.date = None @>
                        test <@ res.Value.datetime = None @>
                        test <@ res.Value.datetime2 = None @>
                        test <@ res.Value.datetimeoffset = None @>
                        test <@ res.Value.decimal = None @>
                        test <@ res.Value.float = None @>
                        test <@ res.Value.image = None @>
                        test <@ res.Value.int = None @>
                        test <@ res.Value.money = None @>
                        test <@ res.Value.nchar = None @>
                        test <@ res.Value.ntext = None @>
                        test <@ res.Value.numeric = None @>
                        test <@ res.Value.nvarchar = None @>
                        test <@ res.Value.real = None @>
                        test <@ res.Value.rowversion = None @>
                        test <@ res.Value.smalldatetime = None @>
                        test <@ res.Value.smallint = None @>
                        test <@ res.Value.smallmoney = None @>
                        test <@ res.Value.text = None @>
                        test <@ res.Value.time = None @>
                        test <@ res.Value.timestamp = None @>
                        test <@ res.Value.tinyint = None @>
                        test <@ res.Value.uniqueidentifier = None @>
                        test <@ res.Value.varbinary = None @>
                        test <@ res.Value.varchar = None @>
                        test <@ res.Value.xml = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesNull + "_allNull_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
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
                                        Rowversion = None
                                        Smalldatetime = None
                                        Smallint = None
                                        Smallmoney = None
                                        Text = None
                                        Time = None
                                        Timestamp = None
                                        Tinyint = None
                                        Uniqueidentifier = None
                                        Varbinary = None
                                        Varchar = None
                                        Xml = None
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint = None @>
                        test <@ res.Value.binary = None @>
                        test <@ res.Value.bit = None @>
                        test <@ res.Value.char = None @>
                        test <@ res.Value.date = None @>
                        test <@ res.Value.datetime = None @>
                        test <@ res.Value.datetime2 = None @>
                        test <@ res.Value.datetimeoffset = None @>
                        test <@ res.Value.decimal = None @>
                        test <@ res.Value.float = None @>
                        test <@ res.Value.image = None @>
                        test <@ res.Value.int = None @>
                        test <@ res.Value.money = None @>
                        test <@ res.Value.nchar = None @>
                        test <@ res.Value.ntext = None @>
                        test <@ res.Value.numeric = None @>
                        test <@ res.Value.nvarchar = None @>
                        test <@ res.Value.real = None @>
                        test <@ res.Value.rowversion = None @>
                        test <@ res.Value.smalldatetime = None @>
                        test <@ res.Value.smallint = None @>
                        test <@ res.Value.smallmoney = None @>
                        test <@ res.Value.text = None @>
                        test <@ res.Value.time = None @>
                        test <@ res.Value.timestamp = None @>
                        test <@ res.Value.tinyint = None @>
                        test <@ res.Value.uniqueidentifier = None @>
                        test <@ res.Value.varbinary = None @>
                        test <@ res.Value.varchar = None @>
                        test <@ res.Value.xml = None @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesNullExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
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
                                            Rowversion = None
                                            Smalldatetime = None
                                            Smallint = None
                                            Smallmoney = None
                                            Text = None
                                            Time = None
                                            Timestamp = None
                                            Tinyint = None
                                            Uniqueidentifier = None
                                            Varbinary = None
                                            Varchar = None
                                            Xml = None
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.bigint = None @>
                            test <@ res.Value.binary = None @>
                            test <@ res.Value.bit = None @>
                            test <@ res.Value.char = None @>
                            test <@ res.Value.date = None @>
                            test <@ res.Value.datetime = None @>
                            test <@ res.Value.datetime2 = None @>
                            test <@ res.Value.datetimeoffset = None @>
                            test <@ res.Value.decimal = None @>
                            test <@ res.Value.float = None @>
                            test <@ res.Value.image = None @>
                            test <@ res.Value.int = None @>
                            test <@ res.Value.money = None @>
                            test <@ res.Value.nchar = None @>
                            test <@ res.Value.ntext = None @>
                            test <@ res.Value.numeric = None @>
                            test <@ res.Value.nvarchar = None @>
                            test <@ res.Value.real = None @>
                            test <@ res.Value.rowversion = None @>
                            test <@ res.Value.smalldatetime = None @>
                            test <@ res.Value.smallint = None @>
                            test <@ res.Value.smallmoney = None @>
                            test <@ res.Value.text = None @>
                            test <@ res.Value.time = None @>
                            test <@ res.Value.timestamp = None @>
                            test <@ res.Value.tinyint = None @>
                            test <@ res.Value.uniqueidentifier = None @>
                            test <@ res.Value.varbinary = None @>
                            test <@ res.Value.varchar = None @>
                            test <@ res.Value.xml = None @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNonNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = 1L @>
                        test <@ res.Value.binary = Array.replicate 42 1uy @>
                        test <@ res.Value.bit = true @>
                        test <@ res.Value.char = String.replicate 42 "a" @>
                        test <@ res.Value.date = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2 = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal = 1M @>
                        test <@ res.Value.float = 1. @>
                        test <@ res.Value.image = [| 1uy |] @>
                        test <@ res.Value.int = 1 @>
                        test <@ res.Value.money = 1M @>
                        test <@ res.Value.nchar = String.replicate 42 "a" @>
                        test <@ res.Value.ntext = "test" @>
                        test <@ res.Value.numeric = 1M @>
                        test <@ res.Value.nvarchar = "test" @>
                        test <@ res.Value.real = 1.f @>
                        test <@ res.Value.smalldatetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint = 1s @>
                        test <@ res.Value.smallmoney = 1M @>
                        test <@ res.Value.text = "test" @>
                        test <@ res.Value.time = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.tinyint = 1uy @>
                        test <@ res.Value.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary = [| 1uy |] @>
                        test <@ res.Value.varchar = "test" @>
                        test <@ res.Value.xml = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNonNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = 1L @>
                        test <@ res.Value.binary = Array.replicate 42 1uy @>
                        test <@ res.Value.bit = true @>
                        test <@ res.Value.char = String.replicate 42 "a" @>
                        test <@ res.Value.date = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2 = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal = 1M @>
                        test <@ res.Value.float = 1. @>
                        test <@ res.Value.image = [| 1uy |] @>
                        test <@ res.Value.int = 1 @>
                        test <@ res.Value.money = 1M @>
                        test <@ res.Value.nchar = String.replicate 42 "a" @>
                        test <@ res.Value.ntext = "test" @>
                        test <@ res.Value.numeric = 1M @>
                        test <@ res.Value.nvarchar = "test" @>
                        test <@ res.Value.real = 1.f @>
                        test <@ res.Value.smalldatetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint = 1s @>
                        test <@ res.Value.smallmoney = 1M @>
                        test <@ res.Value.text = "test" @>
                        test <@ res.Value.time = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.tinyint = 1uy @>
                        test <@ res.Value.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary = [| 1uy |] @>
                        test <@ res.Value.varchar = "test" @>
                        test <@ res.Value.xml = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        Params =
                                            List.map DbGen.TableTypes.dbo.AllTypesNonNull.create [
                                                {|
                                                    bigint = 1L
                                                    binary = Array.replicate 42 1uy
                                                    bit = true
                                                    char = String.replicate 42 "a"
                                                    date = DateTime(2000, 1, 1)
                                                    datetime = DateTime(2000, 1, 1)
                                                    datetime2 = DateTime(2000, 1, 1)
                                                    datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
                                                    decimal = 1M
                                                    float = 1.
                                                    image = [| 1uy |]
                                                    int = 1
                                                    money = 1M
                                                    nchar = String.replicate 42 "a"
                                                    ntext = "test"
                                                    numeric = 1M
                                                    nvarchar = "test"
                                                    real = 1.f
                                                    smalldatetime = DateTime(2000, 1, 1)
                                                    smallint = 1s
                                                    smallmoney = 1M
                                                    text = "test"
                                                    time = TimeSpan.FromSeconds 1.
                                                    tinyint = 1uy
                                                    uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")
                                                    varbinary = [| 1uy |]
                                                    varchar = "test"
                                                    xml = "<tag />"
                                                |}
                                            ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint = 1L @>
                        test <@ res.Value.binary = Array.replicate 42 1uy @>
                        test <@ res.Value.bit = true @>
                        test <@ res.Value.char = String.replicate 42 "a" @>
                        test <@ res.Value.date = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetime2 = DateTime(2000, 1, 1) @>
                        test <@ res.Value.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                        test <@ res.Value.decimal = 1M @>
                        test <@ res.Value.float = 1. @>
                        test <@ res.Value.image = [| 1uy |] @>
                        test <@ res.Value.int = 1 @>
                        test <@ res.Value.money = 1M @>
                        test <@ res.Value.nchar = String.replicate 42 "a" @>
                        test <@ res.Value.ntext = "test" @>
                        test <@ res.Value.numeric = 1M @>
                        test <@ res.Value.nvarchar = "test" @>
                        test <@ res.Value.real = 1.f @>
                        test <@ res.Value.smalldatetime = DateTime(2000, 1, 1) @>
                        test <@ res.Value.smallint = 1s @>
                        test <@ res.Value.smallmoney = 1M @>
                        test <@ res.Value.text = "test" @>
                        test <@ res.Value.time = TimeSpan.FromSeconds 1. @>
                        test <@ res.Value.tinyint = 1uy @>
                        test <@ res.Value.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                        test <@ res.Value.varbinary = [| 1uy |] @>
                        test <@ res.Value.varchar = "test" @>
                        test <@ res.Value.xml = "<tag />" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Params =
                                                List.map DbGen.TableTypes.dbo.AllTypesNonNull.create [
                                                    {|
                                                        bigint = 1L
                                                        binary = Array.replicate 42 1uy
                                                        bit = true
                                                        char = String.replicate 42 "a"
                                                        date = DateTime(2000, 1, 1)
                                                        datetime = DateTime(2000, 1, 1)
                                                        datetime2 = DateTime(2000, 1, 1)
                                                        datetimeoffset =
                                                            DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
                                                        decimal = 1M
                                                        float = 1.
                                                        image = [| 1uy |]
                                                        int = 1
                                                        money = 1M
                                                        nchar = String.replicate 42 "a"
                                                        ntext = "test"
                                                        numeric = 1M
                                                        nvarchar = "test"
                                                        real = 1.f
                                                        smalldatetime = DateTime(2000, 1, 1)
                                                        smallint = 1s
                                                        smallmoney = 1M
                                                        text = "test"
                                                        time = TimeSpan.FromSeconds 1.
                                                        tinyint = 1uy
                                                        uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")
                                                        varbinary = [| 1uy |]
                                                        varchar = "test"
                                                        xml = "<tag />"
                                                    |}
                                                ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.bigint = 1L @>
                            test <@ res.Value.binary = Array.replicate 42 1uy @>
                            test <@ res.Value.bit = true @>
                            test <@ res.Value.char = String.replicate 42 "a" @>
                            test <@ res.Value.date = DateTime(2000, 1, 1) @>
                            test <@ res.Value.datetime = DateTime(2000, 1, 1) @>
                            test <@ res.Value.datetime2 = DateTime(2000, 1, 1) @>
                            test <@ res.Value.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ res.Value.decimal = 1M @>
                            test <@ res.Value.float = 1. @>
                            test <@ res.Value.image = [| 1uy |] @>
                            test <@ res.Value.int = 1 @>
                            test <@ res.Value.money = 1M @>
                            test <@ res.Value.nchar = String.replicate 42 "a" @>
                            test <@ res.Value.ntext = "test" @>
                            test <@ res.Value.numeric = 1M @>
                            test <@ res.Value.nvarchar = "test" @>
                            test <@ res.Value.real = 1.f @>
                            test <@ res.Value.smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ res.Value.smallint = 1s @>
                            test <@ res.Value.smallmoney = 1M @>
                            test <@ res.Value.text = "test" @>
                            test <@ res.Value.time = TimeSpan.FromSeconds 1. @>
                            test <@ res.Value.tinyint = 1uy @>
                            test <@ res.Value.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ res.Value.varbinary = [| 1uy |] @>
                            test <@ res.Value.varchar = "test" @>
                            test <@ res.Value.xml = "<tag />" @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        Params =
                                            List.map DbGen.TableTypes.dbo.AllTypesNull.create [
                                                {|
                                                    bigint = Some 1L
                                                    binary = Some(Array.replicate 42 1uy)
                                                    bit = Some true
                                                    char = Some(String.replicate 42 "a")
                                                    date = Some(DateTime(2000, 1, 1))
                                                    datetime = Some(DateTime(2000, 1, 1))
                                                    datetime2 = Some(DateTime(2000, 1, 1))
                                                    datetimeoffset =
                                                        Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                                    decimal = Some 1M
                                                    float = Some 1.
                                                    image = Some [| 1uy |]
                                                    int = Some 1
                                                    money = Some 1M
                                                    nchar = Some(String.replicate 42 "a")
                                                    ntext = Some "test"
                                                    numeric = Some 1M
                                                    nvarchar = Some "test"
                                                    real = Some 1.f
                                                    smalldatetime = Some(DateTime(2000, 1, 1))
                                                    smallint = Some 1s
                                                    smallmoney = Some 1M
                                                    text = Some "test"
                                                    time = Some(TimeSpan.FromSeconds 1.)
                                                    tinyint = Some 1uy
                                                    uniqueidentifier =
                                                        Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                                    varbinary = Some [| 1uy |]
                                                    varchar = Some "test"
                                                    xml = Some "<tag />"
                                                |}
                                            ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.bigint = Some 1L @>
                        test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                        test <@ res.Value.bit = Some true @>
                        test <@ res.Value.char = Some(String.replicate 42 "a") @>
                        test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                        test <@ res.Value.decimal = Some 1M @>
                        test <@ res.Value.float = Some 1. @>
                        test <@ res.Value.image = Some [| 1uy |] @>
                        test <@ res.Value.int = Some 1 @>
                        test <@ res.Value.money = Some 1M @>
                        test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                        test <@ res.Value.ntext = Some "test" @>
                        test <@ res.Value.numeric = Some 1M @>
                        test <@ res.Value.nvarchar = Some "test" @>
                        test <@ res.Value.real = Some 1.f @>
                        test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                        test <@ res.Value.smallint = Some 1s @>
                        test <@ res.Value.smallmoney = Some 1M @>
                        test <@ res.Value.text = Some "test" @>
                        test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                        test <@ res.Value.tinyint = Some 1uy @>
                        test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                        test <@ res.Value.varbinary = Some [| 1uy |] @>
                        test <@ res.Value.varchar = Some "test" @>
                        test <@ res.Value.xml = Some "<tag />" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Params =
                                                List.map DbGen.TableTypes.dbo.AllTypesNull.create [
                                                    {|
                                                        bigint = Some 1L
                                                        binary = Some(Array.replicate 42 1uy)
                                                        bit = Some true
                                                        char = Some(String.replicate 42 "a")
                                                        date = Some(DateTime(2000, 1, 1))
                                                        datetime = Some(DateTime(2000, 1, 1))
                                                        datetime2 = Some(DateTime(2000, 1, 1))
                                                        datetimeoffset =
                                                            Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))
                                                        decimal = Some 1M
                                                        float = Some 1.
                                                        image = Some [| 1uy |]
                                                        int = Some 1
                                                        money = Some 1M
                                                        nchar = Some(String.replicate 42 "a")
                                                        ntext = Some "test"
                                                        numeric = Some 1M
                                                        nvarchar = Some "test"
                                                        real = Some 1.f
                                                        smalldatetime = Some(DateTime(2000, 1, 1))
                                                        smallint = Some 1s
                                                        smallmoney = Some 1M
                                                        text = Some "test"
                                                        time = Some(TimeSpan.FromSeconds 1.)
                                                        tinyint = Some 1uy
                                                        uniqueidentifier =
                                                            Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145"))
                                                        varbinary = Some [| 1uy |]
                                                        varchar = Some "test"
                                                        xml = Some "<tag />"
                                                    |}
                                                ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.bigint = Some 1L @>
                            test <@ res.Value.binary = Some(Array.replicate 42 1uy) @>
                            test <@ res.Value.bit = Some true @>
                            test <@ res.Value.char = Some(String.replicate 42 "a") @>
                            test <@ res.Value.date = Some(DateTime(2000, 1, 1)) @>
                            test <@ res.Value.datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ res.Value.datetime2 = Some(DateTime(2000, 1, 1)) @>

                            test
                                <@ res.Value.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>

                            test <@ res.Value.decimal = Some 1M @>
                            test <@ res.Value.float = Some 1. @>
                            test <@ res.Value.image = Some [| 1uy |] @>
                            test <@ res.Value.int = Some 1 @>
                            test <@ res.Value.money = Some 1M @>
                            test <@ res.Value.nchar = Some(String.replicate 42 "a") @>
                            test <@ res.Value.ntext = Some "test" @>
                            test <@ res.Value.numeric = Some 1M @>
                            test <@ res.Value.nvarchar = Some "test" @>
                            test <@ res.Value.real = Some 1.f @>
                            test <@ res.Value.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ res.Value.smallint = Some 1s @>
                            test <@ res.Value.smallmoney = Some 1M @>
                            test <@ res.Value.text = Some "test" @>
                            test <@ res.Value.time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ res.Value.tinyint = Some 1uy @>
                            test <@ res.Value.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ res.Value.varbinary = Some [| 1uy |] @>
                            test <@ res.Value.varchar = Some "test" @>
                            test <@ res.Value.xml = Some "<tag />" @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull + "_allNull") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = None @>
                        test <@ res.Value.binary = None @>
                        test <@ res.Value.bit = None @>
                        test <@ res.Value.char = None @>
                        test <@ res.Value.date = None @>
                        test <@ res.Value.datetime = None @>
                        test <@ res.Value.datetime2 = None @>
                        test <@ res.Value.datetimeoffset = None @>
                        test <@ res.Value.decimal = None @>
                        test <@ res.Value.float = None @>
                        test <@ res.Value.image = None @>
                        test <@ res.Value.int = None @>
                        test <@ res.Value.money = None @>
                        test <@ res.Value.nchar = None @>
                        test <@ res.Value.ntext = None @>
                        test <@ res.Value.numeric = None @>
                        test <@ res.Value.nvarchar = None @>
                        test <@ res.Value.real = None @>
                        test <@ res.Value.smalldatetime = None @>
                        test <@ res.Value.smallint = None @>
                        test <@ res.Value.smallmoney = None @>
                        test <@ res.Value.text = None @>
                        test <@ res.Value.time = None @>
                        test <@ res.Value.tinyint = None @>
                        test <@ res.Value.uniqueidentifier = None @>
                        test <@ res.Value.varbinary = None @>
                        test <@ res.Value.varchar = None @>
                        test <@ res.Value.xml = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended + "_allNull") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    ``params`` = [
                                        DbGen.TableTypes.dbo.AllTypesNull.create (
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
                                    ]
                                )
                            |> exec

                        test <@ res.Value.bigint = None @>
                        test <@ res.Value.binary = None @>
                        test <@ res.Value.bit = None @>
                        test <@ res.Value.char = None @>
                        test <@ res.Value.date = None @>
                        test <@ res.Value.datetime = None @>
                        test <@ res.Value.datetime2 = None @>
                        test <@ res.Value.datetimeoffset = None @>
                        test <@ res.Value.decimal = None @>
                        test <@ res.Value.float = None @>
                        test <@ res.Value.image = None @>
                        test <@ res.Value.int = None @>
                        test <@ res.Value.money = None @>
                        test <@ res.Value.nchar = None @>
                        test <@ res.Value.ntext = None @>
                        test <@ res.Value.numeric = None @>
                        test <@ res.Value.nvarchar = None @>
                        test <@ res.Value.real = None @>
                        test <@ res.Value.smalldatetime = None @>
                        test <@ res.Value.smallint = None @>
                        test <@ res.Value.smallmoney = None @>
                        test <@ res.Value.text = None @>
                        test <@ res.Value.time = None @>
                        test <@ res.Value.tinyint = None @>
                        test <@ res.Value.uniqueidentifier = None @>
                        test <@ res.Value.varbinary = None @>
                        test <@ res.Value.varchar = None @>
                        test <@ res.Value.xml = None @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Params =
                                                List.map DbGen.TableTypes.dbo.AllTypesNull.create [
                                                    {|
                                                        bigint = None
                                                        binary = None
                                                        bit = None
                                                        char = None
                                                        date = None
                                                        datetime = None
                                                        datetime2 = None
                                                        datetimeoffset = None
                                                        decimal = None
                                                        float = None
                                                        image = None
                                                        int = None
                                                        money = None
                                                        nchar = None
                                                        ntext = None
                                                        numeric = None
                                                        nvarchar = None
                                                        real = None
                                                        smalldatetime = None
                                                        smallint = None
                                                        smallmoney = None
                                                        text = None
                                                        time = None
                                                        tinyint = None
                                                        uniqueidentifier = None
                                                        varbinary = None
                                                        varchar = None
                                                        xml = None
                                                    |}
                                                ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.bigint = None @>
                            test <@ res.Value.binary = None @>
                            test <@ res.Value.bit = None @>
                            test <@ res.Value.char = None @>
                            test <@ res.Value.date = None @>
                            test <@ res.Value.datetime = None @>
                            test <@ res.Value.datetime2 = None @>
                            test <@ res.Value.datetimeoffset = None @>
                            test <@ res.Value.decimal = None @>
                            test <@ res.Value.float = None @>
                            test <@ res.Value.image = None @>
                            test <@ res.Value.int = None @>
                            test <@ res.Value.money = None @>
                            test <@ res.Value.nchar = None @>
                            test <@ res.Value.ntext = None @>
                            test <@ res.Value.numeric = None @>
                            test <@ res.Value.nvarchar = None @>
                            test <@ res.Value.real = None @>
                            test <@ res.Value.smalldatetime = None @>
                            test <@ res.Value.smallint = None @>
                            test <@ res.Value.smallmoney = None @>
                            test <@ res.Value.text = None @>
                            test <@ res.Value.time = None @>
                            test <@ res.Value.tinyint = None @>
                            test <@ res.Value.uniqueidentifier = None @>
                            test <@ res.Value.varbinary = None @>
                            test <@ res.Value.varchar = None @>
                            test <@ res.Value.xml = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Params =
                                                List.map DbGen.TableTypes.dbo.AllTypesNull.create [
                                                    {|
                                                        bigint = None
                                                        binary = None
                                                        bit = None
                                                        char = None
                                                        date = None
                                                        datetime = None
                                                        datetime2 = None
                                                        datetimeoffset = None
                                                        decimal = None
                                                        float = None
                                                        image = None
                                                        int = None
                                                        money = None
                                                        nchar = None
                                                        ntext = None
                                                        numeric = None
                                                        nvarchar = None
                                                        real = None
                                                        smalldatetime = None
                                                        smallint = None
                                                        smallmoney = None
                                                        text = None
                                                        time = None
                                                        tinyint = None
                                                        uniqueidentifier = None
                                                        varbinary = None
                                                        varchar = None
                                                        xml = None
                                                    |}
                                                ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.bigint = None @>
                            test <@ res.Value.binary = None @>
                            test <@ res.Value.bit = None @>
                            test <@ res.Value.char = None @>
                            test <@ res.Value.date = None @>
                            test <@ res.Value.datetime = None @>
                            test <@ res.Value.datetime2 = None @>
                            test <@ res.Value.datetimeoffset = None @>
                            test <@ res.Value.decimal = None @>
                            test <@ res.Value.float = None @>
                            test <@ res.Value.image = None @>
                            test <@ res.Value.int = None @>
                            test <@ res.Value.money = None @>
                            test <@ res.Value.nchar = None @>
                            test <@ res.Value.ntext = None @>
                            test <@ res.Value.numeric = None @>
                            test <@ res.Value.nvarchar = None @>
                            test <@ res.Value.real = None @>
                            test <@ res.Value.smalldatetime = None @>
                            test <@ res.Value.smallint = None @>
                            test <@ res.Value.smallmoney = None @>
                            test <@ res.Value.text = None @>
                            test <@ res.Value.time = None @>
                            test <@ res.Value.tinyint = None @>
                            test <@ res.Value.uniqueidentifier = None @>
                            test <@ res.Value.varbinary = None @>
                            test <@ res.Value.varchar = None @>
                            test <@ res.Value.xml = None @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParams, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParams.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value.Foo = 1 @>
                        test <@ res.Value.Bar = "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParamsExtended, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParamsExtended.WithConnection(
                                Config.connStr
                            )
                            |> exec

                        test <@ res.Value.Foo = 1 @>
                        test <@ res.Value.Bar = "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1, bar = "test")
                            |> exec

                        test <@ res.Value.Foo.Value = 1 @>
                        test <@ res.Value.Bar.Value = "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1, bar = "test")
                            |> exec

                        test <@ res.Value.Foo.Value = 1 @>
                        test <@ res.Value.Bar.Value = "test" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = 1; Bar = "test" |})
                                |> exec

                            test <@ res.Value.Foo.Value = 1 @>
                            test <@ res.Value.Bar.Value = "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = 1; Bar = "test" |})
                                |> exec

                            test <@ res.Value.Foo.Value = 1 @>
                            test <@ res.Value.Bar.Value = "test" @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1, bar = "test")
                            |> exec

                        test <@ res.Value.Foo.Value = 1 @>
                        test <@ res.Value.Bar.Value = "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1, bar = "test")
                            |> exec

                        test <@ res.Value.Foo.Value = 1 @>
                        test <@ res.Value.Bar.Value = "test" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = 1; Bar = "test" |})
                                |> exec

                            test <@ res.Value.Foo.Value = 1 @>
                            test <@ res.Value.Bar.Value = "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = 1; Bar = "test" |})
                                |> exec

                            test <@ res.Value.Foo.Value = 1 @>
                            test <@ res.Value.Bar.Value = "test" @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = Some 1, bar = Some "test")
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = Some 1, bar = Some "test")
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = Some 1; Bar = Some "test" |})
                                |> exec

                            test <@ res.Value.Foo = Some 1 @>
                            test <@ res.Value.Bar = Some "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = Some 1; Bar = Some "test" |})
                                |> exec

                            test <@ res.Value.Foo = Some 1 @>
                            test <@ res.Value.Bar = Some "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
             + "_allNull")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(foo = None, bar = None)
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
             + "_allNull")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(foo = None, bar = None)
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = None; Bar = None |})
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNullParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = None; Bar = None |})
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    single = [ DbGen.TableTypes.dbo.SingleColNonNull.create (Foo = 1) ],
                                    multi = [ DbGen.TableTypes.dbo.MultiColNonNull.create (Foo = 1, Bar = "test") ]
                                )
                            |> exec

                        test <@ res.Value.Foo = 1 @>
                        test <@ res.Value.Bar = "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    single = [ DbGen.TableTypes.dbo.SingleColNonNull.create (Foo = 1) ],
                                    multi = [ DbGen.TableTypes.dbo.MultiColNonNull.create (Foo = 1, Bar = "test") ]
                                )
                            |> exec

                        test <@ res.Value.Foo = 1 @>
                        test <@ res.Value.Bar = "test" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNonNull.create {| Foo = 1 |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNonNull.create {| Foo = 1; Bar = "test" |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = 1 @>
                            test <@ res.Value.Bar = "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNonNull.create {| Foo = 1 |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNonNull.create {| Foo = 1; Bar = "test" |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = 1 @>
                            test <@ res.Value.Bar = "test" @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    single = [ DbGen.TableTypes.dbo.SingleColNull.create (Foo = Some 1) ],
                                    multi = [
                                        DbGen.TableTypes.dbo.MultiColNull.create (Foo = Some 1, Bar = Some "test")
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    single = [ DbGen.TableTypes.dbo.SingleColNull.create (Foo = Some 1) ],
                                    multi = [
                                        DbGen.TableTypes.dbo.MultiColNull.create (Foo = Some 1, Bar = Some "test")
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNull.create {| Foo = Some 1 |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNull.create {|
                                                    Foo = Some 1
                                                    Bar = Some "test"
                                                |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = Some 1 @>
                            test <@ res.Value.Bar = Some "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNull.create {| Foo = Some 1 |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNull.create {|
                                                    Foo = Some 1
                                                    Bar = Some "test"
                                                |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = Some 1 @>
                            test <@ res.Value.Bar = Some "test" @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
             + "_allNull")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        single = [ DbGen.TableTypes.dbo.SingleColNull.create (Foo = None) ],
                                        multi = [ DbGen.TableTypes.dbo.MultiColNull.create (Foo = None, Bar = None) ]
                                    )
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
             + "_allNull")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        single = [ DbGen.TableTypes.dbo.SingleColNull.create (Foo = None) ],
                                        multi = [ DbGen.TableTypes.dbo.MultiColNull.create (Foo = None, Bar = None) ]
                                    )
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParams
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNull.create {| Foo = None |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNull.create {| Foo = None; Bar = None |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
             + "_allNull_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithMultipleNullableColumnsAndTvpParamsExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters(
                                        {|
                                            Single = [ DbGen.TableTypes.dbo.SingleColNull.create {| Foo = None |} ]
                                            Multi = [
                                                DbGen.TableTypes.dbo.MultiColNull.create {| Foo = None; Bar = None |}
                                            ]
                                        |}
                                    )
                                |> exec

                            test <@ res.Value.Foo = None @>
                            test <@ res.Value.Bar = None @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNames) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNames, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNames.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value.``This is the first column`` = "foo" @>
                        test <@ res.Value.``!"#%&/()=?`` = 2 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNamesExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNamesExtended, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNonFSharpFriendlyNamesExtended.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value.``This is the first column`` = "foo" @>
                        test <@ res.Value.``!"#%&/()=?`` = 2 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNoResults) [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResults_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNoResults
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 2)
                            |> exec

                        test <@ res = -1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNoResultsExtended) [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResultsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNoResultsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 2)
                            |> exec

                        test <@ res = -1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNoResults + "_paramsFromDto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResults_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNoResults
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = 2 |})
                            |> exec

                        test <@ res = -1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithNoResultsExtended + "_paramsFromDto") [
            yield!
                allNonResultExecuteMethods<DbGen.Procedures.dbo.ProcWithNoResultsExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithNoResultsExtended
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = 2 |})
                            |> exec

                        test <@ res = -1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParams, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParams.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value = 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParamsExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParamsExtended, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleColumnAndNoParamsExtended.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value = 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNamelessColumn) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNamelessColumn, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNamelessColumn.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value = 123 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNamelessColumnExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNamelessColumnExtended, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNamelessColumnExtended.WithConnection(Config.connStr)
                            |> exec

                        test <@ res.Value = 123 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNonNullColumn) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNonNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNonNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1)
                            |> exec

                        test <@ res.Value.Value = 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = 1)
                            |> exec

                        test <@ res.Value.Value = 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNonNullColumn + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNonNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNonNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = 1 |})
                            |> exec

                        test <@ res.Value.Value = 1 @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended
             + "_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithSingleNonNullColumnExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = 1 |})
                                |> exec

                            test <@ res.Value.Value = 1 @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumn) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = Some 1)
                            |> exec

                        test <@ res.Value = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = Some 1)
                            |> exec

                        test <@ res.Value = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumn + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = Some 1 |})
                            |> exec

                        test <@ res.Value = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = Some 1 |})
                            |> exec

                        test <@ res.Value = Some 1 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumn + "_null") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = None)
                            |> exec

                        test <@ res.Value = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended + "_null") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(foo = None)
                            |> exec

                        test <@ res.Value = None @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumn + "_null_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumn_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSingleNullColumn
                                .WithConnection(Config.connStr)
                                .WithParameters({| Foo = None |})
                            |> exec

                        test <@ res.Value = None @>
                )
        ]


        testList
            (nameof DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended
             + "_null_paramsFromDto")
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended_Executable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Procedures.dbo.ProcWithSingleNullColumnExtended
                                    .WithConnection(Config.connStr)
                                    .WithParameters({| Foo = None |})
                                |> exec

                            test <@ res.Value = None @>
                    )
            ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSpecialCasing) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpecialCasing_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSpecialCasing
                                .WithConnection(Config.connStr)
                                .WithParameters(PARAM1 = 1, Param2 = 2, param3 = 3)
                            |> exec

                        test <@ res.Value.COL1.Value = 1 @>
                        test <@ res.Value.Col2.Value = 2 @>
                        test <@ res.Value.col3.Value = 3 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSpecialCasingExtended) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpecialCasingExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSpecialCasingExtended
                                .WithConnection(Config.connStr)
                                .WithParameters(PARAM1 = 1, Param2 = 2, param3 = 3)
                            |> exec

                        test <@ res.Value.COL1.Value = 1 @>
                        test <@ res.Value.Col2.Value = 2 @>
                        test <@ res.Value.col3.Value = 3 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSpecialCasing + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpecialCasing_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSpecialCasing
                                .WithConnection(Config.connStr)
                                .WithParameters({| PARAM1 = 1; Param2 = 2; Param3 = 3 |})
                            |> exec

                        test <@ res.Value.COL1.Value = 1 @>
                        test <@ res.Value.Col2.Value = 2 @>
                        test <@ res.Value.col3.Value = 3 @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithSpecialCasingExtended + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithSpecialCasingExtended_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithSpecialCasingExtended
                                .WithConnection(Config.connStr)
                                .WithParameters({| PARAM1 = 1; Param2 = 2; Param3 = 3 |})
                            |> exec

                        test <@ res.Value.COL1.Value = 1 @>
                        test <@ res.Value.Col2.Value = 2 @>
                        test <@ res.Value.col3.Value = 3 @>
                )
        ]

        testList (nameof DbGen.Scripts.ManyColumns) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.ManyColumns, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res = DbGen.Scripts.ManyColumns.WithConnection(Config.connStr) |> exec
                        test <@ res.Value.Column1 = None @>
                        test <@ res.Value.Column600 = None @>
                )
        ]

        testList (nameof DbGen.Scripts.NormalParams) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.NormalParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.NormalParams
                                .WithConnection(Config.connStr)
                                .WithParameters(col1 = "test1")
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.NormalParams + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.NormalParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.NormalParams
                                .WithConnection(Config.connStr)
                                .WithParameters({| Col1 = "test1" |})
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.ParamsUsedTwice) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.ParamsUsedTwice_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.ParamsUsedTwice
                                .WithConnection(Config.connStr)
                                .WithParameters(col1 = Some "test1")
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.ParamsUsedTwice + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.ParamsUsedTwice_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.ParamsUsedTwice
                                .WithConnection(Config.connStr)
                                .WithParameters({| Col1 = Some "test1" |})
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.OptionRecompileAndFetch) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.OptionRecompileAndFetch_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.OptionRecompileAndFetch
                                .WithConnection(Config.connStr)
                                .WithParameters(offset = 0L, limit = 10L)
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.OptionRecompileAndFetch + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.OptionRecompileAndFetch_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.OptionRecompileAndFetch
                                .WithConnection(Config.connStr)
                                .WithParameters({| Offset = 0L; Limit = 10L |})
                            |> exec

                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.SelectAllFromTable) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.SelectAllFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res = DbGen.Scripts.SelectAllFromTable.WithConnection(Config.connStr) |> exec
                        test <@ res.Value.TableCol1 = "test1" @>
                        test <@ res.Value.TableCol2 = Some 1 @>
                )
        ]


        testList (nameof DbGen.Scripts.UserDefinedTableType) [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.UserDefinedTableType_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.UserDefinedTableType
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    tvp = [
                                        DbGen.TableTypes.dbo.MultiColNull.create (Foo = Some 1, Bar = Some "test")
                                    ]
                                )
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList (nameof DbGen.Scripts.UserDefinedTableType + "_paramsFromDto") [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.UserDefinedTableType_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.UserDefinedTableType
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    {|
                                        Tvp = [
                                            DbGen.TableTypes.dbo.MultiColNull.create (Foo = Some 1, Bar = Some "test")
                                        ]
                                    |}
                                )
                            |> exec

                        test <@ res.Value.Foo = Some 1 @>
                        test <@ res.Value.Bar = Some "test" @>
                )
        ]


        testList
            (nameof DbGen.Scripts.SubPath
             + "_"
             + nameof DbGen.Scripts.SubPath.SelectAllFromTable)
            [
                yield!
                    allExecuteMethodsAsSingle<DbGen.Scripts.SubPath.SelectAllFromTable, _>
                    |> List.map (fun (name, exec) ->
                        testCase name
                        <| fun () ->
                            let res =
                                DbGen.Scripts.SubPath.SelectAllFromTable.WithConnection(Config.connStr) |> exec

                            test <@ res.Value.TableCol1 = "test1" @>
                            test <@ res.Value.TableCol2 = Some 1 @>
                    )
            ]


    ]



[<Tests>]
let seqExecTests =
    testList "Multiple rows execute tests" [


        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTable) [
            yield!
                allSeqExecuteMethods<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(Config.connStr)
                            |> exec
                            |> Seq.toList

                        let expected: DbGen.TableDtos.dbo.Table1 list = [
                            {
                                TableCol1 = "test1"
                                TableCol2 = Some 1
                            }
                            {
                                TableCol1 = "test2"
                                TableCol2 = Some 2
                            }
                        ]

                        test <@ res = expected @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull) [
            yield!
                allSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let createTvpRow () =
                            DbGen.TableTypes.dbo.AllTypesNonNull.create (
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
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(``params`` = [ createTvpRow (); createTvpRow (); createTvpRow () ])
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.bigint = 1L @>
                            test <@ row.binary = Array.replicate 42 1uy @>
                            test <@ row.bit = true @>
                            test <@ row.char = String.replicate 42 "a" @>
                            test <@ row.date = DateTime(2000, 1, 1) @>
                            test <@ row.datetime = DateTime(2000, 1, 1) @>
                            test <@ row.datetime2 = DateTime(2000, 1, 1) @>
                            test <@ row.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ row.decimal = 1M @>
                            test <@ row.float = 1. @>
                            test <@ row.image = [| 1uy |] @>
                            test <@ row.int = 1 @>
                            test <@ row.money = 1M @>
                            test <@ row.nchar = String.replicate 42 "a" @>
                            test <@ row.ntext = "test" @>
                            test <@ row.numeric = 1M @>
                            test <@ row.nvarchar = "test" @>
                            test <@ row.real = 1.f @>
                            test <@ row.smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ row.smallint = 1s @>
                            test <@ row.smallmoney = 1M @>
                            test <@ row.text = "test" @>
                            test <@ row.time = TimeSpan.FromSeconds 1. @>
                            test <@ row.tinyint = 1uy @>
                            test <@ row.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ row.varbinary = [| 1uy |] @>
                            test <@ row.varchar = "test" @>
                            test <@ row.xml = "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull) [
            yield!
                allSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let createTvpRow () =
                            DbGen.TableTypes.dbo.AllTypesNull.create (
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
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                .WithConnection(Config.connStr)
                                .WithParameters(``params`` = [ createTvpRow (); createTvpRow (); createTvpRow () ])
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.bigint = Some 1L @>
                            test <@ row.binary = Some(Array.replicate 42 1uy) @>
                            test <@ row.bit = Some true @>
                            test <@ row.char = Some(String.replicate 42 "a") @>
                            test <@ row.date = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.datetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.datetime2 = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.datetimeoffset = Some(DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)) @>
                            test <@ row.decimal = Some 1M @>
                            test <@ row.float = Some 1. @>
                            test <@ row.image = Some [| 1uy |] @>
                            test <@ row.int = Some 1 @>
                            test <@ row.money = Some 1M @>
                            test <@ row.nchar = Some(String.replicate 42 "a") @>
                            test <@ row.ntext = Some "test" @>
                            test <@ row.numeric = Some 1M @>
                            test <@ row.nvarchar = Some "test" @>
                            test <@ row.real = Some 1.f @>
                            test <@ row.smalldatetime = Some(DateTime(2000, 1, 1)) @>
                            test <@ row.smallint = Some 1s @>
                            test <@ row.smallmoney = Some 1M @>
                            test <@ row.text = Some "test" @>
                            test <@ row.time = Some(TimeSpan.FromSeconds 1.) @>
                            test <@ row.tinyint = Some 1uy @>
                            test <@ row.uniqueidentifier = Some(Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145")) @>
                            test <@ row.varbinary = Some [| 1uy |] @>
                            test <@ row.varchar = Some "test" @>
                            test <@ row.xml = Some "<tag />" @>
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull + "_allNull") [
            yield!
                allSeqExecuteMethods<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let createTvpRow () =
                            DbGen.TableTypes.dbo.AllTypesNull.create (
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
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                                .WithConnection(Config.connStr)
                                .WithParameters(``params`` = [ createTvpRow (); createTvpRow (); createTvpRow () ])
                            |> exec

                        test <@ res.Count = 3 @>

                        for row in res do
                            test <@ row.bigint = None @>
                            test <@ row.binary = None @>
                            test <@ row.bit = None @>
                            test <@ row.char = None @>
                            test <@ row.date = None @>
                            test <@ row.datetime = None @>
                            test <@ row.datetime2 = None @>
                            test <@ row.datetimeoffset = None @>
                            test <@ row.decimal = None @>
                            test <@ row.float = None @>
                            test <@ row.image = None @>
                            test <@ row.int = None @>
                            test <@ row.money = None @>
                            test <@ row.nchar = None @>
                            test <@ row.ntext = None @>
                            test <@ row.numeric = None @>
                            test <@ row.nvarchar = None @>
                            test <@ row.real = None @>
                            test <@ row.smalldatetime = None @>
                            test <@ row.smallint = None @>
                            test <@ row.smallmoney = None @>
                            test <@ row.text = None @>
                            test <@ row.time = None @>
                            test <@ row.tinyint = None @>
                            test <@ row.uniqueidentifier = None @>
                            test <@ row.varbinary = None @>
                            test <@ row.varchar = None @>
                            test <@ row.xml = None @>
                )
        ]


    ]



[<Tests>]
let lazyExecTests =
    testList "Lazy execute tests" [


        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTable) [
            yield!
                allLazyExecuteMethods<DbGen.Procedures.dbo.ProcSelectFromTable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {
                        let res =
                            DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(Config.connStr) |> exec


                        let expected: DbGen.TableDtos.dbo.Table1 list = [
                            {
                                TableCol1 = "test1"
                                TableCol2 = Some 1
                            }
                            {
                                TableCol1 = "test2"
                                TableCol2 = Some 2
                            }
                        ]

                        for row, expected in Seq.zip res expected do
                            do! Async.Sleep 500
                            test <@ row = expected @>
                    }
                )
        ]


        testList (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull) [
            yield!
                allLazyExecuteMethods<DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCaseAsync name
                    <| async {

                        let createTvpRow () =
                            DbGen.TableTypes.dbo.AllTypesNonNull.create (
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
                            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                                .WithConnection(Config.connStr)
                                .WithParameters(``params`` = [ createTvpRow (); createTvpRow (); createTvpRow () ])
                            |> exec

                        let mutable fetched = 0

                        for row in res do
                            fetched <- fetched + 1
                            do! Async.Sleep 500
                            test <@ row.bigint = 1L @>
                            test <@ row.binary = Array.replicate 42 1uy @>
                            test <@ row.bit = true @>
                            test <@ row.char = String.replicate 42 "a" @>
                            test <@ row.date = DateTime(2000, 1, 1) @>
                            test <@ row.datetime = DateTime(2000, 1, 1) @>
                            test <@ row.datetime2 = DateTime(2000, 1, 1) @>
                            test <@ row.datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) @>
                            test <@ row.decimal = 1M @>
                            test <@ row.float = 1. @>
                            test <@ row.image = [| 1uy |] @>
                            test <@ row.int = 1 @>
                            test <@ row.money = 1M @>
                            test <@ row.nchar = String.replicate 42 "a" @>
                            test <@ row.ntext = "test" @>
                            test <@ row.numeric = 1M @>
                            test <@ row.nvarchar = "test" @>
                            test <@ row.real = 1.f @>
                            test <@ row.smalldatetime = DateTime(2000, 1, 1) @>
                            test <@ row.smallint = 1s @>
                            test <@ row.smallmoney = 1M @>
                            test <@ row.text = "test" @>
                            test <@ row.time = TimeSpan.FromSeconds 1. @>
                            test <@ row.tinyint = 1uy @>
                            test <@ row.uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") @>
                            test <@ row.varbinary = [| 1uy |] @>
                            test <@ row.varchar = "test" @>
                            test <@ row.xml = "<tag />" @>

                        let fetched = fetched
                        test <@ fetched = 3 @>
                    }

                )
        ]

    ]


[<Tests>]
let readerSingleExecTests =
    testList "Single rows SqlDataReader execute tests" [

        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTable) [
            yield!
                allSingleReaderExecuteMethods<DbGen.Procedures.dbo.ProcSelectFromTable>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        use d =
                            DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(Config.connStr) |> exec

                        test <@ d.Reader.Read() = true @>
                        test <@ d.Reader["TableCol1"] = "test1" @>
                        test <@ d.Reader["TableCol2"] = 1 @>

                        test <@ d.Reader.Read() = false @>
                )
        ]
    ]


[<Tests>]
let readerSeqExecTests =
    testList "Multiple rows SqlDataReader execute tests" [

        testList (nameof DbGen.Procedures.dbo.ProcSelectFromTable) [
            yield!
                allMultiReaderExecuteMethods<DbGen.Procedures.dbo.ProcSelectFromTable>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        use d =
                            DbGen.Procedures.dbo.ProcSelectFromTable.WithConnection(Config.connStr) |> exec

                        test <@ d.Reader.Read() = true @>
                        test <@ d.Reader["TableCol1"] = "test1" @>
                        test <@ d.Reader["TableCol2"] = 1 @>

                        test <@ d.Reader.Read() = true @>
                        test <@ d.Reader["TableCol1"] = "test2" @>
                        test <@ d.Reader["TableCol2"] = 2 @>

                        test <@ d.Reader.Read() = false @>
                )
        ]
    ]



[<Tests>]
let cancellationTests =
    testSequenced
    <| testList "Cancellation tests" [


        testCaseAsync "Can cancel non-queries"
        <| async {
            use cts = new CancellationTokenSource()
            cts.CancelAfter(1000)

            let sut = DbGen.Scripts.LongRunningNonQuery.WithConnection(Config.connStr)

            let mutable isAsyncCanceled = false

            async {
                use! __ = Async.OnCancel(fun () -> isAsyncCanceled <- true)
                return! sut.AsyncExecute()
            }
            |> Async.Ignore<int>
            |> fun comp -> Async.Start(comp, cts.Token)

            let task = sut.ExecuteAsync(cts.Token)

            do! Async.Sleep 1500

            let isAsyncCanceled' = isAsyncCanceled
            test <@ isAsyncCanceled' @>
            test <@ task.IsCanceled @>
        }


        testCaseAsync "Can cancel queries"
        <| async {
            use cts = new CancellationTokenSource()
            cts.CancelAfter(1000)

            let sut = DbGen.Scripts.LongRunningQuery.WithConnection(Config.connStr)

            let comps = [
                sut.AsyncExecute() |> Async.Ignore<ResizeArray<int>>
                sut.AsyncExecuteWithSyncRead() |> Async.Ignore<ResizeArray<int>>
                sut.AsyncExecuteSingle() |> Async.Ignore<int option>
                sut.AsyncExecuteReader() |> Async.Ignore<FacilReaderDisposer>
                sut.AsyncExecuteReaderSingle() |> Async.Ignore<FacilReaderDisposer>
            ]

            let asyncCancellations = Array.zeroCreate comps.Length

            comps
            |> List.iteri (fun i comp ->
                async {
                    use! __ = Async.OnCancel(fun () -> asyncCancellations[i] <- true)
                    return! comp
                }
                |> fun comp -> Async.Start(comp, cts.Token)
            )

            let tasks = [
                sut.ExecuteAsync(cts.Token) :> Task
                sut.ExecuteAsyncWithSyncRead(cts.Token)
                sut.ExecuteSingleAsync(cts.Token)
                sut.ExecuteReaderAsync(cts.Token)
                sut.ExecuteReaderSingleAsync(cts.Token)
            ]

            do! Async.Sleep 1500

            test <@ tasks |> List.map _.IsCanceled |> List.forall id @>
            test <@ asyncCancellations |> Array.forall id @>
        }


    ]
