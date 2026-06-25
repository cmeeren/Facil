// Edit or remove any part of this header to force regeneration.
// Manifest:
(*
{
  "facil": {
    "assemblyVersion": "3.0.0+f077eabfed99907b2dfb79c7c2c585a625a20085",
    "assemblyHash": "d88fc56d62e517a95624723913ea079a"
  },
  "config": {
    "path": "facil.yaml",
    "configsHash": "9c7e87f1906bb406ad64d4e9e8264319",
    "rulesetsHash": "49a57be0eca1c8c273117b3002e5bcfb"
  },
  "scripts": [
    {
      "path": "TempTableAllTypesNonNull.sql",
      "hash": "ee65b1fd4a97e8ae1f8d64d2c32037f2"
    }
  ]
}
*)

[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.0.0+f077eabfed99907b2dfb79c7c2c585a625a20085")>]
module DateOnlyDbGen

#nowarn "49"
#nowarn "3261"

open System
open System.ComponentModel
open System.Data
open System.Threading
open Microsoft.Data.SqlClient
open Microsoft.Data.SqlClient.Server
open Facil.Runtime.CSharp
open Facil.Runtime.GeneratedCodeUtils


[<EditorBrowsable(EditorBrowsableState.Never)>]
type InternalUseOnly = private | InternalUseOnly
[<EditorBrowsable(EditorBrowsableState.Never)>]
let internalUseOnlyValue = InternalUseOnly


module TableDtos =


  module ``dbo`` =


    type ``AllTypesNonNull`` =
      {
        ``Key``: int
        ``Bigint``: int64
        ``Binary``: byte []
        ``Bit``: bool
        ``Char``: string
        ``Date``: DateOnly
        ``Datetime``: DateTime
        ``Datetime2``: DateTime
        ``Datetimeoffset``: DateTimeOffset
        ``Decimal``: decimal
        ``Float``: float
        ``Image``: byte []
        ``Int``: int
        ``Money``: decimal
        ``Nchar``: string
        ``Ntext``: string
        ``Numeric``: decimal
        ``Nvarchar``: string
        ``Real``: float32
        ``Smalldatetime``: DateTime
        ``Smallint``: int16
        ``Smallmoney``: decimal
        ``Text``: string
        ``Time``: TimeSpan
        ``Tinyint``: byte
        ``Uniqueidentifier``: Guid
        ``Varbinary``: byte []
        ``Varchar``: string
        ``Xml``: string
      }

      static member getPrimaryKey (dto: ``AllTypesNonNull``) =
        dto.``Key``


module TableTypes =


  module ``dbo`` =


    let private ``AllTypesNonNull_meta`` =
      [|
        SqlMetaData("bigint", SqlDbType.BigInt)
        SqlMetaData("binary", SqlDbType.Binary, 42L)
        SqlMetaData("bit", SqlDbType.Bit)
        SqlMetaData("char", SqlDbType.Char, 42L)
        SqlMetaData("date", SqlDbType.Date)
        SqlMetaData("datetime", SqlDbType.DateTime)
        SqlMetaData("datetime2", SqlDbType.DateTime2, 0uy, 3uy)
        SqlMetaData("datetimeoffset", SqlDbType.DateTimeOffset, 0uy, 1uy)
        SqlMetaData("decimal", SqlDbType.Decimal, 10uy, 5uy)
        SqlMetaData("float", SqlDbType.Float)
        SqlMetaData("image", SqlDbType.Image)
        SqlMetaData("int", SqlDbType.Int)
        SqlMetaData("money", SqlDbType.Money)
        SqlMetaData("nchar", SqlDbType.NChar, 42L)
        SqlMetaData("ntext", SqlDbType.NText)
        SqlMetaData("numeric", SqlDbType.Decimal, 8uy, 3uy)
        SqlMetaData("nvarchar", SqlDbType.NVarChar, 42L)
        SqlMetaData("real", SqlDbType.Real)
        SqlMetaData("smalldatetime", SqlDbType.SmallDateTime)
        SqlMetaData("smallint", SqlDbType.SmallInt)
        SqlMetaData("smallmoney", SqlDbType.SmallMoney)
        SqlMetaData("text", SqlDbType.Text)
        SqlMetaData("time", SqlDbType.Time, 0uy, 1uy)
        SqlMetaData("tinyint", SqlDbType.TinyInt)
        SqlMetaData("uniqueidentifier", SqlDbType.UniqueIdentifier)
        SqlMetaData("varbinary", SqlDbType.VarBinary, 42L)
        SqlMetaData("varchar", SqlDbType.VarChar, 42L)
        SqlMetaData("xml", SqlDbType.Xml)
      |]


    type ``AllTypesNonNull`` (__: InternalUseOnly) =
      inherit SqlDataRecord (``AllTypesNonNull_meta``)

      static member create
        (
          ``bigint``: int64,
          ``binary``: byte [],
          ``bit``: bool,
          ``char``: string,
          ``date``: DateOnly,
          ``datetime``: DateTime,
          ``datetime2``: DateTime,
          ``datetimeoffset``: DateTimeOffset,
          ``decimal``: decimal,
          ``float``: float,
          ``image``: byte [],
          ``int``: int,
          ``money``: decimal,
          ``nchar``: string,
          ``ntext``: string,
          ``numeric``: decimal,
          ``nvarchar``: string,
          ``real``: float32,
          ``smalldatetime``: DateTime,
          ``smallint``: int16,
          ``smallmoney``: decimal,
          ``text``: string,
          ``time``: TimeSpan,
          ``tinyint``: byte,
          ``uniqueidentifier``: Guid,
          ``varbinary``: byte [],
          ``varchar``: string,
          ``xml``: string
        ) =
        let x = ``AllTypesNonNull``(internalUseOnlyValue)
        x.SetValues(
          ``bigint``,
          ``binary``,
          ``bit``,
          ``char``,
          DateTime(``date``.Year, ``date``.Month, ``date``.Day),
          ``datetime``,
          ``datetime2``,
          ``datetimeoffset``,
          ``decimal``,
          ``float``,
          ``image``,
          ``int``,
          ``money``,
          ``nchar``,
          ``ntext``,
          ``numeric``,
          ``nvarchar``,
          ``real``,
          ``smalldatetime``,
          ``smallint``,
          ``smallmoney``,
          ``text``,
          ``time``,
          ``tinyint``,
          ``uniqueidentifier``,
          ``varbinary``,
          ``varchar``,
          ``xml``
        )
        |> ignore
        x

      static member inline create (dto: ^a) =
        let x = ``AllTypesNonNull``(internalUseOnlyValue)
        x.SetValues(
          (^a: (member ``bigint``: int64) dto),
          (^a: (member ``binary``: byte []) dto),
          (^a: (member ``bit``: bool) dto),
          (^a: (member ``char``: string) dto),
          DateTime((^a: (member ``date``: DateOnly) dto).Year, (^a: (member ``date``: DateOnly) dto).Month, (^a: (member ``date``: DateOnly) dto).Day),
          (^a: (member ``datetime``: DateTime) dto),
          (^a: (member ``datetime2``: DateTime) dto),
          (^a: (member ``datetimeoffset``: DateTimeOffset) dto),
          (^a: (member ``decimal``: decimal) dto),
          (^a: (member ``float``: float) dto),
          (^a: (member ``image``: byte []) dto),
          (^a: (member ``int``: int) dto),
          (^a: (member ``money``: decimal) dto),
          (^a: (member ``nchar``: string) dto),
          (^a: (member ``ntext``: string) dto),
          (^a: (member ``numeric``: decimal) dto),
          (^a: (member ``nvarchar``: string) dto),
          (^a: (member ``real``: float32) dto),
          (^a: (member ``smalldatetime``: DateTime) dto),
          (^a: (member ``smallint``: int16) dto),
          (^a: (member ``smallmoney``: decimal) dto),
          (^a: (member ``text``: string) dto),
          (^a: (member ``time``: TimeSpan) dto),
          (^a: (member ``tinyint``: byte) dto),
          (^a: (member ``uniqueidentifier``: Guid) dto),
          (^a: (member ``varbinary``: byte []) dto),
          (^a: (member ``varchar``: string) dto),
          (^a: (member ``xml``: string) dto)
        )
        |> ignore
        x


module Procedures =


  module ``dbo`` =


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type ``ProcDateOnlyOut_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

      let configureCmd sqlParams (cmd: SqlCommand) =
        cmd.CommandType <- CommandType.StoredProcedure
        cmd.CommandText <- "dbo.ProcDateOnlyOut"
        cmd.Parameters.AddRange sqlParams
        userConfigureCmd cmd

      let initOrdinals = ignore<SqlDataReader>

      let getItem (reader: SqlDataReader) =
        if reader.IsDBNull 0 then None else reader.GetFieldValue<DateOnly> 0 |> Some

      let wrapResultWithOutParams (sqlParams: SqlParameter []) result =
        {|
          Result = result
          Out =
            {|
              ``dateOut`` = if sqlParams[1].Value = box DBNull.Value then None else sqlParams[1].Value |> unbox<DateTime> |> DateOnly.FromDateTime |> Some
            |}
        |}

      member _.ExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)
        |> Task.map (wrapResultWithOutParams sqlParams)

      member this.AsyncExecute() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)
        |> Task.map (wrapResultWithOutParams sqlParams)

      member this.AsyncExecuteWithSyncRead() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsyncWithSyncRead(ct) |> Async.AwaitTask
        }

      member _.Execute() =
        let sqlParams = getSqlParams ()
        executeQueryEager connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData
        |> wrapResultWithOutParams sqlParams

      member _.LazyExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecute() =
        let sqlParams = getSqlParams ()
        executeQueryLazy connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      member _.ExecuteSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQuerySingleAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)
        |> Task.map (wrapResultWithOutParams sqlParams)

      member this.AsyncExecuteSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteSingleAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteSingle() =
        let sqlParams = getSqlParams ()
        executeQuerySingle connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData
        |> wrapResultWithOutParams sqlParams

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReader() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderAsync(ct) |> Async.AwaitTask
        }

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReader() =
        let sqlParams = getSqlParams ()
        executeReader connStr conn tran configureConn (configureCmd sqlParams) tempTableData

      /// Same as ExecuteReaderAsync, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderSingleAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Same as AsyncExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReaderSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderSingleAsync(ct) |> Async.AwaitTask
        }

      /// Same as ExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingle() =
        let sqlParams = getSqlParams ()
        executeReaderSingle connStr conn tran configureConn (configureCmd sqlParams) tempTableData


    type ``ProcDateOnlyOut`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      new() =
        failwith "This constructor is for aiding reflection and type constraints only"
        ``ProcDateOnlyOut``(null, null, null)

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val connStr = connStr

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val conn = conn

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val tran = tran

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val configureConn : SqlConnection -> unit = ignore with get, set

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val userConfigureCmd : SqlCommand -> unit = ignore with get, set

      member this.ConfigureCommand(configureCommand: SqlCommand -> unit) =
        this.userConfigureCmd <- configureCommand
        this

      static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) =
        ``ProcDateOnlyOut``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

      static member WithConnection(connection, ?transaction) = ``ProcDateOnlyOut``(null, connection, defaultArg transaction null)

      member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
        match configureConnection with
        | None -> ()
        | Some config -> this.configureConn <- config
        this

      member this.WithParameters
        (
          ``dateParam``: DateOnly,
          ?``dateOut``: DateOnly option
        ) =
        let getSqlParams () =
          [|
            SqlParameter("@dateParam", SqlDbType.Date, Value = ``dateParam``)
            SqlParameter("@dateOut", SqlDbType.Date, Direction = ParameterDirection.InputOutput, Value = (``dateOut`` |> Option.map Option.toDbNull |> Option.defaultValue (box DBNull.Value)))
          |]
        ``ProcDateOnlyOut_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

      member inline this.WithParameters(dto: ^a) =
        let getSqlParams () =
          [|
            SqlParameter("@dateParam", SqlDbType.Date, Value = (^a: (member ``DateParam``: DateOnly) dto))
            SqlParameter("@dateOut", SqlDbType.Date, Direction = ParameterDirection.InputOutput, Value = Option.toDbNull (^a: (member ``DateOut``: DateOnly option) dto))
          |]
        ``ProcDateOnlyOut_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type ``ProcWithAllTypes_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

      let configureCmd sqlParams (cmd: SqlCommand) =
        cmd.CommandType <- CommandType.StoredProcedure
        cmd.CommandText <- "dbo.ProcWithAllTypes"
        cmd.Parameters.AddRange sqlParams
        userConfigureCmd cmd

      let mutable ``ordinal_bigint`` = 0
      let mutable ``ordinal_binary`` = 0
      let mutable ``ordinal_bit`` = 0
      let mutable ``ordinal_char`` = 0
      let mutable ``ordinal_date`` = 0
      let mutable ``ordinal_datetime`` = 0
      let mutable ``ordinal_datetime2`` = 0
      let mutable ``ordinal_datetimeoffset`` = 0
      let mutable ``ordinal_decimal`` = 0
      let mutable ``ordinal_float`` = 0
      let mutable ``ordinal_image`` = 0
      let mutable ``ordinal_int`` = 0
      let mutable ``ordinal_money`` = 0
      let mutable ``ordinal_nchar`` = 0
      let mutable ``ordinal_ntext`` = 0
      let mutable ``ordinal_numeric`` = 0
      let mutable ``ordinal_nvarchar`` = 0
      let mutable ``ordinal_real`` = 0
      let mutable ``ordinal_rowversion`` = 0
      let mutable ``ordinal_smalldatetime`` = 0
      let mutable ``ordinal_smallint`` = 0
      let mutable ``ordinal_smallmoney`` = 0
      let mutable ``ordinal_text`` = 0
      let mutable ``ordinal_time`` = 0
      let mutable ``ordinal_timestamp`` = 0
      let mutable ``ordinal_tinyint`` = 0
      let mutable ``ordinal_uniqueidentifier`` = 0
      let mutable ``ordinal_varbinary`` = 0
      let mutable ``ordinal_varchar`` = 0
      let mutable ``ordinal_xml`` = 0

      let initOrdinals (reader: SqlDataReader) =
        ``ordinal_bigint`` <- reader.GetOrdinal "bigint"
        ``ordinal_binary`` <- reader.GetOrdinal "binary"
        ``ordinal_bit`` <- reader.GetOrdinal "bit"
        ``ordinal_char`` <- reader.GetOrdinal "char"
        ``ordinal_date`` <- reader.GetOrdinal "date"
        ``ordinal_datetime`` <- reader.GetOrdinal "datetime"
        ``ordinal_datetime2`` <- reader.GetOrdinal "datetime2"
        ``ordinal_datetimeoffset`` <- reader.GetOrdinal "datetimeoffset"
        ``ordinal_decimal`` <- reader.GetOrdinal "decimal"
        ``ordinal_float`` <- reader.GetOrdinal "float"
        ``ordinal_image`` <- reader.GetOrdinal "image"
        ``ordinal_int`` <- reader.GetOrdinal "int"
        ``ordinal_money`` <- reader.GetOrdinal "money"
        ``ordinal_nchar`` <- reader.GetOrdinal "nchar"
        ``ordinal_ntext`` <- reader.GetOrdinal "ntext"
        ``ordinal_numeric`` <- reader.GetOrdinal "numeric"
        ``ordinal_nvarchar`` <- reader.GetOrdinal "nvarchar"
        ``ordinal_real`` <- reader.GetOrdinal "real"
        ``ordinal_rowversion`` <- reader.GetOrdinal "rowversion"
        ``ordinal_smalldatetime`` <- reader.GetOrdinal "smalldatetime"
        ``ordinal_smallint`` <- reader.GetOrdinal "smallint"
        ``ordinal_smallmoney`` <- reader.GetOrdinal "smallmoney"
        ``ordinal_text`` <- reader.GetOrdinal "text"
        ``ordinal_time`` <- reader.GetOrdinal "time"
        ``ordinal_timestamp`` <- reader.GetOrdinal "timestamp"
        ``ordinal_tinyint`` <- reader.GetOrdinal "tinyint"
        ``ordinal_uniqueidentifier`` <- reader.GetOrdinal "uniqueidentifier"
        ``ordinal_varbinary`` <- reader.GetOrdinal "varbinary"
        ``ordinal_varchar`` <- reader.GetOrdinal "varchar"
        ``ordinal_xml`` <- reader.GetOrdinal "xml"

      let getItem (reader: SqlDataReader) =
        let ``bigint`` = if reader.IsDBNull ``ordinal_bigint`` then None else reader.GetInt64 ``ordinal_bigint`` |> Some
        let ``binary`` = if reader.IsDBNull ``ordinal_binary`` then None else reader.GetBytes ``ordinal_binary`` |> Some
        let ``bit`` = if reader.IsDBNull ``ordinal_bit`` then None else reader.GetBoolean ``ordinal_bit`` |> Some
        let ``char`` = if reader.IsDBNull ``ordinal_char`` then None else reader.GetString ``ordinal_char`` |> Some
        let ``date`` = if reader.IsDBNull ``ordinal_date`` then None else reader.GetFieldValue<DateOnly> ``ordinal_date`` |> Some
        let ``datetime`` = if reader.IsDBNull ``ordinal_datetime`` then None else reader.GetDateTime ``ordinal_datetime`` |> Some
        let ``datetime2`` = if reader.IsDBNull ``ordinal_datetime2`` then None else reader.GetDateTime ``ordinal_datetime2`` |> Some
        let ``datetimeoffset`` = if reader.IsDBNull ``ordinal_datetimeoffset`` then None else reader.GetDateTimeOffset ``ordinal_datetimeoffset`` |> Some
        let ``decimal`` = if reader.IsDBNull ``ordinal_decimal`` then None else reader.GetDecimal ``ordinal_decimal`` |> Some
        let ``float`` = if reader.IsDBNull ``ordinal_float`` then None else reader.GetDouble ``ordinal_float`` |> Some
        let ``image`` = if reader.IsDBNull ``ordinal_image`` then None else reader.GetBytes ``ordinal_image`` |> Some
        let ``int`` = if reader.IsDBNull ``ordinal_int`` then None else reader.GetInt32 ``ordinal_int`` |> Some
        let ``money`` = if reader.IsDBNull ``ordinal_money`` then None else reader.GetDecimal ``ordinal_money`` |> Some
        let ``nchar`` = if reader.IsDBNull ``ordinal_nchar`` then None else reader.GetString ``ordinal_nchar`` |> Some
        let ``ntext`` = if reader.IsDBNull ``ordinal_ntext`` then None else reader.GetString ``ordinal_ntext`` |> Some
        let ``numeric`` = if reader.IsDBNull ``ordinal_numeric`` then None else reader.GetDecimal ``ordinal_numeric`` |> Some
        let ``nvarchar`` = if reader.IsDBNull ``ordinal_nvarchar`` then None else reader.GetString ``ordinal_nvarchar`` |> Some
        let ``real`` = if reader.IsDBNull ``ordinal_real`` then None else reader.GetFloat ``ordinal_real`` |> Some
        let ``rowversion`` = if reader.IsDBNull ``ordinal_rowversion`` then None else reader.GetBytes ``ordinal_rowversion`` |> Some
        let ``smalldatetime`` = if reader.IsDBNull ``ordinal_smalldatetime`` then None else reader.GetDateTime ``ordinal_smalldatetime`` |> Some
        let ``smallint`` = if reader.IsDBNull ``ordinal_smallint`` then None else reader.GetInt16 ``ordinal_smallint`` |> Some
        let ``smallmoney`` = if reader.IsDBNull ``ordinal_smallmoney`` then None else reader.GetDecimal ``ordinal_smallmoney`` |> Some
        let ``text`` = if reader.IsDBNull ``ordinal_text`` then None else reader.GetString ``ordinal_text`` |> Some
        let ``time`` = if reader.IsDBNull ``ordinal_time`` then None else reader.GetTimeSpan ``ordinal_time`` |> Some
        let ``timestamp`` = if reader.IsDBNull ``ordinal_timestamp`` then None else reader.GetBytes ``ordinal_timestamp`` |> Some
        let ``tinyint`` = if reader.IsDBNull ``ordinal_tinyint`` then None else reader.GetByte ``ordinal_tinyint`` |> Some
        let ``uniqueidentifier`` = if reader.IsDBNull ``ordinal_uniqueidentifier`` then None else reader.GetGuid ``ordinal_uniqueidentifier`` |> Some
        let ``varbinary`` = if reader.IsDBNull ``ordinal_varbinary`` then None else reader.GetBytes ``ordinal_varbinary`` |> Some
        let ``varchar`` = if reader.IsDBNull ``ordinal_varchar`` then None else reader.GetString ``ordinal_varchar`` |> Some
        let ``xml`` = if reader.IsDBNull ``ordinal_xml`` then None else reader.GetString ``ordinal_xml`` |> Some
        {|
          ``bigint`` = ``bigint``
          ``binary`` = ``binary``
          ``bit`` = ``bit``
          ``char`` = ``char``
          ``date`` = ``date``
          ``datetime`` = ``datetime``
          ``datetime2`` = ``datetime2``
          ``datetimeoffset`` = ``datetimeoffset``
          ``decimal`` = ``decimal``
          ``float`` = ``float``
          ``image`` = ``image``
          ``int`` = ``int``
          ``money`` = ``money``
          ``nchar`` = ``nchar``
          ``ntext`` = ``ntext``
          ``numeric`` = ``numeric``
          ``nvarchar`` = ``nvarchar``
          ``real`` = ``real``
          ``rowversion`` = ``rowversion``
          ``smalldatetime`` = ``smalldatetime``
          ``smallint`` = ``smallint``
          ``smallmoney`` = ``smallmoney``
          ``text`` = ``text``
          ``time`` = ``time``
          ``timestamp`` = ``timestamp``
          ``tinyint`` = ``tinyint``
          ``uniqueidentifier`` = ``uniqueidentifier``
          ``varbinary`` = ``varbinary``
          ``varchar`` = ``varchar``
          ``xml`` = ``xml``
        |}

      member _.ExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecute() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecuteWithSyncRead() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsyncWithSyncRead(ct) |> Async.AwaitTask
        }

      member _.Execute() =
        let sqlParams = getSqlParams ()
        executeQueryEager connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      member _.LazyExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecute() =
        let sqlParams = getSqlParams ()
        executeQueryLazy connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      member _.ExecuteSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQuerySingleAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecuteSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteSingleAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteSingle() =
        let sqlParams = getSqlParams ()
        executeQuerySingle connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReader() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderAsync(ct) |> Async.AwaitTask
        }

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReader() =
        let sqlParams = getSqlParams ()
        executeReader connStr conn tran configureConn (configureCmd sqlParams) tempTableData

      /// Same as ExecuteReaderAsync, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderSingleAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Same as AsyncExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReaderSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderSingleAsync(ct) |> Async.AwaitTask
        }

      /// Same as ExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingle() =
        let sqlParams = getSqlParams ()
        executeReaderSingle connStr conn tran configureConn (configureCmd sqlParams) tempTableData


    type ``ProcWithAllTypes`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      new() =
        failwith "This constructor is for aiding reflection and type constraints only"
        ``ProcWithAllTypes``(null, null, null)

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val connStr = connStr

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val conn = conn

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val tran = tran

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val configureConn : SqlConnection -> unit = ignore with get, set

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val userConfigureCmd : SqlCommand -> unit = ignore with get, set

      member this.ConfigureCommand(configureCommand: SqlCommand -> unit) =
        this.userConfigureCmd <- configureCommand
        this

      static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) =
        ``ProcWithAllTypes``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

      static member WithConnection(connection, ?transaction) = ``ProcWithAllTypes``(null, connection, defaultArg transaction null)

      member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
        match configureConnection with
        | None -> ()
        | Some config -> this.configureConn <- config
        this

      member this.WithParameters
        (
          ``bigint``: int64,
          ``binary``: byte [],
          ``bit``: bool,
          ``char``: string,
          ``date``: DateOnly,
          ``datetime``: DateTime,
          ``datetime2``: DateTime,
          ``datetimeoffset``: DateTimeOffset,
          ``decimal``: decimal,
          ``float``: float,
          ``image``: byte [],
          ``int``: int,
          ``money``: decimal,
          ``nchar``: string,
          ``ntext``: string,
          ``numeric``: decimal,
          ``nvarchar``: string,
          ``real``: float32,
          ``rowversion``: byte [],
          ``smalldatetime``: DateTime,
          ``smallint``: int16,
          ``smallmoney``: decimal,
          ``text``: string,
          ``time``: TimeSpan,
          ``timestamp``: byte [],
          ``tinyint``: byte,
          ``uniqueidentifier``: Guid,
          ``varbinary``: byte [],
          ``varchar``: string,
          ``xml``: string
        ) =
        let getSqlParams () =
          [|
            SqlParameter("@bigint", SqlDbType.BigInt, Value = ``bigint``)
            SqlParameter("@binary", SqlDbType.Binary, Size = 42, Value = ``binary``)
            SqlParameter("@bit", SqlDbType.Bit, Value = ``bit``)
            SqlParameter("@char", SqlDbType.Char, Size = 42, Value = ``char``)
            SqlParameter("@date", SqlDbType.Date, Value = ``date``)
            SqlParameter("@datetime", SqlDbType.DateTime, Value = ``datetime``)
            SqlParameter("@datetime2", SqlDbType.DateTime2, Size = 7, Value = ``datetime2``)
            SqlParameter("@datetimeoffset", SqlDbType.DateTimeOffset, Size = 8, Value = ``datetimeoffset``)
            SqlParameter("@decimal", SqlDbType.Decimal, Precision = 10uy, Scale = 5uy, Value = ``decimal``)
            SqlParameter("@float", SqlDbType.Float, Size = 8, Value = ``float``)
            SqlParameter("@image", SqlDbType.Image, Value = ``image``)
            SqlParameter("@int", SqlDbType.Int, Value = ``int``)
            SqlParameter("@money", SqlDbType.Money, Value = ``money``)
            SqlParameter("@nchar", SqlDbType.NChar, Size = 42, Value = ``nchar``)
            SqlParameter("@ntext", SqlDbType.NText, Value = ``ntext``)
            SqlParameter("@numeric", SqlDbType.Decimal, Precision = 8uy, Scale = 3uy, Value = ``numeric``)
            SqlParameter("@nvarchar", SqlDbType.NVarChar, Size = 42, Value = ``nvarchar``)
            SqlParameter("@real", SqlDbType.Real, Value = ``real``)
            SqlParameter("@rowversion", SqlDbType.Timestamp, Value = ``rowversion``)
            SqlParameter("@smalldatetime", SqlDbType.SmallDateTime, Value = ``smalldatetime``)
            SqlParameter("@smallint", SqlDbType.SmallInt, Value = ``smallint``)
            SqlParameter("@smallmoney", SqlDbType.SmallMoney, Value = ``smallmoney``)
            SqlParameter("@text", SqlDbType.Text, Value = ``text``)
            SqlParameter("@time", SqlDbType.Time, Size = 3, Value = ``time``)
            SqlParameter("@timestamp", SqlDbType.Timestamp, Value = ``timestamp``)
            SqlParameter("@tinyint", SqlDbType.TinyInt, Value = ``tinyint``)
            SqlParameter("@uniqueidentifier", SqlDbType.UniqueIdentifier, Value = ``uniqueidentifier``)
            SqlParameter("@varbinary", SqlDbType.VarBinary, Size = 42, Value = ``varbinary``)
            SqlParameter("@varchar", SqlDbType.VarChar, Size = 42, Value = ``varchar``)
            SqlParameter("@xml", SqlDbType.Xml, Value = ``xml``)
          |]
        ``ProcWithAllTypes_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

      member inline this.WithParameters(dto: ^a) =
        let getSqlParams () =
          [|
            SqlParameter("@bigint", SqlDbType.BigInt, Value = (^a: (member ``Bigint``: int64) dto))
            SqlParameter("@binary", SqlDbType.Binary, Size = 42, Value = (^a: (member ``Binary``: byte []) dto))
            SqlParameter("@bit", SqlDbType.Bit, Value = (^a: (member ``Bit``: bool) dto))
            SqlParameter("@char", SqlDbType.Char, Size = 42, Value = (^a: (member ``Char``: string) dto))
            SqlParameter("@date", SqlDbType.Date, Value = (^a: (member ``Date``: DateOnly) dto))
            SqlParameter("@datetime", SqlDbType.DateTime, Value = (^a: (member ``Datetime``: DateTime) dto))
            SqlParameter("@datetime2", SqlDbType.DateTime2, Size = 7, Value = (^a: (member ``Datetime2``: DateTime) dto))
            SqlParameter("@datetimeoffset", SqlDbType.DateTimeOffset, Size = 8, Value = (^a: (member ``Datetimeoffset``: DateTimeOffset) dto))
            SqlParameter("@decimal", SqlDbType.Decimal, Precision = 10uy, Scale = 5uy, Value = (^a: (member ``Decimal``: decimal) dto))
            SqlParameter("@float", SqlDbType.Float, Size = 8, Value = (^a: (member ``Float``: float) dto))
            SqlParameter("@image", SqlDbType.Image, Value = (^a: (member ``Image``: byte []) dto))
            SqlParameter("@int", SqlDbType.Int, Value = (^a: (member ``Int``: int) dto))
            SqlParameter("@money", SqlDbType.Money, Value = (^a: (member ``Money``: decimal) dto))
            SqlParameter("@nchar", SqlDbType.NChar, Size = 42, Value = (^a: (member ``Nchar``: string) dto))
            SqlParameter("@ntext", SqlDbType.NText, Value = (^a: (member ``Ntext``: string) dto))
            SqlParameter("@numeric", SqlDbType.Decimal, Precision = 8uy, Scale = 3uy, Value = (^a: (member ``Numeric``: decimal) dto))
            SqlParameter("@nvarchar", SqlDbType.NVarChar, Size = 42, Value = (^a: (member ``Nvarchar``: string) dto))
            SqlParameter("@real", SqlDbType.Real, Value = (^a: (member ``Real``: float32) dto))
            SqlParameter("@rowversion", SqlDbType.Timestamp, Value = (^a: (member ``Rowversion``: byte []) dto))
            SqlParameter("@smalldatetime", SqlDbType.SmallDateTime, Value = (^a: (member ``Smalldatetime``: DateTime) dto))
            SqlParameter("@smallint", SqlDbType.SmallInt, Value = (^a: (member ``Smallint``: int16) dto))
            SqlParameter("@smallmoney", SqlDbType.SmallMoney, Value = (^a: (member ``Smallmoney``: decimal) dto))
            SqlParameter("@text", SqlDbType.Text, Value = (^a: (member ``Text``: string) dto))
            SqlParameter("@time", SqlDbType.Time, Size = 3, Value = (^a: (member ``Time``: TimeSpan) dto))
            SqlParameter("@timestamp", SqlDbType.Timestamp, Value = (^a: (member ``Timestamp``: byte []) dto))
            SqlParameter("@tinyint", SqlDbType.TinyInt, Value = (^a: (member ``Tinyint``: byte) dto))
            SqlParameter("@uniqueidentifier", SqlDbType.UniqueIdentifier, Value = (^a: (member ``Uniqueidentifier``: Guid) dto))
            SqlParameter("@varbinary", SqlDbType.VarBinary, Size = 42, Value = (^a: (member ``Varbinary``: byte []) dto))
            SqlParameter("@varchar", SqlDbType.VarChar, Size = 42, Value = (^a: (member ``Varchar``: string) dto))
            SqlParameter("@xml", SqlDbType.Xml, Value = (^a: (member ``Xml``: string) dto))
          |]
        ``ProcWithAllTypes_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type ``ProcWithAllTypesFromTvpNonNull_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

      let configureCmd sqlParams (cmd: SqlCommand) =
        cmd.CommandType <- CommandType.StoredProcedure
        cmd.CommandText <- "dbo.ProcWithAllTypesFromTvpNonNull"
        cmd.Parameters.AddRange sqlParams
        userConfigureCmd cmd

      let mutable ``ordinal_bigint`` = 0
      let mutable ``ordinal_binary`` = 0
      let mutable ``ordinal_bit`` = 0
      let mutable ``ordinal_char`` = 0
      let mutable ``ordinal_date`` = 0
      let mutable ``ordinal_datetime`` = 0
      let mutable ``ordinal_datetime2`` = 0
      let mutable ``ordinal_datetimeoffset`` = 0
      let mutable ``ordinal_decimal`` = 0
      let mutable ``ordinal_float`` = 0
      let mutable ``ordinal_image`` = 0
      let mutable ``ordinal_int`` = 0
      let mutable ``ordinal_money`` = 0
      let mutable ``ordinal_nchar`` = 0
      let mutable ``ordinal_ntext`` = 0
      let mutable ``ordinal_numeric`` = 0
      let mutable ``ordinal_nvarchar`` = 0
      let mutable ``ordinal_real`` = 0
      let mutable ``ordinal_smalldatetime`` = 0
      let mutable ``ordinal_smallint`` = 0
      let mutable ``ordinal_smallmoney`` = 0
      let mutable ``ordinal_text`` = 0
      let mutable ``ordinal_time`` = 0
      let mutable ``ordinal_tinyint`` = 0
      let mutable ``ordinal_uniqueidentifier`` = 0
      let mutable ``ordinal_varbinary`` = 0
      let mutable ``ordinal_varchar`` = 0
      let mutable ``ordinal_xml`` = 0

      let initOrdinals (reader: SqlDataReader) =
        ``ordinal_bigint`` <- reader.GetOrdinal "bigint"
        ``ordinal_binary`` <- reader.GetOrdinal "binary"
        ``ordinal_bit`` <- reader.GetOrdinal "bit"
        ``ordinal_char`` <- reader.GetOrdinal "char"
        ``ordinal_date`` <- reader.GetOrdinal "date"
        ``ordinal_datetime`` <- reader.GetOrdinal "datetime"
        ``ordinal_datetime2`` <- reader.GetOrdinal "datetime2"
        ``ordinal_datetimeoffset`` <- reader.GetOrdinal "datetimeoffset"
        ``ordinal_decimal`` <- reader.GetOrdinal "decimal"
        ``ordinal_float`` <- reader.GetOrdinal "float"
        ``ordinal_image`` <- reader.GetOrdinal "image"
        ``ordinal_int`` <- reader.GetOrdinal "int"
        ``ordinal_money`` <- reader.GetOrdinal "money"
        ``ordinal_nchar`` <- reader.GetOrdinal "nchar"
        ``ordinal_ntext`` <- reader.GetOrdinal "ntext"
        ``ordinal_numeric`` <- reader.GetOrdinal "numeric"
        ``ordinal_nvarchar`` <- reader.GetOrdinal "nvarchar"
        ``ordinal_real`` <- reader.GetOrdinal "real"
        ``ordinal_smalldatetime`` <- reader.GetOrdinal "smalldatetime"
        ``ordinal_smallint`` <- reader.GetOrdinal "smallint"
        ``ordinal_smallmoney`` <- reader.GetOrdinal "smallmoney"
        ``ordinal_text`` <- reader.GetOrdinal "text"
        ``ordinal_time`` <- reader.GetOrdinal "time"
        ``ordinal_tinyint`` <- reader.GetOrdinal "tinyint"
        ``ordinal_uniqueidentifier`` <- reader.GetOrdinal "uniqueidentifier"
        ``ordinal_varbinary`` <- reader.GetOrdinal "varbinary"
        ``ordinal_varchar`` <- reader.GetOrdinal "varchar"
        ``ordinal_xml`` <- reader.GetOrdinal "xml"

      let getItem (reader: SqlDataReader) =
        let ``bigint`` = reader.GetInt64 ``ordinal_bigint``
        let ``binary`` = reader.GetBytes ``ordinal_binary``
        let ``bit`` = reader.GetBoolean ``ordinal_bit``
        let ``char`` = reader.GetString ``ordinal_char``
        let ``date`` = reader.GetFieldValue<DateOnly> ``ordinal_date``
        let ``datetime`` = reader.GetDateTime ``ordinal_datetime``
        let ``datetime2`` = reader.GetDateTime ``ordinal_datetime2``
        let ``datetimeoffset`` = reader.GetDateTimeOffset ``ordinal_datetimeoffset``
        let ``decimal`` = reader.GetDecimal ``ordinal_decimal``
        let ``float`` = reader.GetDouble ``ordinal_float``
        let ``image`` = reader.GetBytes ``ordinal_image``
        let ``int`` = reader.GetInt32 ``ordinal_int``
        let ``money`` = reader.GetDecimal ``ordinal_money``
        let ``nchar`` = reader.GetString ``ordinal_nchar``
        let ``ntext`` = reader.GetString ``ordinal_ntext``
        let ``numeric`` = reader.GetDecimal ``ordinal_numeric``
        let ``nvarchar`` = reader.GetString ``ordinal_nvarchar``
        let ``real`` = reader.GetFloat ``ordinal_real``
        let ``smalldatetime`` = reader.GetDateTime ``ordinal_smalldatetime``
        let ``smallint`` = reader.GetInt16 ``ordinal_smallint``
        let ``smallmoney`` = reader.GetDecimal ``ordinal_smallmoney``
        let ``text`` = reader.GetString ``ordinal_text``
        let ``time`` = reader.GetTimeSpan ``ordinal_time``
        let ``tinyint`` = reader.GetByte ``ordinal_tinyint``
        let ``uniqueidentifier`` = reader.GetGuid ``ordinal_uniqueidentifier``
        let ``varbinary`` = reader.GetBytes ``ordinal_varbinary``
        let ``varchar`` = reader.GetString ``ordinal_varchar``
        let ``xml`` = reader.GetString ``ordinal_xml``
        {|
          ``bigint`` = ``bigint``
          ``binary`` = ``binary``
          ``bit`` = ``bit``
          ``char`` = ``char``
          ``date`` = ``date``
          ``datetime`` = ``datetime``
          ``datetime2`` = ``datetime2``
          ``datetimeoffset`` = ``datetimeoffset``
          ``decimal`` = ``decimal``
          ``float`` = ``float``
          ``image`` = ``image``
          ``int`` = ``int``
          ``money`` = ``money``
          ``nchar`` = ``nchar``
          ``ntext`` = ``ntext``
          ``numeric`` = ``numeric``
          ``nvarchar`` = ``nvarchar``
          ``real`` = ``real``
          ``smalldatetime`` = ``smalldatetime``
          ``smallint`` = ``smallint``
          ``smallmoney`` = ``smallmoney``
          ``text`` = ``text``
          ``time`` = ``time``
          ``tinyint`` = ``tinyint``
          ``uniqueidentifier`` = ``uniqueidentifier``
          ``varbinary`` = ``varbinary``
          ``varchar`` = ``varchar``
          ``xml`` = ``xml``
        |}

      member _.ExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecute() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryEagerAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecuteWithSyncRead() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteAsyncWithSyncRead(ct) |> Async.AwaitTask
        }

      member _.Execute() =
        let sqlParams = getSqlParams ()
        executeQueryEager connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      member _.LazyExecuteAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecuteAsyncWithSyncRead(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQueryLazyAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member _.LazyExecute() =
        let sqlParams = getSqlParams ()
        executeQueryLazy connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      member _.ExecuteSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeQuerySingleAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

      member this.AsyncExecuteSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteSingleAsync(ct) |> Async.AwaitTask
        }

      member _.ExecuteSingle() =
        let sqlParams = getSqlParams ()
        executeQuerySingle connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReader() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderAsync(ct) |> Async.AwaitTask
        }

      /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReader() =
        let sqlParams = getSqlParams ()
        executeReader connStr conn tran configureConn (configureCmd sqlParams) tempTableData

      /// Same as ExecuteReaderAsync, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingleAsync(?cancellationToken) =
        let sqlParams = getSqlParams ()
        executeReaderSingleAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

      /// Same as AsyncExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
      member this.AsyncExecuteReaderSingle() =
        async {
          let! ct = Async.CancellationToken
          return! this.ExecuteReaderSingleAsync(ct) |> Async.AwaitTask
        }

      /// Same as ExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
      member this.ExecuteReaderSingle() =
        let sqlParams = getSqlParams ()
        executeReaderSingle connStr conn tran configureConn (configureCmd sqlParams) tempTableData


    type ``ProcWithAllTypesFromTvpNonNull`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      new() =
        failwith "This constructor is for aiding reflection and type constraints only"
        ``ProcWithAllTypesFromTvpNonNull``(null, null, null)

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val connStr = connStr

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val conn = conn

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val tran = tran

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val configureConn : SqlConnection -> unit = ignore with get, set

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member val userConfigureCmd : SqlCommand -> unit = ignore with get, set

      member this.ConfigureCommand(configureCommand: SqlCommand -> unit) =
        this.userConfigureCmd <- configureCommand
        this

      static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) =
        ``ProcWithAllTypesFromTvpNonNull``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

      static member WithConnection(connection, ?transaction) = ``ProcWithAllTypesFromTvpNonNull``(null, connection, defaultArg transaction null)

      member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
        match configureConnection with
        | None -> ()
        | Some config -> this.configureConn <- config
        this

      member this.WithParameters
        (
          ``params``: seq<TableTypes.``dbo``.``AllTypesNonNull``>
        ) =
        let getSqlParams () =
          [|
            SqlParameter("@params", SqlDbType.Structured, TypeName = "dbo.AllTypesNonNull", Value = boxNullIfEmpty ``params``)
          |]
        ``ProcWithAllTypesFromTvpNonNull_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

      member inline this.WithParameters(dto: ^a) =
        let getSqlParams () =
          [|
            SqlParameter("@params", SqlDbType.Structured, TypeName = "dbo.AllTypesNonNull", Value = boxNullIfEmpty (^a: (member ``Params``: #seq<TableTypes.``dbo``.``AllTypesNonNull``>) dto))
          |]
        ``ProcWithAllTypesFromTvpNonNull_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)


module Scripts =


  module ``TempTableAllTypesNonNull`` =


    type ``AllTypesNonNull`` (__: InternalUseOnly, fields: obj []) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member _.Fields = fields

      static member create
        (
          ``bigint``: int64,
          ``binary``: byte [],
          ``bit``: bool,
          ``char``: string,
          ``date``: DateOnly,
          ``datetime``: DateTime,
          ``datetime2``: DateTime,
          ``datetimeoffset``: DateTimeOffset,
          ``decimal``: decimal,
          ``float``: float,
          ``image``: byte [],
          ``int``: int,
          ``money``: decimal,
          ``nchar``: string,
          ``ntext``: string,
          ``numeric``: decimal,
          ``nvarchar``: string,
          ``real``: float32,
          ``smalldatetime``: DateTime,
          ``smallint``: int16,
          ``smallmoney``: decimal,
          ``text``: string,
          ``time``: TimeSpan,
          ``tinyint``: byte,
          ``uniqueidentifier``: Guid,
          ``varbinary``: byte [],
          ``varchar``: string,
          ``xml``: string
        ) : ``AllTypesNonNull`` =
        [|
          ``bigint`` |> box
          ``binary`` |> box
          ``bit`` |> box
          ``char`` |> box
          DateTime(``date``.Year, ``date``.Month, ``date``.Day) |> box
          ``datetime`` |> box
          ``datetime2`` |> box
          ``datetimeoffset`` |> box
          ``decimal`` |> box
          ``float`` |> box
          ``image`` |> box
          ``int`` |> box
          ``money`` |> box
          ``nchar`` |> box
          ``ntext`` |> box
          ``numeric`` |> box
          ``nvarchar`` |> box
          ``real`` |> box
          ``smalldatetime`` |> box
          ``smallint`` |> box
          ``smallmoney`` |> box
          ``text`` |> box
          ``time`` |> box
          ``tinyint`` |> box
          ``uniqueidentifier`` |> box
          ``varbinary`` |> box
          ``varchar`` |> box
          ``xml`` |> box
        |]
        |> fun fields -> ``AllTypesNonNull``(internalUseOnlyValue, fields)

      static member inline create (dto: ^a) : ``AllTypesNonNull`` =
        [|
          (^a: (member ``Bigint``: int64) dto) |> box
          (^a: (member ``Binary``: byte []) dto) |> box
          (^a: (member ``Bit``: bool) dto) |> box
          (^a: (member ``Char``: string) dto) |> box
          DateTime((^a: (member ``Date``: DateOnly) dto).Year, (^a: (member ``Date``: DateOnly) dto).Month, (^a: (member ``Date``: DateOnly) dto).Day) |> box
          (^a: (member ``Datetime``: DateTime) dto) |> box
          (^a: (member ``Datetime2``: DateTime) dto) |> box
          (^a: (member ``Datetimeoffset``: DateTimeOffset) dto) |> box
          (^a: (member ``Decimal``: decimal) dto) |> box
          (^a: (member ``Float``: float) dto) |> box
          (^a: (member ``Image``: byte []) dto) |> box
          (^a: (member ``Int``: int) dto) |> box
          (^a: (member ``Money``: decimal) dto) |> box
          (^a: (member ``Nchar``: string) dto) |> box
          (^a: (member ``Ntext``: string) dto) |> box
          (^a: (member ``Numeric``: decimal) dto) |> box
          (^a: (member ``Nvarchar``: string) dto) |> box
          (^a: (member ``Real``: float32) dto) |> box
          (^a: (member ``Smalldatetime``: DateTime) dto) |> box
          (^a: (member ``Smallint``: int16) dto) |> box
          (^a: (member ``Smallmoney``: decimal) dto) |> box
          (^a: (member ``Text``: string) dto) |> box
          (^a: (member ``Time``: TimeSpan) dto) |> box
          (^a: (member ``Tinyint``: byte) dto) |> box
          (^a: (member ``Uniqueidentifier``: Guid) dto) |> box
          (^a: (member ``Varbinary``: byte []) dto) |> box
          (^a: (member ``Varchar``: string) dto) |> box
          (^a: (member ``Xml``: string) dto) |> box
        |]
        |> fun fields -> ``AllTypesNonNull``(internalUseOnlyValue, fields)


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  type ``TempTableAllTypesNonNull_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

    let configureCmd sqlParams (cmd: SqlCommand) =
      cmd.CommandText <- """-- TempTableAllTypesNonNull.sql
SELECT * FROM #AllTypesNonNull"""
      cmd.Parameters.AddRange sqlParams
      userConfigureCmd cmd

    let mutable ``ordinal_Bigint`` = 0
    let mutable ``ordinal_Binary`` = 0
    let mutable ``ordinal_Bit`` = 0
    let mutable ``ordinal_Char`` = 0
    let mutable ``ordinal_Date`` = 0
    let mutable ``ordinal_Datetime`` = 0
    let mutable ``ordinal_Datetime2`` = 0
    let mutable ``ordinal_Datetimeoffset`` = 0
    let mutable ``ordinal_Decimal`` = 0
    let mutable ``ordinal_Float`` = 0
    let mutable ``ordinal_Image`` = 0
    let mutable ``ordinal_Int`` = 0
    let mutable ``ordinal_Money`` = 0
    let mutable ``ordinal_Nchar`` = 0
    let mutable ``ordinal_Ntext`` = 0
    let mutable ``ordinal_Numeric`` = 0
    let mutable ``ordinal_Nvarchar`` = 0
    let mutable ``ordinal_Real`` = 0
    let mutable ``ordinal_Smalldatetime`` = 0
    let mutable ``ordinal_Smallint`` = 0
    let mutable ``ordinal_Smallmoney`` = 0
    let mutable ``ordinal_Text`` = 0
    let mutable ``ordinal_Time`` = 0
    let mutable ``ordinal_Tinyint`` = 0
    let mutable ``ordinal_Uniqueidentifier`` = 0
    let mutable ``ordinal_Varbinary`` = 0
    let mutable ``ordinal_Varchar`` = 0
    let mutable ``ordinal_Xml`` = 0

    let initOrdinals (reader: SqlDataReader) =
      ``ordinal_Bigint`` <- reader.GetOrdinal "Bigint"
      ``ordinal_Binary`` <- reader.GetOrdinal "Binary"
      ``ordinal_Bit`` <- reader.GetOrdinal "Bit"
      ``ordinal_Char`` <- reader.GetOrdinal "Char"
      ``ordinal_Date`` <- reader.GetOrdinal "Date"
      ``ordinal_Datetime`` <- reader.GetOrdinal "Datetime"
      ``ordinal_Datetime2`` <- reader.GetOrdinal "Datetime2"
      ``ordinal_Datetimeoffset`` <- reader.GetOrdinal "Datetimeoffset"
      ``ordinal_Decimal`` <- reader.GetOrdinal "Decimal"
      ``ordinal_Float`` <- reader.GetOrdinal "Float"
      ``ordinal_Image`` <- reader.GetOrdinal "Image"
      ``ordinal_Int`` <- reader.GetOrdinal "Int"
      ``ordinal_Money`` <- reader.GetOrdinal "Money"
      ``ordinal_Nchar`` <- reader.GetOrdinal "Nchar"
      ``ordinal_Ntext`` <- reader.GetOrdinal "Ntext"
      ``ordinal_Numeric`` <- reader.GetOrdinal "Numeric"
      ``ordinal_Nvarchar`` <- reader.GetOrdinal "Nvarchar"
      ``ordinal_Real`` <- reader.GetOrdinal "Real"
      ``ordinal_Smalldatetime`` <- reader.GetOrdinal "Smalldatetime"
      ``ordinal_Smallint`` <- reader.GetOrdinal "Smallint"
      ``ordinal_Smallmoney`` <- reader.GetOrdinal "Smallmoney"
      ``ordinal_Text`` <- reader.GetOrdinal "Text"
      ``ordinal_Time`` <- reader.GetOrdinal "Time"
      ``ordinal_Tinyint`` <- reader.GetOrdinal "Tinyint"
      ``ordinal_Uniqueidentifier`` <- reader.GetOrdinal "Uniqueidentifier"
      ``ordinal_Varbinary`` <- reader.GetOrdinal "Varbinary"
      ``ordinal_Varchar`` <- reader.GetOrdinal "Varchar"
      ``ordinal_Xml`` <- reader.GetOrdinal "Xml"

    let getItem (reader: SqlDataReader) =
      let ``Bigint`` = reader.GetInt64 ``ordinal_Bigint``
      let ``Binary`` = reader.GetBytes ``ordinal_Binary``
      let ``Bit`` = reader.GetBoolean ``ordinal_Bit``
      let ``Char`` = reader.GetString ``ordinal_Char``
      let ``Date`` = reader.GetFieldValue<DateOnly> ``ordinal_Date``
      let ``Datetime`` = reader.GetDateTime ``ordinal_Datetime``
      let ``Datetime2`` = reader.GetDateTime ``ordinal_Datetime2``
      let ``Datetimeoffset`` = reader.GetDateTimeOffset ``ordinal_Datetimeoffset``
      let ``Decimal`` = reader.GetDecimal ``ordinal_Decimal``
      let ``Float`` = reader.GetDouble ``ordinal_Float``
      let ``Image`` = reader.GetBytes ``ordinal_Image``
      let ``Int`` = reader.GetInt32 ``ordinal_Int``
      let ``Money`` = reader.GetDecimal ``ordinal_Money``
      let ``Nchar`` = reader.GetString ``ordinal_Nchar``
      let ``Ntext`` = reader.GetString ``ordinal_Ntext``
      let ``Numeric`` = reader.GetDecimal ``ordinal_Numeric``
      let ``Nvarchar`` = reader.GetString ``ordinal_Nvarchar``
      let ``Real`` = reader.GetFloat ``ordinal_Real``
      let ``Smalldatetime`` = reader.GetDateTime ``ordinal_Smalldatetime``
      let ``Smallint`` = reader.GetInt16 ``ordinal_Smallint``
      let ``Smallmoney`` = reader.GetDecimal ``ordinal_Smallmoney``
      let ``Text`` = reader.GetString ``ordinal_Text``
      let ``Time`` = reader.GetTimeSpan ``ordinal_Time``
      let ``Tinyint`` = reader.GetByte ``ordinal_Tinyint``
      let ``Uniqueidentifier`` = reader.GetGuid ``ordinal_Uniqueidentifier``
      let ``Varbinary`` = reader.GetBytes ``ordinal_Varbinary``
      let ``Varchar`` = reader.GetString ``ordinal_Varchar``
      let ``Xml`` = reader.GetString ``ordinal_Xml``
      {|
        ``Bigint`` = ``Bigint``
        ``Binary`` = ``Binary``
        ``Bit`` = ``Bit``
        ``Char`` = ``Char``
        ``Date`` = ``Date``
        ``Datetime`` = ``Datetime``
        ``Datetime2`` = ``Datetime2``
        ``Datetimeoffset`` = ``Datetimeoffset``
        ``Decimal`` = ``Decimal``
        ``Float`` = ``Float``
        ``Image`` = ``Image``
        ``Int`` = ``Int``
        ``Money`` = ``Money``
        ``Nchar`` = ``Nchar``
        ``Ntext`` = ``Ntext``
        ``Numeric`` = ``Numeric``
        ``Nvarchar`` = ``Nvarchar``
        ``Real`` = ``Real``
        ``Smalldatetime`` = ``Smalldatetime``
        ``Smallint`` = ``Smallint``
        ``Smallmoney`` = ``Smallmoney``
        ``Text`` = ``Text``
        ``Time`` = ``Time``
        ``Tinyint`` = ``Tinyint``
        ``Uniqueidentifier`` = ``Uniqueidentifier``
        ``Varbinary`` = ``Varbinary``
        ``Varchar`` = ``Varchar``
        ``Xml`` = ``Xml``
      |}

    member _.ExecuteAsync(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeQueryEagerAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

    member this.AsyncExecute() =
      async {
        let! ct = Async.CancellationToken
        return! this.ExecuteAsync(ct) |> Async.AwaitTask
      }

    member _.ExecuteAsyncWithSyncRead(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeQueryEagerAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

    member this.AsyncExecuteWithSyncRead() =
      async {
        let! ct = Async.CancellationToken
        return! this.ExecuteAsyncWithSyncRead(ct) |> Async.AwaitTask
      }

    member _.Execute() =
      let sqlParams = getSqlParams ()
      executeQueryEager connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

    member _.LazyExecuteAsync(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeQueryLazyAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

    member _.LazyExecuteAsyncWithSyncRead(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeQueryLazyAsyncWithSyncRead connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

    member _.LazyExecute() =
      let sqlParams = getSqlParams ()
      executeQueryLazy connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

    member _.ExecuteSingleAsync(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeQuerySingleAsync connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData (defaultArg cancellationToken CancellationToken.None)

    member this.AsyncExecuteSingle() =
      async {
        let! ct = Async.CancellationToken
        return! this.ExecuteSingleAsync(ct) |> Async.AwaitTask
      }

    member _.ExecuteSingle() =
      let sqlParams = getSqlParams ()
      executeQuerySingle connStr conn tran configureConn (configureCmd sqlParams) initOrdinals getItem tempTableData

    /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
    member this.ExecuteReaderAsync(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeReaderAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

    /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
    member this.AsyncExecuteReader() =
      async {
        let! ct = Async.CancellationToken
        return! this.ExecuteReaderAsync(ct) |> Async.AwaitTask
      }

    /// Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
    member this.ExecuteReader() =
      let sqlParams = getSqlParams ()
      executeReader connStr conn tran configureConn (configureCmd sqlParams) tempTableData

    /// Same as ExecuteReaderAsync, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
    member this.ExecuteReaderSingleAsync(?cancellationToken) =
      let sqlParams = getSqlParams ()
      executeReaderSingleAsync connStr conn tran configureConn (configureCmd sqlParams) tempTableData (defaultArg cancellationToken CancellationToken.None)

    /// Same as AsyncExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use!' to ensure disposal of all resources managed by Facil for this query.
    member this.AsyncExecuteReaderSingle() =
      async {
        let! ct = Async.CancellationToken
        return! this.ExecuteReaderSingleAsync(ct) |> Async.AwaitTask
      }

    /// Same as ExecuteReader, but uses CommandBehavior.SingleRow. Returns a value wrapping a SqlDataReader. The wrapper should be bound with 'use' to ensure disposal of all resources managed by Facil for this query.
    member this.ExecuteReaderSingle() =
      let sqlParams = getSqlParams ()
      executeReaderSingle connStr conn tran configureConn (configureCmd sqlParams) tempTableData


  type ``TempTableAllTypesNonNull`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    new() =
      failwith "This constructor is for aiding reflection and type constraints only"
      ``TempTableAllTypesNonNull``(null, null, null)

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val connStr = connStr

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val conn = conn

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val tran = tran

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val configureConn : SqlConnection -> unit = ignore with get, set

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val userConfigureCmd : SqlCommand -> unit = ignore with get, set

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member val userConfigureBulkCopy : SqlBulkCopy -> unit = ignore with get, set

    member this.ConfigureCommand(configureCommand: SqlCommand -> unit) =
      this.userConfigureCmd <- configureCommand
      this

    member this.ConfigureBulkCopy(configureBulkCopy: SqlBulkCopy -> unit) =
      this.userConfigureBulkCopy <- configureBulkCopy
      this

    static member WithConnection(connectionString, ?configureConnection: SqlConnection -> unit) =
      ``TempTableAllTypesNonNull``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

    static member WithConnection(connection, ?transaction) = ``TempTableAllTypesNonNull``(null, connection, defaultArg transaction null)

    member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
      match configureConnection with
      | None -> ()
      | Some config -> this.configureConn <- config
      this

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member this.CreateTempTableData
      (
        ``AllTypesNonNull``: seq<``TempTableAllTypesNonNull``.``AllTypesNonNull``>
      ) =
      [
        TempTableData
          (
            "#AllTypesNonNull",
            """
            CREATE TABLE #AllTypesNonNull (
              [Bigint] BIGINT NOT NULL,
              [Binary] BINARY(42) NOT NULL,
              [Bit] BIT NOT NULL,
              [Char] CHAR(42) NOT NULL,
              [Date] DATE NOT NULL,
              [Datetime] DATETIME NOT NULL,
              [Datetime2] DATETIME2(3) NOT NULL,
              [Datetimeoffset] DATETIMEOFFSET(1) NOT NULL,
              [Decimal] DECIMAL(10, 5) NOT NULL,
              [Float] FLOAT(42) NOT NULL,
              [Image] IMAGE NOT NULL,
              [Int] INT NOT NULL,
              [Money] MONEY NOT NULL,
              [Nchar] NCHAR(42) NOT NULL,
              [Ntext] NTEXT NOT NULL,
              [Numeric] NUMERIC(8, 3) NOT NULL,
              [Nvarchar] NVARCHAR(42) NOT NULL,
              [Real] REAL NOT NULL,
              [Smalldatetime] SMALLDATETIME NOT NULL,
              [Smallint] SMALLINT NOT NULL,
              [Smallmoney] SMALLMONEY NOT NULL,
              [Text] TEXT NOT NULL,
              [Time] TIME(1) NOT NULL,
              [Tinyint] TINYINT NOT NULL,
              [Uniqueidentifier] UNIQUEIDENTIFIER NOT NULL,
              [Varbinary] VARBINARY(42) NOT NULL,
              [Varchar] VARCHAR(42) NOT NULL,
              [Xml] XML NOT NULL
            )

            """,
            (``AllTypesNonNull`` |> Seq.map (fun x -> x.Fields)),
            [| "Bigint"; "Binary"; "Bit"; "Char"; "Date"; "Datetime"; "Datetime2"; "Datetimeoffset"; "Decimal"; "Float"; "Image"; "Int"; "Money"; "Nchar"; "Ntext"; "Numeric"; "Nvarchar"; "Real"; "Smalldatetime"; "Smallint"; "Smallmoney"; "Text"; "Time"; "Tinyint"; "Uniqueidentifier"; "Varbinary"; "Varchar"; "Xml" |],
            28,
            Action<_> this.userConfigureBulkCopy
          )
      ]
    member this.WithParameters
      (
        ``allTypesNonNull``: seq<``TempTableAllTypesNonNull``.``AllTypesNonNull``>
      ) =
      let getSqlParams () =
        [|
        |]
      let tempTableData =
        this.CreateTempTableData(
          ``allTypesNonNull``
        )
      ``TempTableAllTypesNonNull_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, tempTableData, this.tran)

    member inline this.WithParameters(dto: ^a) =
      let getSqlParams () =
        [|
        |]
      let tempTableData =
        this.CreateTempTableData(
          (^a: (member ``AllTypesNonNull``: #seq<``TempTableAllTypesNonNull``.``AllTypesNonNull``>) dto)
        )
      ``TempTableAllTypesNonNull_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, tempTableData, this.tran)
