module MiscTests

open System
open Expecto
open Hedgehog
open Microsoft.Data.SqlClient
open Swensen.Unquote


[<Tests>]
let tests =
    testList "Misc tests" [

        testSequenced
        <| testList "Execution is resilient against runtime column order changes in result sets" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcToBeModified, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->

                        let revertColumnOrder () =
                            use conn = new SqlConnection(Config.connStr)
                            conn.Open()
                            use cmd = conn.CreateCommand()

                            cmd.CommandText <-
                                "
                    ALTER PROCEDURE [dbo].[ProcToBeModified]
                    AS

                    SELECT
                      Foo = 1,
                      Bar = 'test'
                  "

                            cmd.ExecuteNonQuery() |> ignore

                        let changeColumnOrder () =
                            use conn = new SqlConnection(Config.connStr)
                            conn.Open()
                            use cmd = conn.CreateCommand()

                            cmd.CommandText <-
                                "
                    ALTER PROCEDURE [dbo].[ProcToBeModified]
                    AS

                    SELECT
                      Bar = 'test',
                      Foo = 1
                  "

                            cmd.ExecuteNonQuery() |> ignore

                        let test () =
                            let res =
                                DbGen.Procedures.dbo.ProcToBeModified.WithConnection(Config.connStr) |> exec

                            test <@ res.Value.Foo = 1 @>
                            test <@ res.Value.Bar = "test" @>

                        try
                            revertColumnOrder () // Just to be safe
                            test ()
                            changeColumnOrder ()
                            test ()
                        finally
                            revertColumnOrder ()
                )
        ]


        testList "Dynamic SQL works as expected with explicit declaration" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.DynamicSqlWithDeclaration_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.DynamicSqlWithDeclaration
                                .WithConnection(Config.connStr)
                                .WithParameters(col1Filter = "test2")
                            |> exec

                        test <@ res.Value.TableCol1 = "test2" @>
                        test <@ res.Value.TableCol2 = Some 2 @>
                )
        ]


        testList "Dynamic SQL works as expected with config declaration" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Scripts.DynamicSqlWithoutDeclaration_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Scripts.DynamicSqlWithoutDeclaration
                                .WithConnection(Config.connStr)
                                .WithParameters(col1Filter = "test2")
                            |> exec

                        test <@ res.Value.TableCol1 = "test2" @>
                        test <@ res.Value.TableCol2 = Some 2 @>
                )
        ]

        testList "Truncation behavior" [

            // This functionality is incidental, not a requirement; the test is here to pick
            // up potentially breaking changes, in order to ensure that any change is
            // deliberate.
            testCase "Normal parameters that are too long are silently truncated"
            <| fun () ->
                let res =
                    DbGen.Procedures.dbo.ProcWithLengthTypes
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            binary = [| 1uy; 2uy; 3uy; 4uy |],
                            char = "1234",
                            nchar = "1234",
                            nvarchar = "1234",
                            varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                            varchar = "1234"
                        )
                        .ExecuteSingle()

                test <@ res.Value.binary = Some [| 1uy; 2uy; 3uy |] @>
                test <@ res.Value.char = Some "123" @>
                test <@ res.Value.nchar = Some "123" @>
                test <@ res.Value.nvarchar = Some "123" @>
                test <@ res.Value.varbinary = Some [| 1uy; 2uy; 3uy |] @>
                test <@ res.Value.varchar = Some "123" @>


            // This functionality is incidental, not a requirement; the test is here to pick
            // up potentially breaking changes, in order to ensure that any change is
            // deliberate.
            testCase "TVP parameters that are too long are silently truncated"
            <| fun () ->
                let res =
                    DbGen.Procedures.dbo.ProcWithLengthTypesFromTvp
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.TableTypes.dbo.LengthTypes.create (
                                    binary = [| 1uy; 2uy; 3uy; 4uy |],
                                    char = "1234",
                                    nchar = "1234",
                                    nvarchar = "1234",
                                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                                    varchar = "1234"
                                )
                            ]
                        )
                        .ExecuteSingle()

                test <@ res.Value.binary = [| 1uy; 2uy; 3uy |] @>
                test <@ res.Value.char = "123" @>
                test <@ res.Value.nchar = "123" @>
                test <@ res.Value.nvarchar = "123" @>
                test <@ res.Value.varbinary = [| 1uy; 2uy; 3uy |] @>
                test <@ res.Value.varchar = "123" @>


            // This functionality is incidental, not a requirement; the test is here to pick
            // up potentially breaking changes, in order to ensure that any change is
            // deliberate.
            testCase "Temp table parameters that are too long raise exceptions"
            <| fun () ->
                let run () =
                    DbGen.Scripts.TempTableWithLengthTypes
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.TempTableWithLengthTypes.tempTableWithLengthTypes.create (
                                    binary = [| 1uy; 2uy; 3uy; 4uy |],
                                    char = "1234",
                                    nchar = "1234",
                                    nvarchar = "1234",
                                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                                    varchar = "1234"
                                )
                            ]
                        )
                        .ExecuteSingle()
                    |> ignore

                Expect.throws run "Should throw"


        ]


        testCase "Compile-time script param inheritance test"
        <| fun () ->
            DbGen.Scripts.ParamInheritance
                .WithConnection(Config.connStr)
                .WithParameters(col1 = Some 1, col2 = 1L, col3 = Some true)
            |> ignore


        testCase "Compile-time sproc column inheritance test"
        <| fun () ->
            let f () =
                let res =
                    DbGen.Procedures.dbo.ProcColumnInheritance
                        .WithConnection(Config.connStr)
                        .ExecuteSingle()

                res.Value.Col1 |> ignore<int>
                res.Value.Col2 |> ignore<int>

            ignore f


        testCase "Compile-time script column inheritance test"
        <| fun () ->
            let f () =
                let res =
                    DbGen.Scripts.ColumnInheritance.WithConnection(Config.connStr).ExecuteSingle()

                res.Value.Col1 |> ignore<int>
                res.Value.Col2 |> ignore<int>

            ignore f


        testCase "Compile-time table DTO column inheritance test"
        <| fun () ->
            let f () =
                let (x: DbGen.TableDtos.dbo.TableDtoColumnInheritance) = Unchecked.defaultof<_>
                x.Col1 |> ignore<int>
                x.Col2 |> ignore<int>

            ignore f


        testCase "Compile-time table DTO column order test (should ignore column order when finding matching DTOs)"
        <| fun () ->
            let f () =
                let res =
                    DbGen.Scripts.TableDtoWithDifferentColumnOrder
                        .WithConnection(Config.connStr)
                        .ExecuteSingle()

                ignore<DbGen.TableDtos.dbo.Table1 option> res

            ignore f


        testCase "Table DTO includeColumns and columns overrides work correctly"
        <| fun () ->
            let fieldNames =
                FSharp.Reflection.FSharpType.GetRecordFields(typeof<DbGen.TableDtos.dbo.TableWithComputedCol>)
                |> Array.map (fun pi -> pi.Name)

            Expect.sequenceEqual fieldNames [ "Id"; "Bar" ] ""


        testCase "Table DTO mappingCtor test - AllTypesNonNull"
        <| fun () ->
            let x = {|
                ExtraProperty = "foo"
                Key = 1
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

            let dto = DbGen.TableDtos.dbo.AllTypesNonNull.create x

            test <@ dto.Key = x.Key @>
            test <@ dto.Bigint = x.Bigint @>
            test <@ dto.Binary = x.Binary @>
            test <@ dto.Bit = x.Bit @>
            test <@ dto.Char = x.Char @>
            test <@ dto.Date = x.Date @>
            test <@ dto.Datetime = x.Datetime @>
            test <@ dto.Datetime2 = x.Datetime2 @>
            test <@ dto.Datetimeoffset = x.Datetimeoffset @>
            test <@ dto.Decimal = x.Decimal @>
            test <@ dto.Float = x.Float @>
            test <@ dto.Image = x.Image @>
            test <@ dto.Int = x.Int @>
            test <@ dto.Money = x.Money @>
            test <@ dto.Nchar = x.Nchar @>
            test <@ dto.Ntext = x.Ntext @>
            test <@ dto.Numeric = x.Numeric @>
            test <@ dto.Nvarchar = x.Nvarchar @>
            test <@ dto.Real = x.Real @>
            test <@ dto.Smalldatetime = x.Smalldatetime @>
            test <@ dto.Smallint = x.Smallint @>
            test <@ dto.Smallmoney = x.Smallmoney @>
            test <@ dto.Text = x.Text @>
            test <@ dto.Time = x.Time @>
            test <@ dto.Tinyint = x.Tinyint @>
            test <@ dto.Uniqueidentifier = x.Uniqueidentifier @>
            test <@ dto.Varbinary = x.Varbinary @>
            test <@ dto.Varchar = x.Varchar @>
            test <@ dto.Xml = x.Xml @>


        testCase "Table DTO mappingCtor test - AllTypesNull"
        <| fun () ->
            let x = {|
                ExtraProperty = Some "foo"
                Key1 = 1
                Key2 = 1
                Bigint = Some 1L
                Binary = Array.replicate 42 1uy |> Some
                Bit = Some true
                Char = String.replicate 42 "a" |> Some
                Date = DateTime(2000, 1, 1) |> Some
                Datetime = DateTime(2000, 1, 1) |> Some
                Datetime2 = DateTime(2000, 1, 1) |> Some
                Datetimeoffset = DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) |> Some
                Decimal = Some 1M
                Float = Some 1.
                Image = Some [| 1uy |]
                Int = Some 1
                Money = Some 1M
                Nchar = String.replicate 42 "a" |> Some
                Ntext = Some "test"
                Numeric = Some 1M
                Nvarchar = Some "test"
                Real = Some 1.f
                Smalldatetime = DateTime(2000, 1, 1) |> Some
                Smallint = Some 1s
                Smallmoney = Some 1M
                Text = Some "test"
                Time = TimeSpan.FromSeconds 1. |> Some
                Tinyint = Some 1uy
                Uniqueidentifier = Guid("0fdc8130-b9f1-4dec-9cbc-0f67cd70d145") |> Some
                Varbinary = Some [| 1uy |]
                Varchar = Some "test"
                Xml = Some "<tag />"
            |}

            let dto = DbGen.TableDtos.dbo.AllTypesNull.create x

            test <@ dto.Key1 = x.Key1 @>
            test <@ dto.Key2 = x.Key2 @>
            test <@ dto.Bigint = x.Bigint @>
            test <@ dto.Binary = x.Binary @>
            test <@ dto.Bit = x.Bit @>
            test <@ dto.Char = x.Char @>
            test <@ dto.Date = x.Date @>
            test <@ dto.Datetime = x.Datetime @>
            test <@ dto.Datetime2 = x.Datetime2 @>
            test <@ dto.Datetimeoffset = x.Datetimeoffset @>
            test <@ dto.Decimal = x.Decimal @>
            test <@ dto.Float = x.Float @>
            test <@ dto.Image = x.Image @>
            test <@ dto.Int = x.Int @>
            test <@ dto.Money = x.Money @>
            test <@ dto.Nchar = x.Nchar @>
            test <@ dto.Ntext = x.Ntext @>
            test <@ dto.Numeric = x.Numeric @>
            test <@ dto.Nvarchar = x.Nvarchar @>
            test <@ dto.Real = x.Real @>
            test <@ dto.Smalldatetime = x.Smalldatetime @>
            test <@ dto.Smallint = x.Smallint @>
            test <@ dto.Smallmoney = x.Smallmoney @>
            test <@ dto.Text = x.Text @>
            test <@ dto.Time = x.Time @>
            test <@ dto.Tinyint = x.Tinyint @>
            test <@ dto.Uniqueidentifier = x.Uniqueidentifier @>
            test <@ dto.Varbinary = x.Varbinary @>
            test <@ dto.Varchar = x.Varchar @>
            test <@ dto.Xml = x.Xml @>


        testCase "Compile-time buildValue test"
        <| fun () ->
            let f () =
                let res =
                    DbGen.Scripts.DynamicSqlSensitiveToParamValues
                        .WithConnection(Config.connStr)
                        .WithParameters("unused")
                        .ExecuteSingle()

                res.Value.TableCol1 |> ignore<string>
                res.Value.TableCol2 |> ignore<int option>

            ignore f


        testCase "Compile-time test of WITH RESULT SETS as a buildValue alternative"
        <| fun () ->
            let f () =
                let res =
                    DbGen.Scripts.DynamicSqlSensitiveToParamValuesWithResultSets
                        .WithConnection(Config.connStr)
                        .WithParameters("unused")
                        .ExecuteSingle()

                res.Value.TableCol1 |> ignore<string>
                res.Value.TableCol2 |> ignore<int option>

            ignore f


        testCase "Compile-time dynamic SQL with full-text predicate test"
        <| fun () ->
            let f () =
                DbGen.Procedures.dbo.ProcWithDynamicSqlWithFullTextSearch
                    .WithConnection(Config.connStr)
                    .WithParameters("unused")
                    .ExecuteSingle()
                |> ignore<string option>

            ignore f


        testCase "Compile-time table DTO casing test"
        <| fun () ->
            let f (x: DbGen.TableDtos.dbo.CamelCaseColNames) =
                ignore x.Col1
                ignore x.OtherCol

            ignore f


        testCase "Compile-time nominal result type sproc test"
        <| fun () ->
            let f () =
                let (result: DbGen.Procedures.dbo.ProcNominalResult_Result option) =
                    DbGen.Procedures.dbo.ProcNominalResult
                        .WithConnection(Config.connStr)
                        .ExecuteSingle()

                result.Value.TableCol1 |> ignore<string>
                result.Value.TableCol2 |> ignore<int option>

            ignore f


        testCase "Compile-time nominal result type script test"
        <| fun () ->
            let f () =
                let (result: DbGen.Scripts.NominalResult_Result option) =
                    DbGen.Scripts.NominalResult.WithConnection(Config.connStr).ExecuteSingle()

                result.Value.TableCol1 |> ignore<string>
                result.Value.TableCol2 |> ignore<int option>

            ignore f


        testCase "Compile-time view DTO test"
        <| fun () ->
            let f (x: DbGen.TableDtos.dbo.View1) =
                ignore<string> x.TableCol1
                ignore<int option> x.TableCol2
                ignore<int> x.Foo

            ignore f


        testCase "Compile-time manual result as DTO test"
        <| fun () ->
            let f () =
                DbGen.Scripts.ManualTableDtoResult
                    .WithConnection(Config.connStr)
                    .ExecuteSingle()
                |> ignore<DbGen.TableDtos.dbo.Table1 option>

            ignore f


        testCase
            "Compile-time test to ensure table scripts are produced even if there is no rule for the table DTO for that table (#25)"
        <| fun () -> DbGen.Scripts.OptionTableWithoutDto_All |> ignore


        testCase
            "Parametrized scripts have a public constructor that throws (but can be used for reflection or to aid SRTP)"
        <| fun () -> Expect.throws (DbGen.Procedures.dbo.ProcWithMultipleColumnsAndNoParams >> ignore) ""


        testCase
            "Unparametrized scripts have a public constructor that throws (but can be used for reflection or to aid SRTP)"
        <| fun () -> Expect.throws (DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleNonDefaultParams >> ignore) ""


        testList "MAX length work correctly" [


            testCase "Normal parameters"
            <| fun () ->
                let res =
                    DbGen.Procedures.dbo.ProcWithMaxLengthTypes
                        .WithConnection(Config.connStr)
                        .WithParameters(nvarchar = "1234", varbinary = [| 1uy; 2uy; 3uy; 4uy |], varchar = "1234")
                        .ExecuteSingle()

                test <@ res.Value.nvarchar = Some "1234" @>
                test <@ res.Value.varbinary = Some [| 1uy; 2uy; 3uy; 4uy |] @>
                test <@ res.Value.varchar = Some "1234" @>


            testCase "TVP parameters"
            <| fun () ->
                let res =
                    DbGen.Procedures.dbo.ProcWithMaxLengthTypesFromTvp
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.TableTypes.dbo.MaxLengthTypes.create (
                                    nvarchar = "1234",
                                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                                    varchar = "1234"
                                )
                            ]
                        )
                        .ExecuteSingle()

                test <@ res.Value.nvarchar = "1234" @>
                test <@ res.Value.varbinary = [| 1uy; 2uy; 3uy; 4uy |] @>
                test <@ res.Value.varchar = "1234" @>


            testCase "Temp table"
            <| fun () ->
                let res =
                    DbGen.Scripts.TempTableWithMaxLengthTypes
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.TempTableWithMaxLengthTypes.tempTableWithMaxLengthTypes.create (
                                    nvarchar = "1234",
                                    varbinary = [| 1uy; 2uy; 3uy; 4uy |],
                                    varchar = "1234"
                                )
                            ]
                        )
                        .ExecuteSingle()

                test <@ res.Value.nvarchar = "1234" @>
                test <@ res.Value.varbinary = [| 1uy; 2uy; 3uy; 4uy |] @>
                test <@ res.Value.varchar = "1234" @>


        ]


        testList "Can send empty TVPs" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                                .WithConnection(Config.connStr)
                                .WithParameters(single = [], multi = [])
                            |> exec

                        test <@ res.IsNone @>
                )
        ]


        testList "Can send empty TVPs - params from DTO" [
            yield!
                allExecuteMethodsAsSingle<DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams_Executable, _>
                |> List.map (fun (name, exec) ->
                    testCase name
                    <| fun () ->
                        let res =
                            DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                                .WithConnection(Config.connStr)
                                .WithParameters({| Single = []; Multi = [] |})
                            |> exec

                        test <@ res.IsNone @>
                )
        ]

        testAsync "Can run the resulting Async query computation several times when there are parameters" {
            let comp1 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
                    .WithConnection(Config.connStr)
                    .WithParameters(foo = 1, bar = "a")
                    .AsyncExecute()

            let comp2 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
                    .WithConnection(Config.connStr)
                    .WithParameters(foo = 1, bar = "a")
                    .AsyncExecuteWithSyncRead()

            let comp3 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndSimpleDefaultParams
                    .WithConnection(Config.connStr)
                    .WithParameters(foo = 1, bar = "a")
                    .AsyncExecuteSingle()

            do! comp1 |> Async.Ignore
            do! comp1 |> Async.Ignore

            do! comp2 |> Async.Ignore
            do! comp2 |> Async.Ignore

            do! comp3 |> Async.Ignore
            do! comp3 |> Async.Ignore
        }

        testAsync "Can run the resulting Async query computation several times when there are TVP parameters" {
            let comp1 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        single = [ DbGen.TableTypes.dbo.SingleColNonNull.create (Foo = 1) ],
                        multi = [ DbGen.TableTypes.dbo.MultiColNonNull.create (Foo = 1, Bar = "test") ]
                    )
                    .AsyncExecute()

            let comp2 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        single = [ DbGen.TableTypes.dbo.SingleColNonNull.create (Foo = 1) ],
                        multi = [ DbGen.TableTypes.dbo.MultiColNonNull.create (Foo = 1, Bar = "test") ]
                    )
                    .AsyncExecuteWithSyncRead()

            let comp3 =
                DbGen.Procedures.dbo.ProcWithMultipleColumnsAndTvpParams
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        single = [ DbGen.TableTypes.dbo.SingleColNonNull.create (Foo = 1) ],
                        multi = [ DbGen.TableTypes.dbo.MultiColNonNull.create (Foo = 1, Bar = "test") ]
                    )
                    .AsyncExecuteSingle()

            do! comp1 |> Async.Ignore
            do! comp1 |> Async.Ignore

            do! comp2 |> Async.Ignore
            do! comp2 |> Async.Ignore

            do! comp3 |> Async.Ignore
            do! comp3 |> Async.Ignore
        }

        testAsync "Can run the resulting Async query computation several times when there are temp tables" {
            let comp1 =
                DbGen.Scripts.MultipleTempTables
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        tempTable1 = [
                            DbGen.Scripts.MultipleTempTables.tempTable1.create (Col1 = 1, Col2 = Some "test")
                        ],
                        tempTable2 = [ DbGen.Scripts.MultipleTempTables.tempTable2.create (Col1 = 1, Col3 = "test") ]
                    )
                    .AsyncExecute()

            let comp2 =
                DbGen.Scripts.MultipleTempTables
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        tempTable1 = [
                            DbGen.Scripts.MultipleTempTables.tempTable1.create (Col1 = 1, Col2 = Some "test")
                        ],
                        tempTable2 = [ DbGen.Scripts.MultipleTempTables.tempTable2.create (Col1 = 1, Col3 = "test") ]
                    )
                    .AsyncExecute()

            let comp3 =
                DbGen.Scripts.MultipleTempTables
                    .WithConnection(Config.connStr)
                    .WithParameters(
                        tempTable1 = [
                            DbGen.Scripts.MultipleTempTables.tempTable1.create (Col1 = 1, Col2 = Some "test")
                        ],
                        tempTable2 = [ DbGen.Scripts.MultipleTempTables.tempTable2.create (Col1 = 1, Col3 = "test") ]
                    )
                    .AsyncExecute()

            do! comp1 |> Async.Ignore
            do! comp1 |> Async.Ignore

            do! comp2 |> Async.Ignore
            do! comp2 |> Async.Ignore

            do! comp3 |> Async.Ignore
            do! comp3 |> Async.Ignore
        }

        testAsync "Can run the resulting Async non-query computation several times when there are parameters" {
            let comp =
                DbGen.Procedures.dbo.ProcWithNoResults
                    .WithConnection(Config.connStr)
                    .WithParameters(foo = 1)
                    .AsyncExecute()

            do! comp |> Async.Ignore
            do! comp |> Async.Ignore
        }


        testList "Table DTO primary key tests" [


            testCase "Returns correct value for single-column keys"
            <| fun () ->
                Property.check
                <| property {
                    let! dto = GenX.auto<DbGen.TableDtos.dbo.MaxLengthTypes>

                    let expected = dto.Key
                    let actual = DbGen.TableDtos.dbo.MaxLengthTypes.getPrimaryKey dto

                    Expect.equal actual expected ""
                }


            testCase "Returns correct value for multi-column keys"
            <| fun () ->
                Property.check
                <| property {
                    let! dto = GenX.auto<DbGen.TableDtos.dbo.AllTypesNull>

                    let actual = DbGen.TableDtos.dbo.AllTypesNull.getPrimaryKey dto

                    Expect.equal actual.Key1 dto.Key1 "Key1"
                    Expect.equal actual.Key2 dto.Key2 "Key2"
                }


            testCase "Correctly handles skipped PK columns"
            <| fun () ->
                // Sanity check
                Expect.isSome staticGetPrimaryKeyMethod<DbGen.TableDtos.dbo.MaxLengthTypes> ""

                // Actual test
                Expect.isNone staticGetPrimaryKeyMethod<DbGen.TableDtos.dbo.TableWithSkippedPkColumn> ""
        ]


        testAsync "Modifications when executing scripts during build-time are rolled back" {
            // Make sure they are actually generated
            ignore DbGen.Scripts.DynamicInsertIntoDesignTimeExecuteTest
            ignore DbGen.Scripts.DynamicInsertIntoDesignTimeExecuteTest2

            let! res =
                DbGen.Scripts.DesignTimeExecuteTest_All
                    .WithConnection(Config.connStr)
                    .AsyncExecute()

            Expect.isEmpty res ""
        }

        testAsync "Throws if transaction is set both in WithConnection and ConfigureCommand" {
            use conn = new SqlConnection(Config.connStr)
            conn.Open()
            use tran = conn.BeginTransaction()


            let run () =
                DbGen.Procedures.dbo.ProcWithNoResults
                    .WithConnection(conn, tran)
                    .ConfigureCommand(fun cmd -> cmd.Transaction <- tran)
                    .WithParameters(foo = 1)
                    .Execute()
                |> ignore

            let cont (ex: exn) _ =
                Expect.stringContains ex.Message "WithConnection" ""
                Expect.stringContains ex.Message "ConfigureCommand" ""

            Expect.throwsC run cont ""
        }

    ]
