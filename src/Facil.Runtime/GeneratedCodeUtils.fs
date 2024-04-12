namespace Facil.Runtime

open System
open System.ComponentModel
open System.Threading
open System.Threading.Tasks
open Microsoft.Data.SqlClient
open Microsoft.Data.SqlClient.Server

open type Facil.Runtime.CSharp.GeneratedCodeUtils


type FacilReaderDisposer
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    (conn: SqlConnection, cmd: SqlCommand, reader: SqlDataReader) =
    let disposeIfNotNull (d: IDisposable) =
        if not (isNull d) then
            d.Dispose()

    /// Gets the wrapped SqlDataReader. Do not dispose this directly; instead, dispose the FacilReaderDisposer object.
    member _.Reader = reader

    interface IDisposable with
        member this.Dispose() =
            disposeIfNotNull reader
            disposeIfNotNull cmd
            disposeIfNotNull conn


[<EditorBrowsable(EditorBrowsableState.Never)>]
module GeneratedCodeUtils =


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline handleSqlExceptionCancellation (ct: CancellationToken) (t: Task<'a>) : Task<'a> =
        backgroundTask {
            try
                return! t
            with :? SqlException as ex when
                ct.IsCancellationRequested && ex.Number = 0 && ex.State = 0uy && ex.Class = 11uy ->
                return raise (OperationCanceledException(null, ex, ct))
        }


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryEagerAsync
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        : Task<ResizeArray<_>> =
        ExecuteQueryEagerAsync(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )
        |> handleSqlExceptionCancellation ct

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryEagerAsyncWithSyncRead
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        : Task<ResizeArray<_>> =
        ExecuteQueryEagerAsyncWithSyncRead(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )
        |> handleSqlExceptionCancellation ct

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryEager
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        : ResizeArray<_> =
        ExecuteQueryEager(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData
        )

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryLazyAsync
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        =
        ExecuteQueryLazyAsync(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryLazyAsyncWithSyncRead
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        =
        ExecuteQueryLazyAsyncWithSyncRead(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQueryLazy connStr conn tran configureConn configureCmd initOrdinals getItem tempTableData =
        ExecuteQueryLazy(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData
        )

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQuerySingleAsync
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        : Task<_ option> =
        ExecuteQuerySingleAsync(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )
        |> handleSqlExceptionCancellation ct

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQuerySingleAsyncVoption
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        ct
        : Task<_ voption> =
        ExecuteQuerySingleAsyncVoption(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData,
            ct
        )
        |> handleSqlExceptionCancellation ct

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQuerySingle
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        : _ option =
        ExecuteQuerySingle(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData
        )

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeQuerySingleVoption
        connStr
        conn
        tran
        configureConn
        configureCmd
        initOrdinals
        getItem
        tempTableData
        : _ voption =
        ExecuteQuerySingleVoption(
            conn,
            tran,
            connStr,
            Action<_> configureConn,
            Action<_> configureCmd,
            Action<_> initOrdinals,
            Func<_, _> getItem,
            tempTableData
        )


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeReaderAsync
        connStr
        conn
        tran
        configureConn
        configureCmd
        tempTableData
        ct
        : Task<FacilReaderDisposer> =
        task {
            let! struct (conn, cmd, reader) =
                ExecuteReaderAsync(
                    conn,
                    tran,
                    connStr,
                    Action<_> configureConn,
                    Action<_> configureCmd,
                    tempTableData,
                    false,
                    ct
                )

            return new FacilReaderDisposer(conn, cmd, reader)
        }
        |> handleSqlExceptionCancellation ct


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeReaderSingleAsync
        connStr
        conn
        tran
        configureConn
        configureCmd
        tempTableData
        ct
        : Task<FacilReaderDisposer> =
        task {
            let! struct (conn, cmd, reader) =
                ExecuteReaderAsync(
                    conn,
                    tran,
                    connStr,
                    Action<_> configureConn,
                    Action<_> configureCmd,
                    tempTableData,
                    true,
                    ct
                )

            return new FacilReaderDisposer(conn, cmd, reader)
        }
        |> handleSqlExceptionCancellation ct


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeReader connStr conn tran configureConn configureCmd tempTableData : FacilReaderDisposer =
        let struct (conn, cmd, reader) =
            ExecuteReader(conn, tran, connStr, Action<_> configureConn, Action<_> configureCmd, tempTableData, false)

        new FacilReaderDisposer(conn, cmd, reader)


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeReaderSingle connStr conn tran configureConn configureCmd tempTableData : FacilReaderDisposer =
        let struct (conn, cmd, reader) =
            ExecuteReader(conn, tran, connStr, Action<_> configureConn, Action<_> configureCmd, tempTableData, true)

        new FacilReaderDisposer(conn, cmd, reader)

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeNonQueryAsync connStr conn tran configureConn configureCmd tempTableData ct =
        ExecuteNonQueryAsync(conn, tran, connStr, Action<_> configureConn, Action<_> configureCmd, tempTableData, ct)
        |> handleSqlExceptionCancellation ct

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let inline executeNonQuery connStr conn tran configureConn configureCmd tempTableData =
        ExecuteNonQuery(conn, tran, connStr, Action<_> configureConn, Action<_> configureCmd, tempTableData)


    [<EditorBrowsable(EditorBrowsableState.Never)>]
    let boxNullIfEmpty (seq: #seq<'a>) =
        if Seq.isEmpty seq then null else box seq


    type SqlDataRecord with

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        member inline this.WithValues([<ParamArray>] values: obj[]) =
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
