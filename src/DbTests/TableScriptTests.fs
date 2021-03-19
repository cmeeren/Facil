module TableScriptTests

open Expecto
open Hedgehog
open Swensen.Unquote


let clearTableScriptTables () =
  DbGen.Scripts.DeleteAllFromTableScriptTables
    .WithConnection(Config.connStr)
    .Execute()
  |> ignore<int>


[<Tests>]
let tests =
  testSequenced <| testList "Table script tests" [


    testList "Basic execute + can roundtrip values without losing precision" [


      testCase "Non-null INSERT/UPDATE" <| fun () ->
        Property.check <| property {
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


      testCase "Non-null MERGE" <| fun () ->
        Property.check <| property {
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


      testCase "Null INSERT/UPDATE" <| fun () ->
        Property.check <| property {
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


      testCase "Null MERGE" <| fun () ->
        Property.check <| property {
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


      testCase "Length types" <| fun () ->
        Property.check <| property {
          clearTableScriptTables ()

          let! key = Gen.Sql.int

          let getRes =
            DbGen.Scripts.LengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ getRes = None @>

          let! binary_3 = Gen.Sql.binary 3
          let! char_3 = Gen.Sql.char 3
          let! nchar_3 = Gen.Sql.nchar 3
          let! nvarchar_3 = Gen.Sql.nvarchar 3
          let! varbinary_3 = Gen.Sql.varbinary 3
          let! varchar_3 = Gen.Sql.varchar 3

          let numInsertedRows =
            DbGen.Scripts.LengthTypes_Insert
              .WithConnection(Config.connStr)
              .WithParameters(
                key,
                binary_3,
                char_3,
                nchar_3,
                nvarchar_3,
                varbinary_3,
                varchar_3
              )
              .Execute()

          test <@ numInsertedRows = 1 @>

          let getRes =
            DbGen.Scripts.LengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ binary_3 = getRes.Value.Binary @>
          test <@ char_3 = getRes.Value.Char @>
          test <@ nchar_3 = getRes.Value.Nchar @>
          test <@ nvarchar_3 = getRes.Value.Nvarchar @>
          test <@ varbinary_3= getRes.Value.Varbinary @>
          test <@ varchar_3  = getRes.Value.Varchar @>


          let! binary_3 = Gen.Sql.binary 3
          let! char_3 = Gen.Sql.char 3
          let! nchar_3 = Gen.Sql.nchar 3
          let! nvarchar_3 = Gen.Sql.nvarchar 3
          let! varbinary_3 = Gen.Sql.varbinary 3
          let! varchar_3 = Gen.Sql.varchar 3

          let numUpdatedRows =
            DbGen.Scripts.LengthTypes_Update
              .WithConnection(Config.connStr)
              .WithParameters(
                key,
                binary_3,
                char_3,
                nchar_3,
                nvarchar_3,
                varbinary_3,
                varchar_3
              )
              .Execute()

          test <@ numUpdatedRows = 1 @>

          let getRes =
            DbGen.Scripts.LengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ binary_3 = getRes.Value.Binary @>
          test <@ char_3 = getRes.Value.Char @>
          test <@ nchar_3 = getRes.Value.Nchar @>
          test <@ nvarchar_3 = getRes.Value.Nvarchar @>
          test <@ varbinary_3 = getRes.Value.Varbinary @>
          test <@ varchar_3 = getRes.Value.Varchar @>

          let numDeletedRows =
            DbGen.Scripts.LengthTypes_Delete
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .Execute()

          test <@ numDeletedRows = 1 @>

          let getRes =
            DbGen.Scripts.LengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ getRes = None @>
        }


      testCase "Max length types" <| fun () ->
        Property.check <| property {
          clearTableScriptTables ()

          let! key = Gen.Sql.int

          let getRes =
            DbGen.Scripts.MaxLengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ getRes = None @>

          let! nvarchar_3 = Gen.Sql.nvarchar 3
          let! varbinary_3 = Gen.Sql.varbinary 3
          let! varchar_3 = Gen.Sql.varchar 3

          let numInsertedRows =
            DbGen.Scripts.MaxLengthTypes_Insert
              .WithConnection(Config.connStr)
              .WithParameters(
                key,
                nvarchar_3,
                varbinary_3,
                varchar_3
              )
              .Execute()

          test <@ numInsertedRows = 1 @>

          let getRes =
            DbGen.Scripts.MaxLengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ nvarchar_3 = getRes.Value.Nvarchar @>
          test <@ varbinary_3= getRes.Value.Varbinary @>
          test <@ varchar_3  = getRes.Value.Varchar @>


          let! nvarchar_3 = Gen.Sql.nvarchar 3
          let! varbinary_3 = Gen.Sql.varbinary 3
          let! varchar_3 = Gen.Sql.varchar 3

          let numUpdatedRows =
            DbGen.Scripts.MaxLengthTypes_Update
              .WithConnection(Config.connStr)
              .WithParameters(
                key,
                nvarchar_3,
                varbinary_3,
                varchar_3
              )
              .Execute()

          test <@ numUpdatedRows = 1 @>

          let getRes =
            DbGen.Scripts.MaxLengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ nvarchar_3 = getRes.Value.Nvarchar @>
          test <@ varbinary_3 = getRes.Value.Varbinary @>
          test <@ varchar_3 = getRes.Value.Varchar @>

          let numDeletedRows =
            DbGen.Scripts.MaxLengthTypes_Delete
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .Execute()

          test <@ numDeletedRows = 1 @>

          let getRes =
            DbGen.Scripts.MaxLengthTypes_ById
              .WithConnection(Config.connStr)
              .WithParameters(key)
              .ExecuteSingle()

          test <@ getRes = None @>
        }


    ]


    testList "Misc" [


      testCase "Updates/deletes only the specified row" <| fun () ->
        Property.check <| property {
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
            .WithParameters(
              key1,
              nvarchar_3_1,
              varbinary_3_1,
              varchar_3_1
            )
            .Execute()
          |> ignore<int>

          DbGen.Scripts.MaxLengthTypes_Insert
            .WithConnection(Config.connStr)
            .WithParameters(
              key2,
              nvarchar_3_2,
              varbinary_3_2,
              varchar_3_2
            )
            .Execute()
          |> ignore<int>

          let! nvarchar_3_1 = Gen.Sql.nvarchar 3
          let! varbinary_3_1 = Gen.Sql.varbinary 3
          let! varchar_3_1 = Gen.Sql.varchar 3

          DbGen.Scripts.MaxLengthTypes_Update
            .WithConnection(Config.connStr)
            .WithParameters(
              key1,
              nvarchar_3_1,
              varbinary_3_1,
              varchar_3_1
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

          DbGen.Scripts.MaxLengthTypes_Merge
            .WithConnection(Config.connStr)
            .WithParameters(
              key1,
              nvarchar_3_1,
              varbinary_3_1,
              varchar_3_1
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


      testCase "Identity, output columns and other column config with INSERT/UPDATE/DELETE" <| fun () ->
        Property.check <| property {
          clearTableScriptTables ()

          let insertRes =
            DbGen.Scripts.TableWithIdentityCol_Insert
              .WithConnection(Config.connStr)
              .WithParameters(foo = 0L, bar = None)
              .ExecuteSingle()

          let key = insertRes.Value.Id
          ignore<int64> insertRes.Value.Foo
          ignore<{| Id: int; Foo: int64 |}> insertRes.Value

          let! bigint = Gen.Sql.bigint
          let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

          let updateRes =
            DbGen.Scripts.TableWithIdentityCol_Update
              .WithConnection(Config.connStr)
              .WithParameters(key, foo = bigint, bar = datetimeoffset)
              .ExecuteSingle()

          ignore<{| Id: int; Foo: int64 |}> updateRes.Value

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
          |> ignore<{| Id: int; Foo: int64 |} option>
        }


      testCase "Identity, output columns and other column config with MERGE" <| fun () ->
        Property.check <| property {
          clearTableScriptTables ()

          let tempKey = -1

          let insertRes =
            DbGen.Scripts.TableWithIdentityCol_Merge
              .WithConnection(Config.connStr)
              .WithParameters(id = tempKey, foo = 0L, bar = None)
              .ExecuteSingle()

          let key = insertRes.Value.Id
          test <@ key <> tempKey @>
          ignore<{| Id: int; Foo: int64 |}> insertRes.Value
          ignore<int64> insertRes.Value.Foo

          let! bigint = Gen.Sql.bigint
          let! datetimeoffset = Gen.Sql.datetimeoffset 0 |> Gen.option

          let updateRes =
            DbGen.Scripts.TableWithIdentityCol_Merge
              .WithConnection(Config.connStr)
              .WithParameters(key, foo = bigint, bar = datetimeoffset)
              .ExecuteSingle()

          ignore<{| Id: int; Foo: int64 |}> updateRes.Value

          test <@ updateRes.Value.Foo = bigint @>
        }


      testCase "MERGE WITH (HOLDLOCK) works" <| fun () ->
        Property.check <| property {
          clearTableScriptTables ()

          let tempKey = -1

          let insertedRows =
            DbGen.Scripts.TableWithIdentityCol_MergeWithHoldlock
              .WithConnection(Config.connStr)
              .WithParameters(id = tempKey, foo = 0L, bAR = None)
              .Execute()

          test <@ insertedRows = 1 @>
        }


      testCase "GetByIdBatch" <| fun () ->
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
            .WithParameters([
              DbGen.TableTypes.dbo.SingleColNonNull.create key1
              DbGen.TableTypes.dbo.SingleColNonNull.create key2
            ])
            .Execute()


        test <@ getRes.Count = 2 @>
        let resForKey1 = getRes |> Seq.find (fun x -> x.Id = key1)
        let resForKey2 = getRes |> Seq.find (fun x -> x.Id = key2)

        test <@ resForKey1.Foo = 0L @>
        test <@ resForKey1.BAR = None @>
        test <@ resForKey2.Foo = 1L @>
        test <@ resForKey2.BAR = None @>


      testCase "GetByColumns" <| fun () ->
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


      testCase "GetByColumnsBatch" <| fun () ->
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
            .WithParameters([
              DbGen.TableTypes.dbo.FilterForTableWithIdentityCol.create insertRes1.Value
              DbGen.TableTypes.dbo.FilterForTableWithIdentityCol.create insertRes2.Value
            ])
            .Execute()

        test <@ getRes.Count = 2 @>
        let resForKey1 = getRes |> Seq.find (fun x -> x.Id = key1)
        let resForKey2 = getRes |> Seq.find (fun x -> x.Id = key2)

        test <@ resForKey1.Foo = 123L @>
        test <@ resForKey1.BAR = None @>
        test <@ resForKey2.Foo = 456L @>
        test <@ resForKey2.BAR = None @>


      testCase "Name with subdir" <| fun () ->
        clearTableScriptTables ()
        // Compile-time test
        ignore <| fun () ->
          DbGen.Scripts.TableScriptSubdir.dbo_TableWithIdentityCol_GetById
            .WithConnection(Config.connStr)
            .WithParameters(1)
            .Execute()


      testCase "Can be configured using script rules" <| fun () ->
        clearTableScriptTables ()
        // Compile-time test
        ignore <| fun () ->
          DbGen.Scripts.TableScriptSubdir.TableScriptConfiguredWithScriptRules
            .WithConnection(Config.connStr)
            .WithParameters(123L, ValueNone)
            .Execute()


      testCase "getByColumn with nullable columns use non-nullable parameters" <| fun () ->
        let f () =
          let res =
            DbGen.Scripts.Table1_ByTableCol2
              .WithConnection(Config.connStr)
              .WithParameters(1)
              .ExecuteSingle()
          res.Value.TableCol2 |> ignore<int option>
        ignore f


      testCase "getByColumnBatch with nullable columns use non-nullable parameters" <| fun () ->
        let f () =
          let res =
            DbGen.Scripts.Table1_ByTableCol2s
              .WithConnection(Config.connStr)
              .WithParameters([DbGen.TableTypes.dbo.SingleColNonNull.create 1])
              .ExecuteSingle()
          res.Value.TableCol2 |> ignore<int option>
        ignore f


    ]

  ]
