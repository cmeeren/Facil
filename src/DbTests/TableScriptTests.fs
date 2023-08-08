module TableScriptTests

open Expecto
open Hedgehog
open Microsoft.Data.SqlClient
open Swensen.Unquote


let clearTableScriptTables () =
    DbGen.Scripts.DeleteAllFromTableScriptTables
        .WithConnection(Config.connStr)
        .Execute()
    |> ignore<int>


[<Tests>]
let tests =
    testSequenced
    <| testList "Table script tests" [


        testList "Basic execute + can roundtrip values without losing precision" [


            testCase "Non-null INSERT/UPDATE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key = Gen.Sql.int

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ getRes = None @>

                    let! bigint = Gen.Sql.bigint
                    let! binary_42 = Gen.Sql.binary 42
                    let! bit = Gen.Sql.bit
                    let! char_42 = Gen.Sql.char 42
                    let! date = Gen.Sql.date
                    let! datetime = Gen.Sql.datetime
                    let! datetime2_3 = Gen.Sql.datetime2 3
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5 = Gen.Sql.decimal 10 5
                    let! float_42 = Gen.Sql.float 42
                    let! image = Gen.Sql.image
                    let! int = Gen.Sql.int
                    let! money = Gen.Sql.money
                    let! nchar_42 = Gen.Sql.nchar 42
                    let! ntext = Gen.Sql.ntext
                    let! numeric_8_3 = Gen.Sql.numeric 8 3
                    let! nvarchar_42 = Gen.Sql.nvarchar 42
                    let! real = Gen.Sql.real
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNonNull_Insert
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>


                    let! bigint = Gen.Sql.bigint
                    let! binary_42 = Gen.Sql.binary 42
                    let! bit = Gen.Sql.bit
                    let! char_42 = Gen.Sql.char 42
                    let! date = Gen.Sql.date
                    let! datetime = Gen.Sql.datetime
                    let! datetime2_3 = Gen.Sql.datetime2 3
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5 = Gen.Sql.decimal 10 5
                    let! float_42 = Gen.Sql.float 42
                    let! image = Gen.Sql.image
                    let! int = Gen.Sql.int
                    let! money = Gen.Sql.money
                    let! nchar_42 = Gen.Sql.nchar 42
                    let! ntext = Gen.Sql.ntext
                    let! numeric_8_3 = Gen.Sql.numeric 8 3
                    let! nvarchar_42 = Gen.Sql.nvarchar 42
                    let! real = Gen.Sql.real
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNonNull_Update
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>

                    let numDeletedRows =
                        DbGen.Scripts.AllTypesNonNull_Delete
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .Execute()

                    test <@ numDeletedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ getRes = None @>
                }


            testCase "Non-null INSERT/UPDATE batch"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key_A = Gen.Sql.int
                    let! key_B = Gen.Sql.int |> Gen.filter (not << (=) key_A)

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let! bigint_A = Gen.Sql.bigint
                    let! binary_42_A = Gen.Sql.binary 42
                    let! bit_A = Gen.Sql.bit
                    let! char_42_A = Gen.Sql.char 42
                    let! date_A = Gen.Sql.date
                    let! datetime_A = Gen.Sql.datetime
                    let! datetime2_3_A = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5
                    let! float_42_A = Gen.Sql.float 42
                    let! image_A = Gen.Sql.image
                    let! int_A = Gen.Sql.int
                    let! money_A = Gen.Sql.money
                    let! nchar_42_A = Gen.Sql.nchar 42
                    let! ntext_A = Gen.Sql.ntext
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42
                    let! real_A = Gen.Sql.real
                    let! smalldatetime_A = Gen.Sql.smalldatetime
                    let! smallint_A = Gen.Sql.smallint
                    let! smallmoney_A = Gen.Sql.smallmoney
                    let! text_A = Gen.Sql.text
                    let! time_1_A = Gen.Sql.time 1
                    let! tinyint_A = Gen.Sql.tinyint
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier
                    let! varbinary_42_A = Gen.Sql.varbinary 42
                    let! varchar_42_A = Gen.Sql.varchar 42
                    let! xml_A = Gen.Sql.xml

                    let! bigint_B = Gen.Sql.bigint
                    let! binary_42_B = Gen.Sql.binary 42
                    let! bit_B = Gen.Sql.bit
                    let! char_42_B = Gen.Sql.char 42
                    let! date_B = Gen.Sql.date
                    let! datetime_B = Gen.Sql.datetime
                    let! datetime2_3_B = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5
                    let! float_42_B = Gen.Sql.float 42
                    let! image_B = Gen.Sql.image
                    let! int_B = Gen.Sql.int
                    let! money_B = Gen.Sql.money
                    let! nchar_42_B = Gen.Sql.nchar 42
                    let! ntext_B = Gen.Sql.ntext
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42
                    let! real_B = Gen.Sql.real
                    let! smalldatetime_B = Gen.Sql.smalldatetime
                    let! smallint_B = Gen.Sql.smallint
                    let! smallmoney_B = Gen.Sql.smallmoney
                    let! text_B = Gen.Sql.text
                    let! time_1_B = Gen.Sql.time 1
                    let! tinyint_B = Gen.Sql.tinyint
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier
                    let! varbinary_42_B = Gen.Sql.varbinary 42
                    let! varchar_42_B = Gen.Sql.varchar 42
                    let! xml_B = Gen.Sql.xml

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNonNull_InsertBatch
                            .WithConnection(Config.connStr)
                            .ConfigureBulkCopy(ignore<SqlBulkCopy>) // Verify that the method exists
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNonNull_InsertBatch.args.create (
                                        key_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNonNull_InsertBatch.args.create (
                                        key_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]

                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>

                    let! bigint_A = Gen.Sql.bigint
                    let! binary_42_A = Gen.Sql.binary 42
                    let! bit_A = Gen.Sql.bit
                    let! char_42_A = Gen.Sql.char 42
                    let! date_A = Gen.Sql.date
                    let! datetime_A = Gen.Sql.datetime
                    let! datetime2_3_A = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5
                    let! float_42_A = Gen.Sql.float 42
                    let! image_A = Gen.Sql.image
                    let! int_A = Gen.Sql.int
                    let! money_A = Gen.Sql.money
                    let! nchar_42_A = Gen.Sql.nchar 42
                    let! ntext_A = Gen.Sql.ntext
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42
                    let! real_A = Gen.Sql.real
                    let! smalldatetime_A = Gen.Sql.smalldatetime
                    let! smallint_A = Gen.Sql.smallint
                    let! smallmoney_A = Gen.Sql.smallmoney
                    let! text_A = Gen.Sql.text
                    let! time_1_A = Gen.Sql.time 1
                    let! tinyint_A = Gen.Sql.tinyint
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier
                    let! varbinary_42_A = Gen.Sql.varbinary 42
                    let! varchar_42_A = Gen.Sql.varchar 42
                    let! xml_A = Gen.Sql.xml

                    let! bigint_B = Gen.Sql.bigint
                    let! binary_42_B = Gen.Sql.binary 42
                    let! bit_B = Gen.Sql.bit
                    let! char_42_B = Gen.Sql.char 42
                    let! date_B = Gen.Sql.date
                    let! datetime_B = Gen.Sql.datetime
                    let! datetime2_3_B = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5
                    let! float_42_B = Gen.Sql.float 42
                    let! image_B = Gen.Sql.image
                    let! int_B = Gen.Sql.int
                    let! money_B = Gen.Sql.money
                    let! nchar_42_B = Gen.Sql.nchar 42
                    let! ntext_B = Gen.Sql.ntext
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42
                    let! real_B = Gen.Sql.real
                    let! smalldatetime_B = Gen.Sql.smalldatetime
                    let! smallint_B = Gen.Sql.smallint
                    let! smallmoney_B = Gen.Sql.smallmoney
                    let! text_B = Gen.Sql.text
                    let! time_1_B = Gen.Sql.time 1
                    let! tinyint_B = Gen.Sql.tinyint
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier
                    let! varbinary_42_B = Gen.Sql.varbinary 42
                    let! varchar_42_B = Gen.Sql.varchar 42
                    let! xml_B = Gen.Sql.xml

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNonNull_UpdateBatch
                            .WithConnection(Config.connStr)
                            .ConfigureBulkCopy(ignore<SqlBulkCopy>) // Verify that the method exists
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNonNull_UpdateBatch.args.create (
                                        key_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNonNull_UpdateBatch.args.create (
                                        key_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>
                }


            testCase "Non-null MERGE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key = Gen.Sql.int

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ getRes = None @>

                    let! bigint = Gen.Sql.bigint
                    let! binary_42 = Gen.Sql.binary 42
                    let! bit = Gen.Sql.bit
                    let! char_42 = Gen.Sql.char 42
                    let! date = Gen.Sql.date
                    let! datetime = Gen.Sql.datetime
                    let! datetime2_3 = Gen.Sql.datetime2 3
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5 = Gen.Sql.decimal 10 5
                    let! float_42 = Gen.Sql.float 42
                    let! image = Gen.Sql.image
                    let! int = Gen.Sql.int
                    let! money = Gen.Sql.money
                    let! nchar_42 = Gen.Sql.nchar 42
                    let! ntext = Gen.Sql.ntext
                    let! numeric_8_3 = Gen.Sql.numeric 8 3
                    let! nvarchar_42 = Gen.Sql.nvarchar 42
                    let! real = Gen.Sql.real
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNonNull_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>


                    let! bigint = Gen.Sql.bigint
                    let! binary_42 = Gen.Sql.binary 42
                    let! bit = Gen.Sql.bit
                    let! char_42 = Gen.Sql.char 42
                    let! date = Gen.Sql.date
                    let! datetime = Gen.Sql.datetime
                    let! datetime2_3 = Gen.Sql.datetime2 3
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5 = Gen.Sql.decimal 10 5
                    let! float_42 = Gen.Sql.float 42
                    let! image = Gen.Sql.image
                    let! int = Gen.Sql.int
                    let! money = Gen.Sql.money
                    let! nchar_42 = Gen.Sql.nchar 42
                    let! ntext = Gen.Sql.ntext
                    let! numeric_8_3 = Gen.Sql.numeric 8 3
                    let! nvarchar_42 = Gen.Sql.nvarchar 42
                    let! real = Gen.Sql.real
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNonNull_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>
                }


            testCase "Non-null MERGE batch"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key_A = Gen.Sql.int
                    let! key_B = Gen.Sql.int |> Gen.filter (not << (=) key_A)

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let! bigint_A = Gen.Sql.bigint
                    let! binary_42_A = Gen.Sql.binary 42
                    let! bit_A = Gen.Sql.bit
                    let! char_42_A = Gen.Sql.char 42
                    let! date_A = Gen.Sql.date
                    let! datetime_A = Gen.Sql.datetime
                    let! datetime2_3_A = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5
                    let! float_42_A = Gen.Sql.float 42
                    let! image_A = Gen.Sql.image
                    let! int_A = Gen.Sql.int
                    let! money_A = Gen.Sql.money
                    let! nchar_42_A = Gen.Sql.nchar 42
                    let! ntext_A = Gen.Sql.ntext
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42
                    let! real_A = Gen.Sql.real
                    let! smalldatetime_A = Gen.Sql.smalldatetime
                    let! smallint_A = Gen.Sql.smallint
                    let! smallmoney_A = Gen.Sql.smallmoney
                    let! text_A = Gen.Sql.text
                    let! time_1_A = Gen.Sql.time 1
                    let! tinyint_A = Gen.Sql.tinyint
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier
                    let! varbinary_42_A = Gen.Sql.varbinary 42
                    let! varchar_42_A = Gen.Sql.varchar 42
                    let! xml_A = Gen.Sql.xml

                    let! bigint_B = Gen.Sql.bigint
                    let! binary_42_B = Gen.Sql.binary 42
                    let! bit_B = Gen.Sql.bit
                    let! char_42_B = Gen.Sql.char 42
                    let! date_B = Gen.Sql.date
                    let! datetime_B = Gen.Sql.datetime
                    let! datetime2_3_B = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5
                    let! float_42_B = Gen.Sql.float 42
                    let! image_B = Gen.Sql.image
                    let! int_B = Gen.Sql.int
                    let! money_B = Gen.Sql.money
                    let! nchar_42_B = Gen.Sql.nchar 42
                    let! ntext_B = Gen.Sql.ntext
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42
                    let! real_B = Gen.Sql.real
                    let! smalldatetime_B = Gen.Sql.smalldatetime
                    let! smallint_B = Gen.Sql.smallint
                    let! smallmoney_B = Gen.Sql.smallmoney
                    let! text_B = Gen.Sql.text
                    let! time_1_B = Gen.Sql.time 1
                    let! tinyint_B = Gen.Sql.tinyint
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier
                    let! varbinary_42_B = Gen.Sql.varbinary 42
                    let! varchar_42_B = Gen.Sql.varchar 42
                    let! xml_B = Gen.Sql.xml

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNonNull_MergeBatch
                            .WithConnection(Config.connStr)
                            .ConfigureBulkCopy(ignore<SqlBulkCopy>) // Verify that the method exists
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNonNull_MergeBatch.args.create (
                                        key_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNonNull_MergeBatch.args.create (
                                        key_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>

                    let! bigint_A = Gen.Sql.bigint
                    let! binary_42_A = Gen.Sql.binary 42
                    let! bit_A = Gen.Sql.bit
                    let! char_42_A = Gen.Sql.char 42
                    let! date_A = Gen.Sql.date
                    let! datetime_A = Gen.Sql.datetime
                    let! datetime2_3_A = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5
                    let! float_42_A = Gen.Sql.float 42
                    let! image_A = Gen.Sql.image
                    let! int_A = Gen.Sql.int
                    let! money_A = Gen.Sql.money
                    let! nchar_42_A = Gen.Sql.nchar 42
                    let! ntext_A = Gen.Sql.ntext
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42
                    let! real_A = Gen.Sql.real
                    let! smalldatetime_A = Gen.Sql.smalldatetime
                    let! smallint_A = Gen.Sql.smallint
                    let! smallmoney_A = Gen.Sql.smallmoney
                    let! text_A = Gen.Sql.text
                    let! time_1_A = Gen.Sql.time 1
                    let! tinyint_A = Gen.Sql.tinyint
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier
                    let! varbinary_42_A = Gen.Sql.varbinary 42
                    let! varchar_42_A = Gen.Sql.varchar 42
                    let! xml_A = Gen.Sql.xml

                    let! bigint_B = Gen.Sql.bigint
                    let! binary_42_B = Gen.Sql.binary 42
                    let! bit_B = Gen.Sql.bit
                    let! char_42_B = Gen.Sql.char 42
                    let! date_B = Gen.Sql.date
                    let! datetime_B = Gen.Sql.datetime
                    let! datetime2_3_B = Gen.Sql.datetime2 3
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5
                    let! float_42_B = Gen.Sql.float 42
                    let! image_B = Gen.Sql.image
                    let! int_B = Gen.Sql.int
                    let! money_B = Gen.Sql.money
                    let! nchar_42_B = Gen.Sql.nchar 42
                    let! ntext_B = Gen.Sql.ntext
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42
                    let! real_B = Gen.Sql.real
                    let! smalldatetime_B = Gen.Sql.smalldatetime
                    let! smallint_B = Gen.Sql.smallint
                    let! smallmoney_B = Gen.Sql.smallmoney
                    let! text_B = Gen.Sql.text
                    let! time_1_B = Gen.Sql.time 1
                    let! tinyint_B = Gen.Sql.tinyint
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier
                    let! varbinary_42_B = Gen.Sql.varbinary 42
                    let! varchar_42_B = Gen.Sql.varchar 42
                    let! xml_B = Gen.Sql.xml

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNonNull_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNonNull_MergeBatch.args.create (
                                        key_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNonNull_MergeBatch.args.create (
                                        key_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNonNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>
                }


            testCase "Null INSERT/UPDATE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1 = Gen.Sql.int
                    let! key2 = Gen.Sql.int

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ getRes = None @>

                    let! bigint = Gen.Sql.bigint |> Gen.option
                    let! binary_42 = Gen.Sql.binary 42 |> Gen.option
                    let! bit = Gen.Sql.bit |> Gen.option
                    let! char_42 = Gen.Sql.char 42 |> Gen.option
                    let! date = Gen.Sql.date |> Gen.option
                    let! datetime = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3 = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5 = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42 = Gen.Sql.float 42 |> Gen.option
                    let! image = Gen.Sql.image |> Gen.option
                    let! int = Gen.Sql.int |> Gen.option
                    let! money = Gen.Sql.money |> Gen.option
                    let! nchar_42 = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3 = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42 = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real = Gen.Sql.real |> Gen.option
                    let! smalldatetime = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint = Gen.Sql.smallint |> Gen.option
                    let! smallmoney = Gen.Sql.smallmoney |> Gen.option
                    let! text = Gen.Sql.text |> Gen.option
                    let! time_1 = Gen.Sql.time 1 |> Gen.option
                    let! tinyint = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42 = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42 = Gen.Sql.varchar 42 |> Gen.option
                    let! xml = Gen.Sql.xml |> Gen.option

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNull_Insert
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key1,
                                key2,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>


                    let! bigint = Gen.Sql.bigint |> Gen.option
                    let! binary_42 = Gen.Sql.binary 42 |> Gen.option
                    let! bit = Gen.Sql.bit |> Gen.option
                    let! char_42 = Gen.Sql.char 42 |> Gen.option
                    let! date = Gen.Sql.date |> Gen.option
                    let! datetime = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3 = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5 = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42 = Gen.Sql.float 42 |> Gen.option
                    let! image = Gen.Sql.image |> Gen.option
                    let! int = Gen.Sql.int |> Gen.option
                    let! money = Gen.Sql.money |> Gen.option
                    let! nchar_42 = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3 = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42 = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real = Gen.Sql.real |> Gen.option
                    let! smalldatetime = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint = Gen.Sql.smallint |> Gen.option
                    let! smallmoney = Gen.Sql.smallmoney |> Gen.option
                    let! text = Gen.Sql.text |> Gen.option
                    let! time_1 = Gen.Sql.time 1 |> Gen.option
                    let! tinyint = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42 = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42 = Gen.Sql.varchar 42 |> Gen.option
                    let! xml = Gen.Sql.xml |> Gen.option

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNull_Update
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key1,
                                key2,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>

                    let numDeletedRows =
                        DbGen.Scripts.AllTypesNull_Delete
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .Execute()

                    test <@ numDeletedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ getRes = None @>
                }


            testCase "Null INSERT/UPDATE batch"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1_A, key2_A = Gen.Sql.int |> Gen.tuple
                    let! key1_B, key2_B = Gen.Sql.int |> Gen.tuple |> Gen.filter (not << (=) (key1_A, key2_A))

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let! bigint_A = Gen.Sql.bigint |> Gen.option
                    let! binary_42_A = Gen.Sql.binary 42 |> Gen.option
                    let! bit_A = Gen.Sql.bit |> Gen.option
                    let! char_42_A = Gen.Sql.char 42 |> Gen.option
                    let! date_A = Gen.Sql.date |> Gen.option
                    let! datetime_A = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_A = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_A = Gen.Sql.float 42 |> Gen.option
                    let! image_A = Gen.Sql.image |> Gen.option
                    let! int_A = Gen.Sql.int |> Gen.option
                    let! money_A = Gen.Sql.money |> Gen.option
                    let! nchar_42_A = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_A = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_A = Gen.Sql.real |> Gen.option
                    let! smalldatetime_A = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_A = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_A = Gen.Sql.smallmoney |> Gen.option
                    let! text_A = Gen.Sql.text |> Gen.option
                    let! time_1_A = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_A = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_A = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_A = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_A = Gen.Sql.xml |> Gen.option

                    let! bigint_B = Gen.Sql.bigint |> Gen.option
                    let! binary_42_B = Gen.Sql.binary 42 |> Gen.option
                    let! bit_B = Gen.Sql.bit |> Gen.option
                    let! char_42_B = Gen.Sql.char 42 |> Gen.option
                    let! date_B = Gen.Sql.date |> Gen.option
                    let! datetime_B = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_B = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_B = Gen.Sql.float 42 |> Gen.option
                    let! image_B = Gen.Sql.image |> Gen.option
                    let! int_B = Gen.Sql.int |> Gen.option
                    let! money_B = Gen.Sql.money |> Gen.option
                    let! nchar_42_B = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_B = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_B = Gen.Sql.real |> Gen.option
                    let! smalldatetime_B = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_B = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_B = Gen.Sql.smallmoney |> Gen.option
                    let! text_B = Gen.Sql.text |> Gen.option
                    let! time_1_B = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_B = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_B = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_B = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_B = Gen.Sql.xml |> Gen.option

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNull_InsertBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNull_InsertBatch.args.create (
                                        key1_A,
                                        key2_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNull_InsertBatch.args.create (
                                        key1_B,
                                        key2_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]

                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>

                    let! bigint_A = Gen.Sql.bigint |> Gen.option
                    let! binary_42_A = Gen.Sql.binary 42 |> Gen.option
                    let! bit_A = Gen.Sql.bit |> Gen.option
                    let! char_42_A = Gen.Sql.char 42 |> Gen.option
                    let! date_A = Gen.Sql.date |> Gen.option
                    let! datetime_A = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_A = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_A = Gen.Sql.float 42 |> Gen.option
                    let! image_A = Gen.Sql.image |> Gen.option
                    let! int_A = Gen.Sql.int |> Gen.option
                    let! money_A = Gen.Sql.money |> Gen.option
                    let! nchar_42_A = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_A = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_A = Gen.Sql.real |> Gen.option
                    let! smalldatetime_A = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_A = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_A = Gen.Sql.smallmoney |> Gen.option
                    let! text_A = Gen.Sql.text |> Gen.option
                    let! time_1_A = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_A = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_A = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_A = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_A = Gen.Sql.xml |> Gen.option

                    let! bigint_B = Gen.Sql.bigint |> Gen.option
                    let! binary_42_B = Gen.Sql.binary 42 |> Gen.option
                    let! bit_B = Gen.Sql.bit |> Gen.option
                    let! char_42_B = Gen.Sql.char 42 |> Gen.option
                    let! date_B = Gen.Sql.date |> Gen.option
                    let! datetime_B = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_B = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_B = Gen.Sql.float 42 |> Gen.option
                    let! image_B = Gen.Sql.image |> Gen.option
                    let! int_B = Gen.Sql.int |> Gen.option
                    let! money_B = Gen.Sql.money |> Gen.option
                    let! nchar_42_B = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_B = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_B = Gen.Sql.real |> Gen.option
                    let! smalldatetime_B = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_B = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_B = Gen.Sql.smallmoney |> Gen.option
                    let! text_B = Gen.Sql.text |> Gen.option
                    let! time_1_B = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_B = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_B = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_B = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_B = Gen.Sql.xml |> Gen.option

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNull_UpdateBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNull_UpdateBatch.args.create (
                                        key1_A,
                                        key2_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNull_UpdateBatch.args.create (
                                        key1_B,
                                        key2_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>
                }


            testCase "Null MERGE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1 = Gen.Sql.int
                    let! key2 = Gen.Sql.int

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ getRes = None @>

                    let! bigint = Gen.Sql.bigint |> Gen.option
                    let! binary_42 = Gen.Sql.binary 42 |> Gen.option
                    let! bit = Gen.Sql.bit |> Gen.option
                    let! char_42 = Gen.Sql.char 42 |> Gen.option
                    let! date = Gen.Sql.date |> Gen.option
                    let! datetime = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3 = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5 = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42 = Gen.Sql.float 42 |> Gen.option
                    let! image = Gen.Sql.image |> Gen.option
                    let! int = Gen.Sql.int |> Gen.option
                    let! money = Gen.Sql.money |> Gen.option
                    let! nchar_42 = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3 = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42 = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real = Gen.Sql.real |> Gen.option
                    let! smalldatetime = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint = Gen.Sql.smallint |> Gen.option
                    let! smallmoney = Gen.Sql.smallmoney |> Gen.option
                    let! text = Gen.Sql.text |> Gen.option
                    let! time_1 = Gen.Sql.time 1 |> Gen.option
                    let! tinyint = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42 = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42 = Gen.Sql.varchar 42 |> Gen.option
                    let! xml = Gen.Sql.xml |> Gen.option

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNull_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key1,
                                key2,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>


                    let! bigint = Gen.Sql.bigint |> Gen.option
                    let! binary_42 = Gen.Sql.binary 42 |> Gen.option
                    let! bit = Gen.Sql.bit |> Gen.option
                    let! char_42 = Gen.Sql.char 42 |> Gen.option
                    let! date = Gen.Sql.date |> Gen.option
                    let! datetime = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3 = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5 = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42 = Gen.Sql.float 42 |> Gen.option
                    let! image = Gen.Sql.image |> Gen.option
                    let! int = Gen.Sql.int |> Gen.option
                    let! money = Gen.Sql.money |> Gen.option
                    let! nchar_42 = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3 = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42 = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real = Gen.Sql.real |> Gen.option
                    let! smalldatetime = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint = Gen.Sql.smallint |> Gen.option
                    let! smallmoney = Gen.Sql.smallmoney |> Gen.option
                    let! text = Gen.Sql.text |> Gen.option
                    let! time_1 = Gen.Sql.time 1 |> Gen.option
                    let! tinyint = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42 = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42 = Gen.Sql.varchar 42 |> Gen.option
                    let! xml = Gen.Sql.xml |> Gen.option

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNull_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key1,
                                key2,
                                bigint,
                                binary_42,
                                bit,
                                char_42,
                                date,
                                datetime,
                                datetime2_3,
                                datetimeoffset_1,
                                decimal_10_5,
                                float_42,
                                image,
                                int,
                                money,
                                nchar_42,
                                ntext,
                                numeric_8_3,
                                nvarchar_42,
                                real,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1, key2)
                            .ExecuteSingle()

                    test <@ bigint = getRes.Value.Bigint @>
                    test <@ binary_42 = getRes.Value.Binary @>
                    test <@ bit = getRes.Value.Bit @>
                    test <@ char_42 = getRes.Value.Char @>
                    test <@ date = getRes.Value.Date @>
                    test <@ datetime = getRes.Value.Datetime @>
                    test <@ datetime2_3 = getRes.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = getRes.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = getRes.Value.Decimal @>
                    test <@ float_42 = getRes.Value.Float @>
                    test <@ image = getRes.Value.Image @>
                    test <@ int = getRes.Value.Int @>
                    test <@ money = getRes.Value.Money @>
                    test <@ nchar_42 = getRes.Value.Nchar @>
                    test <@ ntext = getRes.Value.Ntext @>
                    test <@ numeric_8_3 = getRes.Value.Numeric @>
                    test <@ nvarchar_42 = getRes.Value.Nvarchar @>
                    test <@ real = getRes.Value.Real @>
                    test <@ smalldatetime = getRes.Value.Smalldatetime @>
                    test <@ smallint = getRes.Value.Smallint @>
                    test <@ smallmoney = getRes.Value.Smallmoney @>
                    test <@ text = getRes.Value.Text @>
                    test <@ time_1 = getRes.Value.Time @>
                    test <@ tinyint = getRes.Value.Tinyint @>
                    test <@ uniqueidentifier = getRes.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = getRes.Value.Varbinary @>
                    test <@ varchar_42 = getRes.Value.Varchar @>
                    test <@ xml = getRes.Value.Xml @>
                }


            testCase "Null MERGE batch"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1_A, key2_A = Gen.Sql.int |> Gen.tuple
                    let! key1_B, key2_B = Gen.Sql.int |> Gen.tuple |> Gen.filter (not << (=) (key1_A, key2_A))

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let! bigint_A = Gen.Sql.bigint |> Gen.option
                    let! binary_42_A = Gen.Sql.binary 42 |> Gen.option
                    let! bit_A = Gen.Sql.bit |> Gen.option
                    let! char_42_A = Gen.Sql.char 42 |> Gen.option
                    let! date_A = Gen.Sql.date |> Gen.option
                    let! datetime_A = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_A = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_A = Gen.Sql.float 42 |> Gen.option
                    let! image_A = Gen.Sql.image |> Gen.option
                    let! int_A = Gen.Sql.int |> Gen.option
                    let! money_A = Gen.Sql.money |> Gen.option
                    let! nchar_42_A = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_A = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_A = Gen.Sql.real |> Gen.option
                    let! smalldatetime_A = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_A = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_A = Gen.Sql.smallmoney |> Gen.option
                    let! text_A = Gen.Sql.text |> Gen.option
                    let! time_1_A = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_A = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_A = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_A = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_A = Gen.Sql.xml |> Gen.option

                    let! bigint_B = Gen.Sql.bigint |> Gen.option
                    let! binary_42_B = Gen.Sql.binary 42 |> Gen.option
                    let! bit_B = Gen.Sql.bit |> Gen.option
                    let! char_42_B = Gen.Sql.char 42 |> Gen.option
                    let! date_B = Gen.Sql.date |> Gen.option
                    let! datetime_B = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_B = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_B = Gen.Sql.float 42 |> Gen.option
                    let! image_B = Gen.Sql.image |> Gen.option
                    let! int_B = Gen.Sql.int |> Gen.option
                    let! money_B = Gen.Sql.money |> Gen.option
                    let! nchar_42_B = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_B = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_B = Gen.Sql.real |> Gen.option
                    let! smalldatetime_B = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_B = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_B = Gen.Sql.smallmoney |> Gen.option
                    let! text_B = Gen.Sql.text |> Gen.option
                    let! time_1_B = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_B = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_B = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_B = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_B = Gen.Sql.xml |> Gen.option

                    let numInsertedRows =
                        DbGen.Scripts.AllTypesNull_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNull_MergeBatch.args.create (
                                        key1_A,
                                        key2_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNull_MergeBatch.args.create (
                                        key1_B,
                                        key2_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>

                    let! bigint_A = Gen.Sql.bigint |> Gen.option
                    let! binary_42_A = Gen.Sql.binary 42 |> Gen.option
                    let! bit_A = Gen.Sql.bit |> Gen.option
                    let! char_42_A = Gen.Sql.char 42 |> Gen.option
                    let! date_A = Gen.Sql.date |> Gen.option
                    let! datetime_A = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_A = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_A = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_A = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_A = Gen.Sql.float 42 |> Gen.option
                    let! image_A = Gen.Sql.image |> Gen.option
                    let! int_A = Gen.Sql.int |> Gen.option
                    let! money_A = Gen.Sql.money |> Gen.option
                    let! nchar_42_A = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_A = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_A = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_A = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_A = Gen.Sql.real |> Gen.option
                    let! smalldatetime_A = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_A = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_A = Gen.Sql.smallmoney |> Gen.option
                    let! text_A = Gen.Sql.text |> Gen.option
                    let! time_1_A = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_A = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_A = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_A = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_A = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_A = Gen.Sql.xml |> Gen.option

                    let! bigint_B = Gen.Sql.bigint |> Gen.option
                    let! binary_42_B = Gen.Sql.binary 42 |> Gen.option
                    let! bit_B = Gen.Sql.bit |> Gen.option
                    let! char_42_B = Gen.Sql.char 42 |> Gen.option
                    let! date_B = Gen.Sql.date |> Gen.option
                    let! datetime_B = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3_B = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1_B = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5_B = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42_B = Gen.Sql.float 42 |> Gen.option
                    let! image_B = Gen.Sql.image |> Gen.option
                    let! int_B = Gen.Sql.int |> Gen.option
                    let! money_B = Gen.Sql.money |> Gen.option
                    let! nchar_42_B = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext_B = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3_B = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42_B = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real_B = Gen.Sql.real |> Gen.option
                    let! smalldatetime_B = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint_B = Gen.Sql.smallint |> Gen.option
                    let! smallmoney_B = Gen.Sql.smallmoney |> Gen.option
                    let! text_B = Gen.Sql.text |> Gen.option
                    let! time_1_B = Gen.Sql.time 1 |> Gen.option
                    let! tinyint_B = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier_B = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42_B = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42_B = Gen.Sql.varchar 42 |> Gen.option
                    let! xml_B = Gen.Sql.xml |> Gen.option

                    let numUpdatedRows =
                        DbGen.Scripts.AllTypesNull_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.AllTypesNull_MergeBatch.args.create (
                                        key1_A,
                                        key2_A,
                                        bigint_A,
                                        binary_42_A,
                                        bit_A,
                                        char_42_A,
                                        date_A,
                                        datetime_A,
                                        datetime2_3_A,
                                        datetimeoffset_1_A,
                                        decimal_10_5_A,
                                        float_42_A,
                                        image_A,
                                        int_A,
                                        money_A,
                                        nchar_42_A,
                                        ntext_A,
                                        numeric_8_3_A,
                                        nvarchar_42_A,
                                        real_A,
                                        smalldatetime_A,
                                        smallint_A,
                                        smallmoney_A,
                                        text_A,
                                        time_1_A,
                                        tinyint_A,
                                        uniqueidentifier_A,
                                        varbinary_42_A,
                                        varchar_42_A,
                                        xml_A
                                    )

                                    DbGen.Scripts.AllTypesNull_MergeBatch.args.create (
                                        key1_B,
                                        key2_B,
                                        bigint_B,
                                        binary_42_B,
                                        bit_B,
                                        char_42_B,
                                        date_B,
                                        datetime_B,
                                        datetime2_3_B,
                                        datetimeoffset_1_B,
                                        decimal_10_5_B,
                                        float_42_B,
                                        image_B,
                                        int_B,
                                        money_B,
                                        nchar_42_B,
                                        ntext_B,
                                        numeric_8_3_B,
                                        nvarchar_42_B,
                                        real_B,
                                        smalldatetime_B,
                                        smallint_B,
                                        smallmoney_B,
                                        text_B,
                                        time_1_B,
                                        tinyint_B,
                                        uniqueidentifier_B,
                                        varbinary_42_B,
                                        varchar_42_B,
                                        xml_B
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_A, key2_A)
                            .ExecuteSingle()

                    test <@ bigint_A = getRes_A.Value.Bigint @>
                    test <@ binary_42_A = getRes_A.Value.Binary @>
                    test <@ bit_A = getRes_A.Value.Bit @>
                    test <@ char_42_A = getRes_A.Value.Char @>
                    test <@ date_A = getRes_A.Value.Date @>
                    test <@ datetime_A = getRes_A.Value.Datetime @>
                    test <@ datetime2_3_A = getRes_A.Value.Datetime2 @>
                    test <@ datetimeoffset_1_A = getRes_A.Value.Datetimeoffset @>
                    test <@ decimal_10_5_A = getRes_A.Value.Decimal @>
                    test <@ float_42_A = getRes_A.Value.Float @>
                    test <@ image_A = getRes_A.Value.Image @>
                    test <@ int_A = getRes_A.Value.Int @>
                    test <@ money_A = getRes_A.Value.Money @>
                    test <@ nchar_42_A = getRes_A.Value.Nchar @>
                    test <@ ntext_A = getRes_A.Value.Ntext @>
                    test <@ numeric_8_3_A = getRes_A.Value.Numeric @>
                    test <@ nvarchar_42_A = getRes_A.Value.Nvarchar @>
                    test <@ real_A = getRes_A.Value.Real @>
                    test <@ smalldatetime_A = getRes_A.Value.Smalldatetime @>
                    test <@ smallint_A = getRes_A.Value.Smallint @>
                    test <@ smallmoney_A = getRes_A.Value.Smallmoney @>
                    test <@ text_A = getRes_A.Value.Text @>
                    test <@ time_1_A = getRes_A.Value.Time @>
                    test <@ tinyint_A = getRes_A.Value.Tinyint @>
                    test <@ uniqueidentifier_A = getRes_A.Value.Uniqueidentifier @>
                    test <@ varbinary_42_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_42_A = getRes_A.Value.Varchar @>
                    test <@ xml_A = getRes_A.Value.Xml @>

                    let getRes_B =
                        DbGen.Scripts.AllTypesNull_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1_B, key2_B)
                            .ExecuteSingle()

                    test <@ bigint_B = getRes_B.Value.Bigint @>
                    test <@ binary_42_B = getRes_B.Value.Binary @>
                    test <@ bit_B = getRes_B.Value.Bit @>
                    test <@ char_42_B = getRes_B.Value.Char @>
                    test <@ date_B = getRes_B.Value.Date @>
                    test <@ datetime_B = getRes_B.Value.Datetime @>
                    test <@ datetime2_3_B = getRes_B.Value.Datetime2 @>
                    test <@ datetimeoffset_1_B = getRes_B.Value.Datetimeoffset @>
                    test <@ decimal_10_5_B = getRes_B.Value.Decimal @>
                    test <@ float_42_B = getRes_B.Value.Float @>
                    test <@ image_B = getRes_B.Value.Image @>
                    test <@ int_B = getRes_B.Value.Int @>
                    test <@ money_B = getRes_B.Value.Money @>
                    test <@ nchar_42_B = getRes_B.Value.Nchar @>
                    test <@ ntext_B = getRes_B.Value.Ntext @>
                    test <@ numeric_8_3_B = getRes_B.Value.Numeric @>
                    test <@ nvarchar_42_B = getRes_B.Value.Nvarchar @>
                    test <@ real_B = getRes_B.Value.Real @>
                    test <@ smalldatetime_B = getRes_B.Value.Smalldatetime @>
                    test <@ smallint_B = getRes_B.Value.Smallint @>
                    test <@ smallmoney_B = getRes_B.Value.Smallmoney @>
                    test <@ text_B = getRes_B.Value.Text @>
                    test <@ time_1_B = getRes_B.Value.Time @>
                    test <@ tinyint_B = getRes_B.Value.Tinyint @>
                    test <@ uniqueidentifier_B = getRes_B.Value.Uniqueidentifier @>
                    test <@ varbinary_42_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_42_B = getRes_B.Value.Varchar @>
                    test <@ xml_B = getRes_B.Value.Xml @>
                }


            testCase "GetAll"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let getRes = DbGen.Scripts.AllTypesNull_All.WithConnection(Config.connStr).Execute()

                    test <@ Seq.isEmpty getRes @>

                    let! key1_1, key2_1 = Gen.Sql.int |> Gen.tuple
                    let! key1_2, key2_2 = Gen.Sql.int |> Gen.tuple |> Gen.filter (not << (=) (key1_1, key2_1))

                    let! bigint = Gen.Sql.bigint |> Gen.option
                    let! binary_42 = Gen.Sql.binary 42 |> Gen.option
                    let! bit = Gen.Sql.bit |> Gen.option
                    let! char_42 = Gen.Sql.char 42 |> Gen.option
                    let! date = Gen.Sql.date |> Gen.option
                    let! datetime = Gen.Sql.datetime |> Gen.option
                    let! datetime2_3 = Gen.Sql.datetime2 3 |> Gen.option
                    let! datetimeoffset_1 = Gen.Sql.datetimeoffset 1 |> Gen.option
                    let! decimal_10_5 = Gen.Sql.decimal 10 5 |> Gen.option
                    let! float_42 = Gen.Sql.float 42 |> Gen.option
                    let! image = Gen.Sql.image |> Gen.option
                    let! int = Gen.Sql.int |> Gen.option
                    let! money = Gen.Sql.money |> Gen.option
                    let! nchar_42 = Gen.Sql.nchar 42 |> Gen.option
                    let! ntext = Gen.Sql.ntext |> Gen.option
                    let! numeric_8_3 = Gen.Sql.numeric 8 3 |> Gen.option
                    let! nvarchar_42 = Gen.Sql.nvarchar 42 |> Gen.option
                    let! real = Gen.Sql.real |> Gen.option
                    let! smalldatetime = Gen.Sql.smalldatetime |> Gen.option
                    let! smallint = Gen.Sql.smallint |> Gen.option
                    let! smallmoney = Gen.Sql.smallmoney |> Gen.option
                    let! text = Gen.Sql.text |> Gen.option
                    let! time_1 = Gen.Sql.time 1 |> Gen.option
                    let! tinyint = Gen.Sql.tinyint |> Gen.option
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier |> Gen.option
                    let! varbinary_42 = Gen.Sql.varbinary 42 |> Gen.option
                    let! varchar_42 = Gen.Sql.varchar 42 |> Gen.option
                    let! xml = Gen.Sql.xml |> Gen.option

                    for i in [ 0; 1 ] do
                        let numInsertedRows =
                            DbGen.Scripts.AllTypesNull_Insert
                                .WithConnection(Config.connStr)
                                .WithParameters(
                                    (if i = 0 then key1_1 else key1_2),
                                    (if i = 0 then key2_1 else key2_2),
                                    bigint,
                                    binary_42,
                                    bit,
                                    char_42,
                                    date,
                                    datetime,
                                    datetime2_3,
                                    datetimeoffset_1,
                                    decimal_10_5,
                                    float_42,
                                    image,
                                    int,
                                    money,
                                    nchar_42,
                                    ntext,
                                    numeric_8_3,
                                    nvarchar_42,
                                    real,
                                    smalldatetime,
                                    smallint,
                                    smallmoney,
                                    text,
                                    time_1,
                                    tinyint,
                                    uniqueidentifier,
                                    varbinary_42,
                                    varchar_42,
                                    xml
                                )
                                .Execute()

                        test <@ numInsertedRows = 1 @>

                    let getRes = DbGen.Scripts.AllTypesNull_All.WithConnection(Config.connStr).Execute()

                    for i in [ 0; 1 ] do
                        test <@ bigint = getRes[i].Bigint @>
                        test <@ binary_42 = getRes[i].Binary @>
                        test <@ bit = getRes[i].Bit @>
                        test <@ char_42 = getRes[i].Char @>
                        test <@ date = getRes[i].Date @>
                        test <@ datetime = getRes[i].Datetime @>
                        test <@ datetime2_3 = getRes[i].Datetime2 @>
                        test <@ datetimeoffset_1 = getRes[i].Datetimeoffset @>
                        test <@ decimal_10_5 = getRes[i].Decimal @>
                        test <@ float_42 = getRes[i].Float @>
                        test <@ image = getRes[i].Image @>
                        test <@ int = getRes[i].Int @>
                        test <@ money = getRes[i].Money @>
                        test <@ nchar_42 = getRes[i].Nchar @>
                        test <@ ntext = getRes[i].Ntext @>
                        test <@ numeric_8_3 = getRes[i].Numeric @>
                        test <@ nvarchar_42 = getRes[i].Nvarchar @>
                        test <@ real = getRes[i].Real @>
                        test <@ smalldatetime = getRes[i].Smalldatetime @>
                        test <@ smallint = getRes[i].Smallint @>
                        test <@ smallmoney = getRes[i].Smallmoney @>
                        test <@ text = getRes[i].Text @>
                        test <@ time_1 = getRes[i].Time @>
                        test <@ tinyint = getRes[i].Tinyint @>
                        test <@ uniqueidentifier = getRes[i].Uniqueidentifier @>
                        test <@ varbinary_42 = getRes[i].Varbinary @>
                        test <@ varchar_42 = getRes[i].Varchar @>
                        test <@ xml = getRes[i].Xml @>
                }


            testCase "Length types"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key_A = Gen.Sql.int
                    let! key_B = Gen.Sql.int |> Gen.filter (not << (=) key_A)
                    let! key_C = Gen.Sql.int |> Gen.filter (not << (=) key_A) |> Gen.filter (not << (=) key_B)

                    let getRes_A =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let getRes_C =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ getRes_C = None @>

                    let! binary_3_A = Gen.Sql.binary 3
                    let! char_3_A = Gen.Sql.char 3
                    let! nchar_3_A = Gen.Sql.nchar 3
                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numInsertedRows =
                        DbGen.Scripts.LengthTypes_Insert
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key_A,
                                binary_3_A,
                                char_3_A,
                                nchar_3_A,
                                nvarchar_3_A,
                                varbinary_3_A,
                                varchar_3_A
                            )
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let! binary_3_B = Gen.Sql.binary 3
                    let! char_3_B = Gen.Sql.char 3
                    let! nchar_3_B = Gen.Sql.nchar 3
                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! binary_3_C = Gen.Sql.binary 3
                    let! char_3_C = Gen.Sql.char 3
                    let! nchar_3_C = Gen.Sql.nchar 3
                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numInsertedRows =
                        DbGen.Scripts.LengthTypes_InsertBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.LengthTypes_InsertBatch.args.create (
                                        key_B,
                                        binary_3_B,
                                        char_3_B,
                                        nchar_3_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.LengthTypes_InsertBatch.args.create (
                                        key_C,
                                        binary_3_C,
                                        char_3_C,
                                        nchar_3_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ binary_3_A = getRes_A.Value.Binary @>
                    test <@ char_3_A = getRes_A.Value.Char @>
                    test <@ nchar_3_A = getRes_A.Value.Nchar @>
                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ binary_3_B = getRes_B.Value.Binary @>
                    test <@ char_3_B = getRes_B.Value.Char @>
                    test <@ nchar_3_B = getRes_B.Value.Nchar @>
                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ binary_3_C = getRes_C.Value.Binary @>
                    test <@ char_3_C = getRes_C.Value.Char @>
                    test <@ nchar_3_C = getRes_C.Value.Nchar @>
                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let! binary_3_A = Gen.Sql.binary 3
                    let! char_3_A = Gen.Sql.char 3
                    let! nchar_3_A = Gen.Sql.nchar 3
                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.LengthTypes_Update
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key_A,
                                binary_3_A,
                                char_3_A,
                                nchar_3_A,
                                nvarchar_3_A,
                                varbinary_3_A,
                                varchar_3_A
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let! binary_3_B = Gen.Sql.binary 3
                    let! char_3_B = Gen.Sql.char 3
                    let! nchar_3_B = Gen.Sql.nchar 3
                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! binary_3_C = Gen.Sql.binary 3
                    let! char_3_C = Gen.Sql.char 3
                    let! nchar_3_C = Gen.Sql.nchar 3
                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.LengthTypes_UpdateBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.LengthTypes_UpdateBatch.args.create (
                                        key_B,
                                        binary_3_B,
                                        char_3_B,
                                        nchar_3_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.LengthTypes_UpdateBatch.args.create (
                                        key_C,
                                        binary_3_C,
                                        char_3_C,
                                        nchar_3_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]

                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ binary_3_A = getRes_A.Value.Binary @>
                    test <@ char_3_A = getRes_A.Value.Char @>
                    test <@ nchar_3_A = getRes_A.Value.Nchar @>
                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ binary_3_B = getRes_B.Value.Binary @>
                    test <@ char_3_B = getRes_B.Value.Char @>
                    test <@ nchar_3_B = getRes_B.Value.Nchar @>
                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ binary_3_C = getRes_C.Value.Binary @>
                    test <@ char_3_C = getRes_C.Value.Char @>
                    test <@ nchar_3_C = getRes_C.Value.Nchar @>
                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let! binary_3_A = Gen.Sql.binary 3
                    let! char_3_A = Gen.Sql.char 3
                    let! nchar_3_A = Gen.Sql.nchar 3
                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.LengthTypes_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                key_A,
                                binary_3_A,
                                char_3_A,
                                nchar_3_A,
                                nvarchar_3_A,
                                varbinary_3_A,
                                varchar_3_A
                            )
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let! binary_3_B = Gen.Sql.binary 3
                    let! char_3_B = Gen.Sql.char 3
                    let! nchar_3_B = Gen.Sql.nchar 3
                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! binary_3_C = Gen.Sql.binary 3
                    let! char_3_C = Gen.Sql.char 3
                    let! nchar_3_C = Gen.Sql.nchar 3
                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.LengthTypes_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.LengthTypes_MergeBatch.args.create (
                                        key_B,
                                        binary_3_B,
                                        char_3_B,
                                        nchar_3_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.LengthTypes_MergeBatch.args.create (
                                        key_C,
                                        binary_3_C,
                                        char_3_C,
                                        nchar_3_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]

                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ binary_3_A = getRes_A.Value.Binary @>
                    test <@ char_3_A = getRes_A.Value.Char @>
                    test <@ nchar_3_A = getRes_A.Value.Nchar @>
                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ binary_3_B = getRes_B.Value.Binary @>
                    test <@ char_3_B = getRes_B.Value.Char @>
                    test <@ nchar_3_B = getRes_B.Value.Nchar @>
                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ binary_3_C = getRes_C.Value.Binary @>
                    test <@ char_3_C = getRes_C.Value.Char @>
                    test <@ nchar_3_C = getRes_C.Value.Nchar @>
                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let numDeletedRows =
                        DbGen.Scripts.LengthTypes_Delete
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .Execute()

                    test <@ numDeletedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.LengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes = None @>
                }


            testCase "Max length types"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key_A = Gen.Sql.int
                    let! key_B = Gen.Sql.int |> Gen.filter (not << (=) key_A)
                    let! key_C = Gen.Sql.int |> Gen.filter (not << (=) key_A) |> Gen.filter (not << (=) key_B)

                    let getRes_A =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes_A = None @>

                    let getRes_B =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ getRes_B = None @>

                    let getRes_C =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ getRes_C = None @>

                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numInsertedRows =
                        DbGen.Scripts.MaxLengthTypes_Insert
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A, nvarchar_3_A, varbinary_3_A, varchar_3_A)
                            .Execute()

                    test <@ numInsertedRows = 1 @>

                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numInsertedRows =
                        DbGen.Scripts.MaxLengthTypes_InsertBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.MaxLengthTypes_InsertBatch.args.create (
                                        key_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.MaxLengthTypes_InsertBatch.args.create (
                                        key_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numInsertedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.MaxLengthTypes_Update
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A, nvarchar_3_A, varbinary_3_A, varchar_3_A)
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.MaxLengthTypes_UpdateBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.MaxLengthTypes_UpdateBatch.args.create (
                                        key_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.MaxLengthTypes_UpdateBatch.args.create (
                                        key_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let! nvarchar_3_A = Gen.Sql.nvarchar 3
                    let! varbinary_3_A = Gen.Sql.varbinary 3
                    let! varchar_3_A = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.MaxLengthTypes_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A, nvarchar_3_A, varbinary_3_A, varchar_3_A)
                            .Execute()

                    test <@ numUpdatedRows = 1 @>

                    let! nvarchar_3_B = Gen.Sql.nvarchar 3
                    let! varbinary_3_B = Gen.Sql.varbinary 3
                    let! varchar_3_B = Gen.Sql.varchar 3

                    let! nvarchar_3_C = Gen.Sql.nvarchar 3
                    let! varbinary_3_C = Gen.Sql.varbinary 3
                    let! varchar_3_C = Gen.Sql.varchar 3

                    let numUpdatedRows =
                        DbGen.Scripts.MaxLengthTypes_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.MaxLengthTypes_MergeBatch.args.create (
                                        key_B,
                                        nvarchar_3_B,
                                        varbinary_3_B,
                                        varchar_3_B
                                    )

                                    DbGen.Scripts.MaxLengthTypes_MergeBatch.args.create (
                                        key_C,
                                        nvarchar_3_C,
                                        varbinary_3_C,
                                        varchar_3_C
                                    )
                                ]
                            )
                            .Execute()

                    test <@ numUpdatedRows = 2 @>

                    let getRes_A =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ nvarchar_3_A = getRes_A.Value.Nvarchar @>
                    test <@ varbinary_3_A = getRes_A.Value.Varbinary @>
                    test <@ varchar_3_A = getRes_A.Value.Varchar @>

                    let getRes_B =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_B)
                            .ExecuteSingle()

                    test <@ nvarchar_3_B = getRes_B.Value.Nvarchar @>
                    test <@ varbinary_3_B = getRes_B.Value.Varbinary @>
                    test <@ varchar_3_B = getRes_B.Value.Varchar @>

                    let getRes_C =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_C)
                            .ExecuteSingle()

                    test <@ nvarchar_3_C = getRes_C.Value.Nvarchar @>
                    test <@ varbinary_3_C = getRes_C.Value.Varbinary @>
                    test <@ varchar_3_C = getRes_C.Value.Varchar @>

                    let numDeletedRows =
                        DbGen.Scripts.MaxLengthTypes_Delete
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .Execute()

                    test <@ numDeletedRows = 1 @>

                    let getRes =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key_A)
                            .ExecuteSingle()

                    test <@ getRes = None @>
                }


        ]


        testList "Misc" [


            testCase "Updates/deletes only the specified row"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1 = Gen.Sql.int
                    let! key2 = Gen.Sql.int |> Gen.filter (not << (=) key1)

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3
                    let! nvarchar_3_2 = Gen.Sql.nvarchar 3
                    let! varbinary_3_2 = Gen.Sql.varbinary 3
                    let! varchar_3_2 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(key1, nvarchar_3_1, varbinary_3_1, varchar_3_1)
                        .Execute()
                    |> ignore<int>

                    DbGen.Scripts.MaxLengthTypes_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(key2, nvarchar_3_2, varbinary_3_2, varchar_3_2)
                        .Execute()
                    |> ignore<int>

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_Update
                        .WithConnection(Config.connStr)
                        .WithParameters(key1, nvarchar_3_1, varbinary_3_1, varchar_3_1)
                        .Execute()
                    |> ignore<int>

                    let getRes1 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1)
                            .ExecuteSingle()

                    let getRes2 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key2)
                            .ExecuteSingle()

                    test <@ nvarchar_3_1 = getRes1.Value.Nvarchar @>
                    test <@ varbinary_3_1 = getRes1.Value.Varbinary @>
                    test <@ varchar_3_1 = getRes1.Value.Varchar @>

                    test <@ nvarchar_3_2 = getRes2.Value.Nvarchar @>
                    test <@ varbinary_3_2 = getRes2.Value.Varbinary @>
                    test <@ varchar_3_2 = getRes2.Value.Varchar @>

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_Merge
                        .WithConnection(Config.connStr)
                        .WithParameters(key1, nvarchar_3_1, varbinary_3_1, varchar_3_1)
                        .Execute()
                    |> ignore<int>

                    let getRes1 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1)
                            .ExecuteSingle()

                    let getRes2 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key2)
                            .ExecuteSingle()

                    test <@ nvarchar_3_1 = getRes1.Value.Nvarchar @>
                    test <@ varbinary_3_1 = getRes1.Value.Varbinary @>
                    test <@ varchar_3_1 = getRes1.Value.Varchar @>

                    test <@ nvarchar_3_2 = getRes2.Value.Nvarchar @>
                    test <@ varbinary_3_2 = getRes2.Value.Varbinary @>
                    test <@ varchar_3_2 = getRes2.Value.Varchar @>

                    DbGen.Scripts.MaxLengthTypes_Delete
                        .WithConnection(Config.connStr)
                        .WithParameters(key1)
                        .Execute()
                    |> ignore<int>

                    let getRes1 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1)
                            .ExecuteSingle()

                    let getRes2 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key2)
                            .ExecuteSingle()

                    test <@ getRes1 = None @>
                    test <@ getRes2.IsSome = true @>
                }


            testCase "Batch updates only the specified rows"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let! key1 = Gen.Sql.int
                    let! key2 = Gen.Sql.int |> Gen.filter (not << (=) key1)

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3
                    let! nvarchar_3_2 = Gen.Sql.nvarchar 3
                    let! varbinary_3_2 = Gen.Sql.varbinary 3
                    let! varchar_3_2 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(key1, nvarchar_3_1, varbinary_3_1, varchar_3_1)
                        .Execute()
                    |> ignore<int>

                    DbGen.Scripts.MaxLengthTypes_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(key2, nvarchar_3_2, varbinary_3_2, varchar_3_2)
                        .Execute()
                    |> ignore<int>

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_UpdateBatch
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.MaxLengthTypes_UpdateBatch.args.create (
                                    key1,
                                    nvarchar_3_1,
                                    varbinary_3_1,
                                    varchar_3_1
                                )
                            ]
                        )
                        .Execute()
                    |> ignore<int>

                    let getRes1 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1)
                            .ExecuteSingle()

                    let getRes2 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key2)
                            .ExecuteSingle()

                    test <@ nvarchar_3_1 = getRes1.Value.Nvarchar @>
                    test <@ varbinary_3_1 = getRes1.Value.Varbinary @>
                    test <@ varchar_3_1 = getRes1.Value.Varchar @>

                    test <@ nvarchar_3_2 = getRes2.Value.Nvarchar @>
                    test <@ varbinary_3_2 = getRes2.Value.Varbinary @>
                    test <@ varchar_3_2 = getRes2.Value.Varchar @>

                    let! nvarchar_3_1 = Gen.Sql.nvarchar 3
                    let! varbinary_3_1 = Gen.Sql.varbinary 3
                    let! varchar_3_1 = Gen.Sql.varchar 3

                    DbGen.Scripts.MaxLengthTypes_MergeBatch
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.MaxLengthTypes_MergeBatch.args.create (
                                    key1,
                                    nvarchar_3_1,
                                    varbinary_3_1,
                                    varchar_3_1
                                )
                            ]
                        )
                        .Execute()
                    |> ignore<int>

                    let getRes1 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key1)
                            .ExecuteSingle()

                    let getRes2 =
                        DbGen.Scripts.MaxLengthTypes_ById
                            .WithConnection(Config.connStr)
                            .WithParameters(key2)
                            .ExecuteSingle()

                    test <@ nvarchar_3_1 = getRes1.Value.Nvarchar @>
                    test <@ varbinary_3_1 = getRes1.Value.Varbinary @>
                    test <@ varchar_3_1 = getRes1.Value.Varchar @>

                    test <@ nvarchar_3_2 = getRes2.Value.Nvarchar @>
                    test <@ varbinary_3_2 = getRes2.Value.Varbinary @>
                    test <@ varchar_3_2 = getRes2.Value.Varchar @>
                }


            testCase "Identity, output columns and other column config with INSERT/UPDATE/DELETE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let insertRes =
                        DbGen.Scripts.TableWithIdentityCol_Insert
                            .WithConnection(Config.connStr)
                            .WithParameters(foo = 0L, bar = None)
                            .ExecuteSingle()

                    let key = insertRes.Value.Id
                    ignore<int64> insertRes.Value.Foo

                    ignore<{| Id: int; Foo: int64 |}> {|
                        insertRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    let! bigint = Gen.Sql.bigint
                    let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

                    let updateRes =
                        DbGen.Scripts.TableWithIdentityCol_Update
                            .WithConnection(Config.connStr)
                            .WithParameters(key, foo = bigint, bar = datetimeoffset)
                            .ExecuteSingle()

                    ignore<{| Id: int; Foo: int64 |}> {|
                        updateRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    test <@ updateRes.Value.Foo = bigint @>

                    let deleteRes =
                        DbGen.Scripts.TableWithIdentityCol_Delete
                            .WithConnection(Config.connStr)
                            .WithParameters(key)
                            .ExecuteSingle()

                    test <@ deleteRes.Value.Foo = bigint @>
                    test <@ deleteRes.Value.BAR = datetimeoffset @>

                    // Compile-time test; should have expected result set since a column is skipped
                    DbGen.Scripts.TableWithIdentityCol_ById
                        .WithConnection(Config.connStr)
                        .WithParameters(key)
                        .ExecuteSingle()
                    |> Option.map (fun x -> {| x with Id = x.Id |})
                    |> ignore<{| Id: int; Foo: int64 |} option>
                }


            testCase "Identity, output columns and other column config with batch INSERT/UPDATE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let insertRes =
                        DbGen.Scripts.TableWithIdentityCol_InsertBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.TableWithIdentityCol_InsertBatch.args.create (Foo = 0L, BAR = None)
                                ]
                            )
                            .ExecuteSingle()

                    let key = insertRes.Value.Id
                    ignore<int64> insertRes.Value.Foo

                    ignore<{| Id: int; Foo: int64 |}> {|
                        insertRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    let! bigint = Gen.Sql.bigint
                    let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

                    let updateRes =
                        DbGen.Scripts.TableWithIdentityCol_UpdateBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.TableWithIdentityCol_UpdateBatch.args.create (
                                        Id = key,
                                        Foo = bigint,
                                        BAR = datetimeoffset
                                    )
                                ]
                            )
                            .ExecuteSingle()

                    ignore<{| Id: int; Foo: int64 |}> {|
                        updateRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    test <@ updateRes.Value.Foo = bigint @>
                }


            testCase "Identity, output columns and other column config with MERGE"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let tempKey = -1

                    let insertRes =
                        DbGen.Scripts.TableWithIdentityCol_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(id = tempKey, foo = 0L, bar = None)
                            .ExecuteSingle()

                    let key = insertRes.Value.Id
                    test <@ key <> tempKey @>

                    ignore<{| Id: int; Foo: int64 |}> {|
                        insertRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    ignore<int64> insertRes.Value.Foo

                    let! bigint = Gen.Sql.bigint
                    let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

                    let updateRes =
                        DbGen.Scripts.TableWithIdentityCol_Merge
                            .WithConnection(Config.connStr)
                            .WithParameters(key, foo = bigint, bar = datetimeoffset)
                            .ExecuteSingle()

                    ignore<{| Id: int; Foo: int64 |}> {|
                        updateRes.Value with
                            Id = updateRes.Value.Id
                    |}

                    test <@ updateRes.Value.Foo = bigint @>
                }


            testCase "Identity, output columns and other column config with MERGE batch"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let tempKey = -1

                    let insertRes =
                        DbGen.Scripts.TableWithIdentityCol_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.TableWithIdentityCol_MergeBatch.args.create (
                                        Id = tempKey,
                                        Foo = 0L,
                                        BAR = None
                                    )
                                ]
                            )
                            .ExecuteSingle()

                    let key = insertRes.Value.Id
                    test <@ key <> tempKey @>

                    ignore<{| Id: int; Foo: int64 |}> {|
                        insertRes.Value with
                            Id = insertRes.Value.Id
                    |}

                    ignore<int64> insertRes.Value.Foo

                    let! bigint = Gen.Sql.bigint
                    let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

                    let updateRes =
                        DbGen.Scripts.TableWithIdentityCol_MergeBatch
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.TableWithIdentityCol_MergeBatch.args.create (
                                        key,
                                        Foo = bigint,
                                        BAR = datetimeoffset
                                    )
                                ]
                            )
                            .ExecuteSingle()

                    ignore<{| Id: int; Foo: int64 |}> {|
                        updateRes.Value with
                            Id = updateRes.Value.Id
                    |}

                    test <@ updateRes.Value.Foo = bigint @>
                }


            testCase "MERGE WITH (HOLDLOCK) works"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let tempKey = -1

                    let insertedRows =
                        DbGen.Scripts.TableWithIdentityCol_MergeWithHoldlock
                            .WithConnection(Config.connStr)
                            .WithParameters(id = tempKey, foo = 0L, bAR = None)
                            .Execute()

                    test <@ insertedRows = 1 @>
                }


            testCase "Batch MERGE WITH (HOLDLOCK) works"
            <| fun () ->
                Property.check
                <| property {
                    clearTableScriptTables ()

                    let tempKey = -1

                    let insertedRows =
                        DbGen.Scripts.TableWithIdentityCol_MergeBatchWithHoldlock
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.Scripts.TableWithIdentityCol_MergeBatchWithHoldlock.args.create (
                                        Id = tempKey,
                                        Foo = 0L,
                                        BAR = None
                                    )
                                ]
                            )
                            .Execute()

                    test <@ insertedRows = 1 @>
                }


            testCase "GetById with selectColumns"
            <| fun () ->
                clearTableScriptTables ()

                let insertRes =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 123L, bar = None)
                        .ExecuteSingle()

                let getRes =
                    DbGen.Scripts.TableWithIdentityCol_ById_WithSelectColumns
                        .WithConnection(Config.connStr)
                        .WithParameters(insertRes.Value.Id)
                        .ExecuteSingle()

                ignore<{| Id: int; Foo: int64 |}> {|
                    getRes.Value with
                        Id = getRes.Value.Id
                |}

                test <@ getRes.Value.Id = insertRes.Value.Id @>
                test <@ getRes.Value.Foo = insertRes.Value.Foo @>


            testCase "GetByIdBatch"
            <| fun () ->
                clearTableScriptTables ()

                let insertRes1 =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 0L, bar = None)
                        .ExecuteSingle()

                let insertRes2 =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 1L, bar = None)
                        .ExecuteSingle()

                let key1 = insertRes1.Value.Id
                let key2 = insertRes2.Value.Id

                let getRes =
                    DbGen.Scripts.TableWithIdentityCol_ByIds
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.TableTypes.dbo.SingleColNonNull.create key1
                                DbGen.TableTypes.dbo.SingleColNonNull.create key2
                            ]
                        )
                        .Execute()


                test <@ getRes.Count = 2 @>
                let resForKey1 = getRes |> Seq.find (fun x -> x.Id = key1)
                let resForKey2 = getRes |> Seq.find (fun x -> x.Id = key2)

                test <@ resForKey1.Foo = 0L @>
                test <@ resForKey1.BAR = None @>
                test <@ resForKey2.Foo = 1L @>
                test <@ resForKey2.BAR = None @>


            testCase "GetByColumns"
            <| fun () ->
                clearTableScriptTables ()

                let insertRes =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 123L, bar = None)
                        .ExecuteSingle()

                let getRes =
                    DbGen.Scripts.TableWithIdentityCol_ByFoo
                        .WithConnection(Config.connStr)
                        .WithParameters(123L)
                        .ExecuteSingle()

                test <@ getRes.Value.Id = insertRes.Value.Id @>
                test <@ getRes.Value.Foo = insertRes.Value.Foo @>


            testCase "GetByColumnsBatch"
            <| fun () ->
                clearTableScriptTables ()

                let insertRes1 =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 123L, bar = None)
                        .ExecuteSingle()

                let insertRes2 =
                    DbGen.Scripts.TableWithIdentityCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(foo = 456L, bar = None)
                        .ExecuteSingle()

                let key1 = insertRes1.Value.Id
                let key2 = insertRes2.Value.Id

                let getRes =
                    DbGen.Scripts.TableWithIdentityCol_ByIdAndFoos
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.TableTypes.dbo.FilterForTableWithIdentityCol.create insertRes1.Value
                                DbGen.TableTypes.dbo.FilterForTableWithIdentityCol.create insertRes2.Value
                            ]
                        )
                        .Execute()

                test <@ getRes.Count = 2 @>
                let resForKey1 = getRes |> Seq.find (fun x -> x.Id = key1)
                let resForKey2 = getRes |> Seq.find (fun x -> x.Id = key2)

                test <@ resForKey1.Foo = 123L @>
                test <@ resForKey1.BAR = None @>
                test <@ resForKey2.Foo = 456L @>
                test <@ resForKey2.BAR = None @>


            testCase "Name with subdir"
            <| fun () ->
                clearTableScriptTables ()
                // Compile-time test
                ignore
                <| fun () ->
                    DbGen.Scripts.TableScriptSubdir.dbo_TableWithIdentityCol_GetById
                        .WithConnection(Config.connStr)
                        .WithParameters(1)
                        .Execute()


            testCase "Can be configured using script rules"
            <| fun () ->
                clearTableScriptTables ()
                // Compile-time test
                ignore
                <| fun () ->
                    DbGen.Scripts.TableScriptSubdir.TableScriptConfiguredWithScriptRules
                        .WithConnection(Config.connStr)
                        .WithParameters(123L, ValueNone)
                        .Execute()


            testCase "getByColumn with nullable columns use non-nullable parameters"
            <| fun () ->
                let f () =
                    let res =
                        DbGen.Scripts.Table1_ByTableCol2
                            .WithConnection(Config.connStr)
                            .WithParameters(1)
                            .ExecuteSingle()

                    res.Value.TableCol2 |> ignore<int option>

                ignore f


            testCase "getByColumnBatch with nullable columns use non-nullable parameters"
            <| fun () ->
                let f () =
                    let res =
                        DbGen.Scripts.Table1_ByTableCol2s
                            .WithConnection(Config.connStr)
                            .WithParameters([ DbGen.TableTypes.dbo.SingleColNonNull.create 1 ])
                            .ExecuteSingle()

                    res.Value.TableCol2 |> ignore<int option>

                ignore f


            testCase "Can have multiple getByColumns with different columns"
            <| fun () ->
                let f () =
                    DbGen.Scripts.Table1_ByTableCol1
                        .WithConnection(Config.connStr)
                        .WithParameters("")
                        .ExecuteSingle()
                    |> ignore

                    DbGen.Scripts.Table1_ByTableCol2
                        .WithConnection(Config.connStr)
                        .WithParameters(0)
                        .ExecuteSingle()
                    |> ignore

                ignore f


            testCase "Skips computed columns in insert scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Insert
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch insert scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_InsertBatch
                        .WithConnection(Config.connStr)
                        .WithParameters([ DbGen.Scripts.TableWithComputedCol_InsertBatch.args.create (0, 0L) ])
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in insert scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Insert_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch insert scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_InsertBatch_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.TableWithComputedCol_InsertBatch_WithSkipFalse.args.create (0, 0L)
                            ]
                        )
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in update scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Update
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch update scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_UpdateBatch
                        .WithConnection(Config.connStr)
                        .WithParameters([ DbGen.Scripts.TableWithComputedCol_UpdateBatch.args.create (0, 0L) ])
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in update scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Update_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch update scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_UpdateBatch_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.TableWithComputedCol_UpdateBatch_WithSkipFalse.args.create (0, 0L)
                            ]
                        )
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in merge scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Merge
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch merge scripts"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_MergeBatch
                        .WithConnection(Config.connStr)
                        .WithParameters([ DbGen.Scripts.TableWithComputedCol_MergeBatch.args.create (0, 0L) ])
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in merge scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_Merge_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(0, 0L)
                        .Execute()
                    |> ignore

                ignore f


            testCase "Skips computed columns in batch merge scripts even if skip = false"
            <| fun () ->
                let f () =
                    DbGen.Scripts.TableWithComputedCol_MergeBatch_WithSkipFalse
                        .WithConnection(Config.connStr)
                        .WithParameters(
                            [
                                DbGen.Scripts.TableWithComputedCol_MergeBatch_WithSkipFalse.args.create (0, 0L)
                            ]
                        )
                        .Execute()
                    |> ignore

                ignore f


        ]

    ]
