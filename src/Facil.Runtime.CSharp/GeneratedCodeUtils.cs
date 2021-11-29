using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.FSharp.Core;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Facil.Runtime.CSharp
{

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GeneratedCodeUtils
    {
        private static async Task LoadTempTablesAsync(SqlConnection conn, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            foreach (var data in tempTableData)
            {
                using var cmd = conn.CreateCommand();
                // Note: If there is ever a need for letting users configure the command,
                // do not use the configureCmd parameter passed to methods on this class,
                // which also sets any parameters.
                cmd.CommandText = data.Definition;
                await cmd.ExecuteNonQueryAsync(ct);

                using var bulkCopy = new SqlBulkCopy(conn) { DestinationTableName = data.DestinationTableName };
                data.ConfigureBulkCopy(bulkCopy);
                var reader = new TempTableLoader(data.NumFields, data.Data);
                await bulkCopy.WriteToServerAsync(reader, ct);
            }
        }

        private static void LoadTempTables(SqlConnection conn, IEnumerable<TempTableData> tempTableData)
        {
            foreach (var data in tempTableData)
            {
                using var cmd = conn.CreateCommand();
                // Note: If there is ever a need for letting users configure the command,
                // do not use the configureCmd parameter passed to methods on this class,
                // which also sets any parameters.
                cmd.CommandText = data.Definition;
                cmd.ExecuteNonQuery();

                using var bulkCopy = new SqlBulkCopy(conn) { DestinationTableName = data.DestinationTableName };
                data.ConfigureBulkCopy(bulkCopy);
                var reader = new TempTableLoader(data.NumFields, data.Data);
                bulkCopy.WriteToServer(reader);
            }
        }

        public static async Task<List<T>> ExecuteQueryEagerAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                var list = new List<T>();
                if (reader.HasRows) {
                    initOrdinals(reader);
                    while (await reader.ReadAsync(ct).ConfigureAwait(false))
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                var list = new List<T>();
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (await reader.ReadAsync(ct).ConfigureAwait(false))
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async Task<List<T>> ExecuteQueryEagerAsyncWithSyncRead<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                var list = new List<T>();
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                var list = new List<T>();
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static List<T> ExecuteQueryEager<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
        {
            if (existingConn is not null)
            {
                LoadTempTables(existingConn, tempTableData);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                var list = new List<T>();
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                LoadTempTables(conn, tempTableData);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                var list = new List<T>();
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        list.Add(getItem(reader));
                    }
                }
                return list;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async IAsyncEnumerable<T> ExecuteQueryLazyAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, [EnumeratorCancellation] CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (await reader.ReadAsync(ct).ConfigureAwait(false))
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (await reader.ReadAsync(ct).ConfigureAwait(false))
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async IAsyncEnumerable<T> ExecuteQueryLazyAsyncWithSyncRead<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, [EnumeratorCancellation] CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static IEnumerable<T> ExecuteQueryLazy<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
        {
            if (existingConn is not null)
            {
                LoadTempTables(existingConn, tempTableData);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                LoadTempTables(conn, tempTableData);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (reader.HasRows)
                {
                    initOrdinals(reader);
                    while (reader.Read())
                    {
                        yield return getItem(reader);
                    }
                }
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async Task<FSharpOption<T>> ExecuteQuerySingleAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
                if (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    initOrdinals(reader);
                    return FSharpOption<T>.Some(getItem(reader));
                }
                return FSharpOption<T>.None;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
                if (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    initOrdinals(reader);
                    return FSharpOption<T>.Some(getItem(reader));
                }
                return FSharpOption<T>.None;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async Task<FSharpValueOption<T>> ExecuteQuerySingleAsyncVoption<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
                if (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    initOrdinals(reader);
                    return FSharpValueOption<T>.Some(getItem(reader));
                }
                return FSharpValueOption<T>.None;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                await conn.OpenAsync(ct).ConfigureAwait(false);
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
                if (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    initOrdinals(reader);
                    return FSharpValueOption<T>.Some(getItem(reader));
                }
                return FSharpValueOption<T>.None;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static FSharpOption<T> ExecuteQuerySingle<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
        {
            if (existingConn is not null)
            {
                LoadTempTables(existingConn, tempTableData);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    initOrdinals(reader);
                    return FSharpOption<T>.Some(getItem(reader));
                }
                return FSharpOption<T>.None;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                LoadTempTables(conn, tempTableData);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    initOrdinals(reader);
                    return FSharpOption<T>.Some(getItem(reader));
                }
                return FSharpOption<T>.None;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static FSharpValueOption<T> ExecuteQuerySingleVoption<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
        {
            if (existingConn is not null)
            {
                LoadTempTables(existingConn, tempTableData);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    initOrdinals(reader);
                    return FSharpValueOption<T>.Some(getItem(reader));
                }
                return FSharpValueOption<T>.None;
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                LoadTempTables(conn, tempTableData);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    initOrdinals(reader);
                    return FSharpValueOption<T>.Some(getItem(reader));
                }
                return FSharpValueOption<T>.None;
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static async Task<int> ExecuteNonQueryAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                await LoadTempTablesAsync(existingConn, tempTableData, ct);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                await LoadTempTablesAsync(conn, tempTableData, ct);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static int ExecuteNonQuery<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData)
        {
            if (existingConn is not null)
            {
                LoadTempTables(existingConn, tempTableData);
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                return cmd.ExecuteNonQuery();
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                LoadTempTables(conn, tempTableData);
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

    }
}
