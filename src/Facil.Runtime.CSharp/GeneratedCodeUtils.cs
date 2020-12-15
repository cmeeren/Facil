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
        public static async Task<List<T>> ExecuteQueryEagerAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, CancellationToken ct)
        {
            if (existingConn is not null)
            {
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

        public static async Task<List<T>> ExecuteQueryEagerAsyncWithSyncRead<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, CancellationToken ct)
        {
            if (existingConn is not null)
            {
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

        public static List<T> ExecuteQueryEager<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem)
        {
            if (existingConn is not null)
            {
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
#if !NETSTANDARD2_0
        public static async IAsyncEnumerable<T> ExecuteQueryLazyAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, [EnumeratorCancellation] CancellationToken ct)
        {
            if (existingConn is not null)
            {
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

        public static async IAsyncEnumerable<T> ExecuteQueryLazyAsyncWithSyncRead<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, [EnumeratorCancellation] CancellationToken ct)
        {
            if (existingConn is not null)
            {
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
        #endif

        public static IEnumerable<T> ExecuteQueryLazy<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem)
        {
            if (existingConn is not null)
            {
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

        public static async Task<FSharpOption<T>> ExecuteQuerySingleAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, CancellationToken ct)
        {
            if (existingConn is not null)
            {
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

        public static async Task<FSharpValueOption<T>> ExecuteQuerySingleAsyncVoption<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, CancellationToken ct)
        {
            if (existingConn is not null)
            {
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

        public static FSharpOption<T> ExecuteQuerySingle<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem)
        {
            if (existingConn is not null)
            {
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

        public static FSharpValueOption<T> ExecuteQuerySingleVoption<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem)
        {
            if (existingConn is not null)
            {
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

        public static async Task<int> ExecuteNonQueryAsync<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, CancellationToken ct)
        {
            if (existingConn is not null)
            {
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
                using var cmd = conn.CreateCommand();
                configureCmd(cmd);
                return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
            else
            {
                throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
            }
        }

        public static int ExecuteNonQuery<T>(SqlConnection? existingConn, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd)
        {
            if (existingConn is not null)
            {
                using var cmd = existingConn.CreateCommand();
                configureCmd(cmd);
                return cmd.ExecuteNonQuery();
            }
            else if (connStr is not null)
            {
                using var conn = new SqlConnection(connStr);
                configureNewConn(conn);
                conn.Open();
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
