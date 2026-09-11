// Edit or remove any part of this header to force regeneration.
// Manifest:
(*
{
  "facil": {
    "assemblyVersion": "3.2.0+585adb160b6b1e5eccb4f3d13dd671a49c9fa854",
    "assemblyHash": "3f77108c0521dff2a955b04ef83bea75"
  },
  "config": {
    "path": "facil.yaml",
    "configsHash": "9c7e87f1906bb406ad64d4e9e8264319",
    "rulesetsHash": "b757b7105a1dc52aa7abefef6d6c7909"
  },
  "scripts": [
    {
      "path": "Count.sql",
      "hash": "9bbda3bbd6b1ed78ed2c7612aaf7b215"
    },
    {
      "path": "Rows.sql",
      "hash": "31b840817c2890213a8e8831869e2655"
    }
  ]
}
*)

[<System.CodeDom.Compiler.GeneratedCode("Facil", "3.2.0+585adb160b6b1e5eccb4f3d13dd671a49c9fa854")>]
module IncludeDbGen

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


module Scripts =


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  type ``Count_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

    let configureCmd sqlParams (cmd: SqlCommand) =
      cmd.CommandText <- """-- Count.sql
DECLARE @countOnly BIT = 1;
DECLARE @sql NVARCHAR(MAX) =
    CASE WHEN @countOnly = 1 THEN N'SELECT COUNT(*)' ELSE N'SELECT Id, Name' END;

SET @sql += N'
FROM (VALUES (1, N''One''), (2, N''Two'')) AS Items(Id, Name)
WHERE Id >= @minId';


EXEC sp_executesql @sql, N'@minId INT', @minId = @minId;
"""
      cmd.Parameters.AddRange sqlParams
      userConfigureCmd cmd

    let initOrdinals = ignore<SqlDataReader>

    let getItem (reader: SqlDataReader) =
      if reader.IsDBNull 0 then None else reader.GetInt32 0 |> Some

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


  type ``Count`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    new() =
      failwith "This constructor is for aiding reflection and type constraints only"
      ``Count``(null, null, null)

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
      ``Count``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

    static member WithConnection(connection, ?transaction) = ``Count``(null, connection, defaultArg transaction null)

    member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
      match configureConnection with
      | None -> ()
      | Some config -> this.configureConn <- config
      this

    member this.WithParameters
      (
        ``minId``: int
      ) =
      let getSqlParams () =
        [|
          SqlParameter("@minId", SqlDbType.Int, Value = ``minId``)
        |]
      ``Count_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

    member inline this.WithParameters(dto: ^a) =
      let getSqlParams () =
        [|
          SqlParameter("@minId", SqlDbType.Int, Value = (^a: (member ``MinId``: int) dto))
        |]
      ``Count_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  type ``Rows_Executable`` (connStr: string, conn: SqlConnection, configureConn: SqlConnection -> unit, userConfigureCmd: SqlCommand -> unit, getSqlParams: unit -> SqlParameter [], tempTableData: seq<TempTableData>, tran: SqlTransaction) =

    let configureCmd sqlParams (cmd: SqlCommand) =
      cmd.CommandText <- """-- Rows.sql
DECLARE @countOnly BIT = 0;
DECLARE @sql NVARCHAR(MAX) =
    CASE WHEN @countOnly = 1 THEN N'SELECT COUNT(*)' ELSE N'SELECT Id, Name' END;

SET @sql += N'
FROM (VALUES (1, N''One''), (2, N''Two'')) AS Items(Id, Name)
WHERE Id >= @minId';


EXEC sp_executesql @sql, N'@minId INT', @minId = @minId;
"""
      cmd.Parameters.AddRange sqlParams
      userConfigureCmd cmd

    let mutable ``ordinal_Id`` = 0
    let mutable ``ordinal_Name`` = 0

    let initOrdinals (reader: SqlDataReader) =
      ``ordinal_Id`` <- reader.GetOrdinal "Id"
      ``ordinal_Name`` <- reader.GetOrdinal "Name"

    let getItem (reader: SqlDataReader) =
      let ``Id`` = reader.GetInt32 ``ordinal_Id``
      let ``Name`` = reader.GetString ``ordinal_Name``
      {|
        ``Id`` = ``Id``
        ``Name`` = ``Name``
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


  type ``Rows`` private (connStr: string, conn: SqlConnection, tran: SqlTransaction) =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    new() =
      failwith "This constructor is for aiding reflection and type constraints only"
      ``Rows``(null, null, null)

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
      ``Rows``(connectionString, null, null).ConfigureConnection(?configureConnection=configureConnection)

    static member WithConnection(connection, ?transaction) = ``Rows``(null, connection, defaultArg transaction null)

    member private this.ConfigureConnection(?configureConnection: SqlConnection -> unit) =
      match configureConnection with
      | None -> ()
      | Some config -> this.configureConn <- config
      this

    member this.WithParameters
      (
        ``minId``: int
      ) =
      let getSqlParams () =
        [|
          SqlParameter("@minId", SqlDbType.Int, Value = ``minId``)
        |]
      ``Rows_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)

    member inline this.WithParameters(dto: ^a) =
      let getSqlParams () =
        [|
          SqlParameter("@minId", SqlDbType.Int, Value = (^a: (member ``MinId``: int) dto))
        |]
      ``Rows_Executable``(this.connStr, this.conn, this.configureConn, this.userConfigureCmd, getSqlParams, [], this.tran)
