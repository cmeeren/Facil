module NominalParamDtoTests

open System
open Expecto


[<Tests>]
let tests =
    testList "Nominal param DTO tests" [


        testCase (nameof DbGen.Procedures.dbo.ProcWithAllTypesNominalParams)
        <| fun () ->
            let dto: DbGen.Procedures.dbo.ProcWithAllTypesNominalParams_Params = {
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
            }

            DbGen.Procedures.dbo.ProcWithAllTypesNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


        testCase (nameof DbGen.Procedures.dbo.ProcWithAllTypesNullNominalParams)
        <| fun () ->
            let dto: DbGen.Procedures.dbo.ProcWithAllTypesNullNominalParams_Params = {
                Bigint = 1L |> Some
                Binary = Array.replicate 42 1uy |> Some
                Bit = true |> Some
                Char = String.replicate 42 "a" |> Some
                Date = DateTime(2000, 1, 1) |> Some
                Datetime = DateTime(2000, 1, 1) |> Some
                Datetime2 = DateTime(2000, 1, 1) |> Some
                Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) |> Some
                Decimal = 1M |> Some
                Float = 1. |> Some
                Image = [| 1uy |] |> Some
                Int = 1 |> Some
                Money = 1M |> Some
                Nchar = String.replicate 42 "a" |> Some
                Ntext = "test" |> Some
                Numeric = 1M |> Some
                Nvarchar = "test" |> Some
                Real = 1.f |> Some
                Rowversion = Array.replicate 8 1uy |> Some
                Smalldatetime = DateTime(2000, 1, 1) |> Some
                Smallint = 1s |> Some
                Smallmoney = 1M |> Some
                Text = "test" |> Some
                Time = TimeSpan.FromSeconds 1. |> Some
                Timestamp = Array.replicate 8 1uy |> Some
                Tinyint = 1uy |> Some
                Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") |> Some
                Varbinary = [| 1uy |] |> Some
                Varchar = "test" |> Some
                Xml = "<tag />" |> Some
            }

            DbGen.Procedures.dbo.ProcWithAllTypesNullNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


        testCase (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullNominalParams)
        <| fun () ->
            let dto: DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullNominalParams_Params = {
                Params = [
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
            }

            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


        testCase (nameof DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullNominalParams)
        <| fun () ->
            let dto: DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullNominalParams_Params = {
                Params = [
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
            }

            DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


        testCase (nameof DbGen.Scripts.TempTableAllTypesNonNullNominalParams)
        <| fun () ->
            let dto: DbGen.Scripts.TempTableAllTypesNonNullNominalParams_Params = {
                AllTypesNonNull = [
                    DbGen.Scripts.TempTableAllTypesNonNullNominalParams.AllTypesNonNull.create (
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
                ]
            }

            DbGen.Scripts.TempTableAllTypesNonNullNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


        testCase (nameof DbGen.Scripts.TempTableAllTypesNullNominalParams)
        <| fun () ->
            let dto: DbGen.Scripts.TempTableAllTypesNullNominalParams_Params = {
                AllTypesNull = [
                    DbGen.Scripts.TempTableAllTypesNullNominalParams.AllTypesNull.create (
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
                ]
            }

            DbGen.Scripts.TempTableAllTypesNullNominalParams
                .WithConnection(Config.connStr)
                .WithParameters(dto)
                .Execute()
            |> ignore


    ]
