module PrecisionTests

open Expecto
open Hedgehog
open Swensen.Unquote


[<Tests>]
let tests =
    testList "Precision tests" [


        testList "Can roundtrip values without losing precision" [


            testCase "Normal parameters"
            <| fun () ->
                Property.check
                <| property {
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
                    let! rowversion = Gen.Sql.rowversion
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! timestamp = Gen.Sql.timestamp
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypes
                            .WithConnection(Config.connStr)
                            .WithParameters(
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
                                rowversion,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                timestamp,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
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
                                rowversion,
                                smalldatetime,
                                smallint,
                                smallmoney,
                                text,
                                time_1,
                                timestamp,
                                tinyint,
                                uniqueidentifier,
                                varbinary_42,
                                varchar_42,
                                xml
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>
                }


            testCase "Normal nullable parameters"
            <| fun () ->
                Property.check
                <| property {
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
                    let! rowversion = Gen.Sql.rowversion
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! timestamp = Gen.Sql.timestamp
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                Some bigint,
                                Some binary_42,
                                Some bit,
                                Some char_42,
                                Some date,
                                Some datetime,
                                Some datetime2_3,
                                Some datetimeoffset_1,
                                Some decimal_10_5,
                                Some float_42,
                                Some image,
                                Some int,
                                Some money,
                                Some nchar_42,
                                Some ntext,
                                Some numeric_8_3,
                                Some nvarchar_42,
                                Some real,
                                Some rowversion,
                                Some smalldatetime,
                                Some smallint,
                                Some smallmoney,
                                Some text,
                                Some time_1,
                                Some timestamp,
                                Some tinyint,
                                Some uniqueidentifier,
                                Some varbinary_42,
                                Some varchar_42,
                                Some xml
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                Some bigint,
                                Some binary_42,
                                Some bit,
                                Some char_42,
                                Some date,
                                Some datetime,
                                Some datetime2_3,
                                Some datetimeoffset_1,
                                Some decimal_10_5,
                                Some float_42,
                                Some image,
                                Some int,
                                Some money,
                                Some nchar_42,
                                Some ntext,
                                Some numeric_8_3,
                                Some nvarchar_42,
                                Some real,
                                Some rowversion,
                                Some smalldatetime,
                                Some smallint,
                                Some smallmoney,
                                Some text,
                                Some time_1,
                                Some timestamp,
                                Some tinyint,
                                Some uniqueidentifier,
                                Some varbinary_42,
                                Some varchar_42,
                                Some xml
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>
                }


            testCase "DTO parameters"
            <| fun () ->
                Property.check
                <| property {
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
                    let! rowversion = Gen.Sql.rowversion
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! timestamp = Gen.Sql.timestamp
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypes
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                {|
                                    Bigint = bigint
                                    Binary = binary_42
                                    Bit = bit
                                    Char = char_42
                                    Date = date
                                    Datetime = datetime
                                    Datetime2 = datetime2_3
                                    Datetimeoffset = datetimeoffset_1
                                    Decimal = decimal_10_5
                                    Float = float_42
                                    Image = image
                                    Int = int
                                    Money = money
                                    Nchar = nchar_42
                                    Ntext = ntext
                                    Numeric = numeric_8_3
                                    Nvarchar = nvarchar_42
                                    Real = real
                                    Rowversion = rowversion
                                    Smalldatetime = smalldatetime
                                    Smallint = smallint
                                    Smallmoney = smallmoney
                                    Text = text
                                    Time = time_1
                                    Timestamp = timestamp
                                    Tinyint = tinyint
                                    Uniqueidentifier = uniqueidentifier
                                    Varbinary = varbinary_42
                                    Varchar = varchar_42
                                    Xml = xml
                                |}
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                {|
                                    Bigint = bigint
                                    Binary = binary_42
                                    Bit = bit
                                    Char = char_42
                                    Date = date
                                    Datetime = datetime
                                    Datetime2 = datetime2_3
                                    Datetimeoffset = datetimeoffset_1
                                    Decimal = decimal_10_5
                                    Float = float_42
                                    Image = image
                                    Int = int
                                    Money = money
                                    Nchar = nchar_42
                                    Ntext = ntext
                                    Numeric = numeric_8_3
                                    Nvarchar = nvarchar_42
                                    Real = real
                                    Rowversion = rowversion
                                    Smalldatetime = smalldatetime
                                    Smallint = smallint
                                    Smallmoney = smallmoney
                                    Text = text
                                    Time = time_1
                                    Timestamp = timestamp
                                    Tinyint = tinyint
                                    Uniqueidentifier = uniqueidentifier
                                    Varbinary = varbinary_42
                                    Varchar = varchar_42
                                    Xml = xml
                                |}
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>
                }


            testCase "Nullable DTO parameters"
            <| fun () ->
                Property.check
                <| property {
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
                    let! rowversion = Gen.Sql.rowversion
                    let! smalldatetime = Gen.Sql.smalldatetime
                    let! smallint = Gen.Sql.smallint
                    let! smallmoney = Gen.Sql.smallmoney
                    let! text = Gen.Sql.text
                    let! time_1 = Gen.Sql.time 1
                    let! timestamp = Gen.Sql.timestamp
                    let! tinyint = Gen.Sql.tinyint
                    let! uniqueidentifier = Gen.Sql.uniqueidentifier
                    let! varbinary_42 = Gen.Sql.varbinary 42
                    let! varchar_42 = Gen.Sql.varchar 42
                    let! xml = Gen.Sql.xml

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                {|
                                    Bigint = Some bigint
                                    Binary = Some binary_42
                                    Bit = Some bit
                                    Char = Some char_42
                                    Date = Some date
                                    Datetime = Some datetime
                                    Datetime2 = Some datetime2_3
                                    Datetimeoffset = Some datetimeoffset_1
                                    Decimal = Some decimal_10_5
                                    Float = Some float_42
                                    Image = Some image
                                    Int = Some int
                                    Money = Some money
                                    Nchar = Some nchar_42
                                    Ntext = Some ntext
                                    Numeric = Some numeric_8_3
                                    Nvarchar = Some nvarchar_42
                                    Real = Some real
                                    Rowversion = Some rowversion
                                    Smalldatetime = Some smalldatetime
                                    Smallint = Some smallint
                                    Smallmoney = Some smallmoney
                                    Text = Some text
                                    Time = Some time_1
                                    Timestamp = Some timestamp
                                    Tinyint = Some tinyint
                                    Uniqueidentifier = Some uniqueidentifier
                                    Varbinary = Some varbinary_42
                                    Varchar = Some varchar_42
                                    Xml = Some xml
                                |}
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesNullExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                {|
                                    Bigint = Some bigint
                                    Binary = Some binary_42
                                    Bit = Some bit
                                    Char = Some char_42
                                    Date = Some date
                                    Datetime = Some datetime
                                    Datetime2 = Some datetime2_3
                                    Datetimeoffset = Some datetimeoffset_1
                                    Decimal = Some decimal_10_5
                                    Float = Some float_42
                                    Image = Some image
                                    Int = Some int
                                    Money = Some money
                                    Nchar = Some nchar_42
                                    Ntext = Some ntext
                                    Numeric = Some numeric_8_3
                                    Nvarchar = Some nvarchar_42
                                    Real = Some real
                                    Rowversion = Some rowversion
                                    Smalldatetime = Some smalldatetime
                                    Smallint = Some smallint
                                    Smallmoney = Some smallmoney
                                    Text = Some text
                                    Time = Some time_1
                                    Timestamp = Some timestamp
                                    Tinyint = Some tinyint
                                    Uniqueidentifier = Some uniqueidentifier
                                    Varbinary = Some varbinary_42
                                    Varchar = Some varchar_42
                                    Xml = Some xml
                                |}
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ rowversion = res.Value.rowversion.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ timestamp = res.Value.timestamp.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>
                }


            testCase "TVP parameters"
            <| fun () ->
                Property.check
                <| property {
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

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.TableTypes.dbo.AllTypesNonNull.create
                                        {|
                                            bigint = bigint
                                            binary = binary_42
                                            bit = bit
                                            char = char_42
                                            date = date
                                            datetime = datetime
                                            datetime2 = datetime2_3
                                            datetimeoffset = datetimeoffset_1
                                            decimal = decimal_10_5
                                            float = float_42
                                            image = image
                                            int = int
                                            money = money
                                            nchar = nchar_42
                                            ntext = ntext
                                            numeric = numeric_8_3
                                            nvarchar = nvarchar_42
                                            real = real
                                            smalldatetime = smalldatetime
                                            smallint = smallint
                                            smallmoney = smallmoney
                                            text = text
                                            time = time_1
                                            tinyint = tinyint
                                            uniqueidentifier = uniqueidentifier
                                            varbinary = varbinary_42
                                            varchar = varchar_42
                                            xml = xml
                                        |}
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint @>
                    test <@ binary_42 = res.Value.binary @>
                    test <@ bit = res.Value.bit @>
                    test <@ char_42 = res.Value.char @>
                    test <@ date = res.Value.date @>
                    test <@ datetime = res.Value.datetime @>
                    test <@ datetime2_3 = res.Value.datetime2 @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset @>
                    test <@ decimal_10_5 = res.Value.decimal @>
                    test <@ float_42 = res.Value.float @>
                    test <@ image = res.Value.image @>
                    test <@ int = res.Value.int @>
                    test <@ money = res.Value.money @>
                    test <@ nchar_42 = res.Value.nchar @>
                    test <@ ntext = res.Value.ntext @>
                    test <@ numeric_8_3 = res.Value.numeric @>
                    test <@ nvarchar_42 = res.Value.nvarchar @>
                    test <@ real = res.Value.real @>
                    test <@ smalldatetime = res.Value.smalldatetime @>
                    test <@ smallint = res.Value.smallint @>
                    test <@ smallmoney = res.Value.smallmoney @>
                    test <@ text = res.Value.text @>
                    test <@ time_1 = res.Value.time @>
                    test <@ tinyint = res.Value.tinyint @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier @>
                    test <@ varbinary_42 = res.Value.varbinary @>
                    test <@ varchar_42 = res.Value.varchar @>
                    test <@ xml = res.Value.xml @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNonNullExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.TableTypes.dbo.AllTypesNonNull.create
                                        {|
                                            bigint = bigint
                                            binary = binary_42
                                            bit = bit
                                            char = char_42
                                            date = date
                                            datetime = datetime
                                            datetime2 = datetime2_3
                                            datetimeoffset = datetimeoffset_1
                                            decimal = decimal_10_5
                                            float = float_42
                                            image = image
                                            int = int
                                            money = money
                                            nchar = nchar_42
                                            ntext = ntext
                                            numeric = numeric_8_3
                                            nvarchar = nvarchar_42
                                            real = real
                                            smalldatetime = smalldatetime
                                            smallint = smallint
                                            smallmoney = smallmoney
                                            text = text
                                            time = time_1
                                            tinyint = tinyint
                                            uniqueidentifier = uniqueidentifier
                                            varbinary = varbinary_42
                                            varchar = varchar_42
                                            xml = xml
                                        |}
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint @>
                    test <@ binary_42 = res.Value.binary @>
                    test <@ bit = res.Value.bit @>
                    test <@ char_42 = res.Value.char @>
                    test <@ date = res.Value.date @>
                    test <@ datetime = res.Value.datetime @>
                    test <@ datetime2_3 = res.Value.datetime2 @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset @>
                    test <@ decimal_10_5 = res.Value.decimal @>
                    test <@ float_42 = res.Value.float @>
                    test <@ image = res.Value.image @>
                    test <@ int = res.Value.int @>
                    test <@ money = res.Value.money @>
                    test <@ nchar_42 = res.Value.nchar @>
                    test <@ ntext = res.Value.ntext @>
                    test <@ numeric_8_3 = res.Value.numeric @>
                    test <@ nvarchar_42 = res.Value.nvarchar @>
                    test <@ real = res.Value.real @>
                    test <@ smalldatetime = res.Value.smalldatetime @>
                    test <@ smallint = res.Value.smallint @>
                    test <@ smallmoney = res.Value.smallmoney @>
                    test <@ text = res.Value.text @>
                    test <@ time_1 = res.Value.time @>
                    test <@ tinyint = res.Value.tinyint @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier @>
                    test <@ varbinary_42 = res.Value.varbinary @>
                    test <@ varchar_42 = res.Value.varchar @>
                    test <@ xml = res.Value.xml @>
                }


            testCase "Nullable TVP parameters"
            <| fun () ->
                Property.check
                <| property {
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

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.TableTypes.dbo.AllTypesNull.create
                                        {|
                                            bigint = Some bigint
                                            binary = Some binary_42
                                            bit = Some bit
                                            char = Some char_42
                                            date = Some date
                                            datetime = Some datetime
                                            datetime2 = Some datetime2_3
                                            datetimeoffset = Some datetimeoffset_1
                                            decimal = Some decimal_10_5
                                            float = Some float_42
                                            image = Some image
                                            int = Some int
                                            money = Some money
                                            nchar = Some nchar_42
                                            ntext = Some ntext
                                            numeric = Some numeric_8_3
                                            nvarchar = Some nvarchar_42
                                            real = Some real
                                            smalldatetime = Some smalldatetime
                                            smallint = Some smallint
                                            smallmoney = Some smallmoney
                                            text = Some text
                                            time = Some time_1
                                            tinyint = Some tinyint
                                            uniqueidentifier = Some uniqueidentifier
                                            varbinary = Some varbinary_42
                                            varchar = Some varchar_42
                                            xml = Some xml
                                        |}
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>

                    let res =
                        DbGen.Procedures.dbo.ProcWithAllTypesFromTvpNullExtended
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                [
                                    DbGen.TableTypes.dbo.AllTypesNull.create
                                        {|
                                            bigint = Some bigint
                                            binary = Some binary_42
                                            bit = Some bit
                                            char = Some char_42
                                            date = Some date
                                            datetime = Some datetime
                                            datetime2 = Some datetime2_3
                                            datetimeoffset = Some datetimeoffset_1
                                            decimal = Some decimal_10_5
                                            float = Some float_42
                                            image = Some image
                                            int = Some int
                                            money = Some money
                                            nchar = Some nchar_42
                                            ntext = Some ntext
                                            numeric = Some numeric_8_3
                                            nvarchar = Some nvarchar_42
                                            real = Some real
                                            smalldatetime = Some smalldatetime
                                            smallint = Some smallint
                                            smallmoney = Some smallmoney
                                            text = Some text
                                            time = Some time_1
                                            tinyint = Some tinyint
                                            uniqueidentifier = Some uniqueidentifier
                                            varbinary = Some varbinary_42
                                            varchar = Some varchar_42
                                            xml = Some xml
                                        |}
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.bigint.Value @>
                    test <@ binary_42 = res.Value.binary.Value @>
                    test <@ bit = res.Value.bit.Value @>
                    test <@ char_42 = res.Value.char.Value @>
                    test <@ date = res.Value.date.Value @>
                    test <@ datetime = res.Value.datetime.Value @>
                    test <@ datetime2_3 = res.Value.datetime2.Value @>
                    test <@ datetimeoffset_1 = res.Value.datetimeoffset.Value @>
                    test <@ decimal_10_5 = res.Value.decimal.Value @>
                    test <@ float_42 = res.Value.float.Value @>
                    test <@ image = res.Value.image.Value @>
                    test <@ int = res.Value.int.Value @>
                    test <@ money = res.Value.money.Value @>
                    test <@ nchar_42 = res.Value.nchar.Value @>
                    test <@ ntext = res.Value.ntext.Value @>
                    test <@ numeric_8_3 = res.Value.numeric.Value @>
                    test <@ nvarchar_42 = res.Value.nvarchar.Value @>
                    test <@ real = res.Value.real.Value @>
                    test <@ smalldatetime = res.Value.smalldatetime.Value @>
                    test <@ smallint = res.Value.smallint.Value @>
                    test <@ smallmoney = res.Value.smallmoney.Value @>
                    test <@ text = res.Value.text.Value @>
                    test <@ time_1 = res.Value.time.Value @>
                    test <@ tinyint = res.Value.tinyint.Value @>
                    test <@ uniqueidentifier = res.Value.uniqueidentifier.Value @>
                    test <@ varbinary_42 = res.Value.varbinary.Value @>
                    test <@ varchar_42 = res.Value.varchar.Value @>
                    test <@ xml = res.Value.xml.Value @>
                }


            testCase "Temp table parameters"
            <| fun () ->
                Property.check
                <| property {
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

                    let res =
                        DbGen.Scripts.TempTableAllTypesNonNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                allTypesNonNull = [
                                    DbGen.Scripts.TempTableAllTypesNonNull.AllTypesNonNull.create (
                                        Bigint = bigint,
                                        Binary = binary_42,
                                        Bit = bit,
                                        Char = char_42,
                                        Date = date,
                                        Datetime = datetime,
                                        Datetime2 = datetime2_3,
                                        Datetimeoffset = datetimeoffset_1,
                                        Decimal = decimal_10_5,
                                        Float = float_42,
                                        Image = image,
                                        Int = int,
                                        Money = money,
                                        Nchar = nchar_42,
                                        Ntext = ntext,
                                        Numeric = numeric_8_3,
                                        Nvarchar = nvarchar_42,
                                        Real = real,
                                        Smalldatetime = smalldatetime,
                                        Smallint = smallint,
                                        Smallmoney = smallmoney,
                                        Text = text,
                                        Time = time_1,
                                        Tinyint = tinyint,
                                        Uniqueidentifier = uniqueidentifier,
                                        Varbinary = varbinary_42,
                                        Varchar = varchar_42,
                                        Xml = xml
                                    )
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.Bigint @>
                    test <@ binary_42 = res.Value.Binary @>
                    test <@ bit = res.Value.Bit @>
                    test <@ char_42 = res.Value.Char @>
                    test <@ date = res.Value.Date @>
                    test <@ datetime = res.Value.Datetime @>
                    test <@ datetime2_3 = res.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = res.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = res.Value.Decimal @>
                    test <@ float_42 = res.Value.Float @>
                    test <@ image = res.Value.Image @>
                    test <@ int = res.Value.Int @>
                    test <@ money = res.Value.Money @>
                    test <@ nchar_42 = res.Value.Nchar @>
                    test <@ ntext = res.Value.Ntext @>
                    test <@ numeric_8_3 = res.Value.Numeric @>
                    test <@ nvarchar_42 = res.Value.Nvarchar @>
                    test <@ real = res.Value.Real @>
                    test <@ smalldatetime = res.Value.Smalldatetime @>
                    test <@ smallint = res.Value.Smallint @>
                    test <@ smallmoney = res.Value.Smallmoney @>
                    test <@ text = res.Value.Text @>
                    test <@ time_1 = res.Value.Time @>
                    test <@ tinyint = res.Value.Tinyint @>
                    test <@ uniqueidentifier = res.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = res.Value.Varbinary @>
                    test <@ varchar_42 = res.Value.Varchar @>
                    test <@ xml = res.Value.Xml @>
                }


            testCase "Nullable temp table parameters"
            <| fun () ->
                Property.check
                <| property {
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

                    let res =
                        DbGen.Scripts.TempTableAllTypesNull
                            .WithConnection(Config.connStr)
                            .WithParameters(
                                allTypesNull = [
                                    DbGen.Scripts.TempTableAllTypesNull.AllTypesNull.create (
                                        Bigint = bigint,
                                        Binary = binary_42,
                                        Bit = bit,
                                        Char = char_42,
                                        Date = date,
                                        Datetime = datetime,
                                        Datetime2 = datetime2_3,
                                        Datetimeoffset = datetimeoffset_1,
                                        Decimal = decimal_10_5,
                                        Float = float_42,
                                        Image = image,
                                        Int = int,
                                        Money = money,
                                        Nchar = nchar_42,
                                        Ntext = ntext,
                                        Numeric = numeric_8_3,
                                        Nvarchar = nvarchar_42,
                                        Real = real,
                                        Smalldatetime = smalldatetime,
                                        Smallint = smallint,
                                        Smallmoney = smallmoney,
                                        Text = text,
                                        Time = time_1,
                                        Tinyint = tinyint,
                                        Uniqueidentifier = uniqueidentifier,
                                        Varbinary = varbinary_42,
                                        Varchar = varchar_42,
                                        Xml = xml
                                    )
                                ]
                            )
                            .ExecuteSingle()

                    test <@ bigint = res.Value.Bigint @>
                    test <@ binary_42 = res.Value.Binary @>
                    test <@ bit = res.Value.Bit @>
                    test <@ char_42 = res.Value.Char @>
                    test <@ date = res.Value.Date @>
                    test <@ datetime = res.Value.Datetime @>
                    test <@ datetime2_3 = res.Value.Datetime2 @>
                    test <@ datetimeoffset_1 = res.Value.Datetimeoffset @>
                    test <@ decimal_10_5 = res.Value.Decimal @>
                    test <@ float_42 = res.Value.Float @>
                    test <@ image = res.Value.Image @>
                    test <@ int = res.Value.Int @>
                    test <@ money = res.Value.Money @>
                    test <@ nchar_42 = res.Value.Nchar @>
                    test <@ ntext = res.Value.Ntext @>
                    test <@ numeric_8_3 = res.Value.Numeric @>
                    test <@ nvarchar_42 = res.Value.Nvarchar @>
                    test <@ real = res.Value.Real @>
                    test <@ smalldatetime = res.Value.Smalldatetime @>
                    test <@ smallint = res.Value.Smallint @>
                    test <@ smallmoney = res.Value.Smallmoney @>
                    test <@ text = res.Value.Text @>
                    test <@ time_1 = res.Value.Time @>
                    test <@ tinyint = res.Value.Tinyint @>
                    test <@ uniqueidentifier = res.Value.Uniqueidentifier @>
                    test <@ varbinary_42 = res.Value.Varbinary @>
                    test <@ varchar_42 = res.Value.Varchar @>
                    test <@ xml = res.Value.Xml @>
                }

        ]

    ]
