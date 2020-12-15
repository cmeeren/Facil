namespace Facil.Runtime

open System
open System.ComponentModel
open System.Threading.Tasks
open Microsoft.Data.SqlClient
open Microsoft.Data.SqlClient.Server
open type Facil.Runtime.CSharp.GeneratedCodeUtils


[<EditorBrowsable(EditorBrowsableState.Never)>]
module GeneratedCodeUtils =


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryEagerAsync connStr conn configureConn configureCmd initOrdinals getItem ct : Task<ResizeArray<_>> =
    ExecuteQueryEagerAsync(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryEagerAsyncWithSyncRead connStr conn configureConn configureCmd initOrdinals getItem ct : Task<ResizeArray<_>> =
    ExecuteQueryEagerAsyncWithSyncRead(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryEager connStr conn configureConn configureCmd initOrdinals getItem : ResizeArray<_> =
    ExecuteQueryEager(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem)

  #if !NETSTANDARD2_0
  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryLazyAsync connStr conn configureConn configureCmd initOrdinals getItem ct =
    ExecuteQueryLazyAsync(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryLazyAsyncWithSyncRead connStr conn configureConn configureCmd initOrdinals getItem ct =
    ExecuteQueryLazyAsyncWithSyncRead(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)
  #endif

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQueryLazy connStr conn configureConn configureCmd initOrdinals getItem =
    ExecuteQueryLazy(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQuerySingleAsync connStr conn configureConn configureCmd initOrdinals getItem ct : Task<_ option> =
    ExecuteQuerySingleAsync(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQuerySingleAsyncVoption connStr conn configureConn configureCmd initOrdinals getItem ct : Task<_ voption> =
    ExecuteQuerySingleAsyncVoption(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQuerySingle connStr conn configureConn configureCmd initOrdinals getItem : _ option =
    ExecuteQuerySingle(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeQuerySingleVoption connStr conn configureConn configureCmd initOrdinals getItem : _ voption =
    ExecuteQuerySingleVoption(conn, connStr, Action<_> configureConn, Action<_> configureCmd, Action<_> initOrdinals, Func<_,_> getItem)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeNonQueryAsync connStr conn configureConn configureCmd ct =
    ExecuteNonQueryAsync(conn, connStr, Action<_> configureConn, Action<_> configureCmd, ct)

  [<EditorBrowsable(EditorBrowsableState.Never)>]
  let inline executeNonQuery connStr conn configureConn configureCmd =
    ExecuteNonQuery(conn, connStr, Action<_> configureConn, Action<_> configureCmd)



  type SqlDataRecord with

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member inline this.WithValues([<ParamArray>] values: obj []) =
      this.SetValues(values) |> ignore
      this


  type SqlDataReader with

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    member inline this.GetBytes(ordinal: int) =
      let length = this.GetBytes(ordinal, 0L, null, 0, Int32.MaxValue) |> int32
      let buffer = Array.zeroCreate length
      this.GetBytes(ordinal, 0L, buffer, 0, buffer.Length) |> ignore
      buffer


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  module Option =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline toDbNull x =
      match x with
      | None -> box DBNull.Value
      | Some x -> box x


  [<EditorBrowsable(EditorBrowsableState.Never)>]
  module ValueOption =

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline toDbNull x =
      match x with
      | ValueNone -> box DBNull.Value
      | ValueSome x -> box x


  module Task =

    let map f (t: Task<_>) =
      t.ContinueWith(fun (t: Task<_>) -> f t.Result)
