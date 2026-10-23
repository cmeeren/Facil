// Edit or remove any part of this header to force regeneration.
// Manifest:
(*
{
  "facil": {
    "assemblyVersion": "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9",
    "assemblyHash": "1404cfb46ab133969ce3c92ae83b6c3f"
  },
  "config": {
    "path": "facil.yaml",
    "configsHash": "9c7e87f1906bb406ad64d4e9e8264319",
    "rulesetsHash": "792a261b5bcd3f2024c65c05c2f6ace3"
  },
  "scripts": [
    {
      "path": "TempTableInlined.sql",
      "hash": "f84c1c85a53c68fe545af931fcd90e01"
    }
  ]
}
*)

namespace NamespaceDbGen

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


[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9")>]
[<EditorBrowsable(EditorBrowsableState.Never)>]
type InternalUseOnly =
  private
  | InternalUseOnly

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  static member Value = InternalUseOnly


[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9")>]
module TableDtos =


  module ``dbo`` =


    type ``Table1`` =
      {
        ``TableCol1``: string
        ``TableCol2``: int option
      }


[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9")>]
module TableTypes =


  module ``dbo`` =


    let private ``MultiColNonNull_meta`` =
      [|
        SqlMetaData("Foo", SqlDbType.Int)
        SqlMetaData("Bar", SqlDbType.NVarChar, 50L)
      |]


    type ``MultiColNonNull`` (__: InternalUseOnly) =
      inherit SqlDataRecord (``MultiColNonNull_meta``)

      static member create
        (
          ``Foo``: int,
          ``Bar``: string
        ) =
        let x = ``MultiColNonNull``(InternalUseOnly.Value)
        x.SetValues(
          ``Foo``,
          ``Bar``
        )
        |> ignore
        x

      static member inline create (dto: ^a) =
        let x = ``MultiColNonNull``(InternalUseOnly.Value)
        x.SetValues(
          (^a: (member ``Foo``: int) dto),
          (^a: (member ``Bar``: string) dto)
        )
        |> ignore
        x


    let private ``SingleColNonNull_meta`` =
      [|
        SqlMetaData("Foo", SqlDbType.Int)
      |]


    type ``SingleColNonNull`` (__: InternalUseOnly) =
      inherit SqlDataRecord (``SingleColNonNull_meta``)

      static member create
        (
          ``Foo``: int
        ) =
        let x = ``SingleColNonNull``(InternalUseOnly.Value)
        x.SetValues(
          ``Foo``
        )
        |> ignore
        x

      static member inline create (dto: ^a) =
        let x = ``SingleColNonNull``(InternalUseOnly.Value)
        x.SetValues(
          (^a: (member ``Foo``: int) dto)
        )
        |> ignore
        x


[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9")>]
module Procedures =


  module ``dbo`` =


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type ``ProcWithMultipleColumnsAndTvpParams_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

      let configureCmd sqlParams (cmd: SqlCommand) =
        cmd.CommandType <- CommandType.StoredProcedure
        cmd.CommandText <- "dbo.ProcWithMultipleColumnsAndTvpParams"
        cmd.Parameters.AddRange sqlParams
        userConfigureCmd cmd

      let mutable ``ordinal_Foo`` = 0
      let mutable ``ordinal_Bar`` = 0

      let initOrdinals (reader: SqlDataReader) =
        ``ordinal_Foo`` <- reader.GetOrdinal "Foo"
        ``ordinal_Bar`` <- reader.GetOrdinal "Bar"

      let getItem (reader: SqlDataReader) =
        let ``Foo`` = reader.GetInt32 ``ordinal_Foo``
        let ``Bar`` = reader.GetString ``ordinal_Bar``
        {|
          ``Foo`` = ``Foo``
          ``Bar`` = ``Bar``
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


    type ``ProcWithMultipleColumnsAndTvpParams`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      new() =
        failwith "This constructor is for aiding reflection and type constraints only"
        ``ProcWithMultipleColumnsAndTvpParams``(null, null, null)

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
        ``ProcWithMultipleColumnsAndTvpParams``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

      static member WithConnection(connection, ?transaction) = ``ProcWithMultipleColumnsAndTvpParams``(null, connection, defaultArg transaction null)

      member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
        match configureConnection with
        | None -> ()
        | Some config -> this.configureConn <- config
        this

      member this.WithParameters
        (
          ``single``: seq<TableTypes.``dbo``.``SingleColNonNull``>,
          ``multi``: seq<TableTypes.``dbo``.``MultiColNonNull``>
        ) =
        let getSqlParams () =
          [|
            SqlParameter("@single", SqlDbType.Structured, TypeName = "dbo.SingleColNonNull", Value = boxNullIfEmpty ``single``)
            SqlParameter("@multi", SqlDbType.Structured, TypeName = "dbo.MultiColNonNull", Value = boxNullIfEmpty ``multi``)
          |]
        ``ProcWithMultipleColumnsAndTvpParams_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

      member inline this.WithParameters(dto: ^a) =
        let getSqlParams () =
          [|
            SqlParameter("@single", SqlDbType.Structured, TypeName = "dbo.SingleColNonNull", Value = boxNullIfEmpty (^a: (member ``Single``: #seq<TableTypes.``dbo``.``SingleColNonNull``>) dto))
            SqlParameter("@multi", SqlDbType.Structured, TypeName = "dbo.MultiColNonNull", Value = boxNullIfEmpty (^a: (member ``Multi``: #seq<TableTypes.``dbo``.``MultiColNonNull``>) dto))
          |]
        ``ProcWithMultipleColumnsAndTvpParams_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)


[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.3.0+e81a7c3b39430b6d0deeab6e2667b416c924d3d9")>]
module Scripts =


  module ``TempTableInlined`` =


    type ``tempTableInlined`` (__: InternalUseOnly, fields: obj []) =

      [<EditorBrowsable(EditorBrowsableState.Never)>]
      member _.Fields = fields

      static member create
        (
          ``col1``: int,
          ``col2``: string option
        ) : ``tempTableInlined`` =
        [|
          ``col1`` |> box
          Option.toDbNull ``col2`` |> box
        |]
        |> fun fields -> ``tempTableInlined``(InternalUseOnly.Value, fields)

      static member inline create (dto: ^a) : ``tempTableInlined`` =
        [|
          (^a: (member ``Col1``: int) dto) |> box
          Option.toDbNull (^a: (member ``Col2``: string option) dto) |> box
        |]
        |> fun fields -> ``tempTableInlined``(InternalUseOnly.Value, fields)


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  type ``TempTableInlined_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

    let configureCmd sqlParams (cmd: SqlCommand) =
      cmd.CommandText <- """-- TempTableInlined.sql
SELECT * FROM #tempTableInlined"""
      cmd.Parameters.AddRange sqlParams
      userConfigureCmd cmd

    let mutable ``ordinal_Col1`` = 0
    let mutable ``ordinal_Col2`` = 0

    let initOrdinals (reader: SqlDataReader) =
      ``ordinal_Col1`` <- reader.GetOrdinal "Col1"
      ``ordinal_Col2`` <- reader.GetOrdinal "Col2"

    let getItem (reader: SqlDataReader) =
      let ``Col1`` = reader.GetInt32 ``ordinal_Col1``
      let ``Col2`` = if reader.IsDBNull ``ordinal_Col2`` then None else reader.GetString ``ordinal_Col2`` |> Some
      {|
        ``Col1`` = ``Col1``
        ``Col2`` = ``Col2``
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


  type ``TempTableInlined`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    new() =
      failwith "This constructor is for aiding reflection and type constraints only"
      ``TempTableInlined``(null, null, null)

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
      ``TempTableInlined``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

    static member WithConnection(connection, ?transaction) = ``TempTableInlined``(null, connection, defaultArg transaction null)

    member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
      match configureConnection with
      | None -> ()
      | Some config -> this.configureConn <- config
      this

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member this.CreateTempTableData
      (
        ``tempTableInlined``: seq<``TempTableInlined``.``tempTableInlined``>
      ) =
      [
        TempTableData
          (
            "#tempTableInlined",
            """
            CREATE TABLE #tempTableInlined (Col1 INT NOT NULL, Col2 NVARCHAR(42) NULL)
            """,
            (``tempTableInlined`` |> Seq.map (fun x -> x.Fields)),
            [| "Col1"; "Col2" |],
            2,
            Action<_> this.userConfigureBulkCopy
          )
      ]
    member this.WithParameters
      (
        ``tempTableInlined``: seq<``TempTableInlined``.``tempTableInlined``>
      ) =
      let getSqlParams () =
        [|
        |]
      let tempTableData =
        this.CreateTempTableData(
          ``tempTableInlined``
        )
      ``TempTableInlined_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, tempTableData, this.tran)

    member inline this.WithParameters(dto: ^a) =
      let getSqlParams () =
        [|
        |]
      let tempTableData =
        this.CreateTempTableData(
          (^a: (member ``TempTableInlined``: #seq<``TempTableInlined``.``tempTableInlined``>) dto)
        )
      ``TempTableInlined_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, tempTableData, this.tran)
