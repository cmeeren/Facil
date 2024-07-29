using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.FSharp.Core;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Facil.Runtime.CSharp
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public static class GeneratedCodeUtils
  {
    private static async Task LoadTempTablesAsync(SqlConnection conn, IEnumerable<TempTableData> tempTableData,
      SqlTransaction? tran, CancellationToken ct)
    {
      foreach (var data in tempTableData)
      {
        using var cmd = conn.CreateCommand();
        if (tran != null) cmd.Transaction = tran;
        // Note: If there is ever a need for letting users configure the command,
        // do not use the configureCmd parameter passed to methods on this class,
        // which also sets any parameters.
        cmd.CommandText = data.Definition;
        await cmd.ExecuteNonQueryAsync(ct);

        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
          { DestinationTableName = data.DestinationTableName };
        data.ConfigureBulkCopy(bulkCopy);
        var reader = new TempTableLoader(data.NumFields, data.Data);
        await bulkCopy.WriteToServerAsync(reader, ct);
      }
    }

    private static void LoadTempTables(SqlConnection conn, IEnumerable<TempTableData> tempTableData,
      SqlTransaction? tran)
    {
      foreach (var data in tempTableData)
      {
        using var cmd = conn.CreateCommand();
        if (tran != null) cmd.Transaction = tran;
        // Note: If there is ever a need for letting users configure the command,
        // do not use the configureCmd parameter passed to methods on this class,
        // which also sets any parameters.
        cmd.CommandText = data.Definition;
        cmd.ExecuteNonQuery();

        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
          { DestinationTableName = data.DestinationTableName };
        data.ConfigureBulkCopy(bulkCopy);
        var reader = new TempTableLoader(data.NumFields, data.Data);
        bulkCopy.WriteToServer(reader);
      }
    }

    private static void ConfigureCommand(SqlCommand cmd, SqlTransaction? tran, Action<SqlCommand> configureCmd)
    {
      configureCmd(cmd);
      if (tran == null) return;
      if (cmd.Transaction != null)
        throw new Exception(
          "Transaction must not be set both WithConnection and ConfigureCommand; prefer WithConnection");

      cmd.Transaction = tran;
    }

    public static async Task<List<T>> ExecuteQueryEagerAsync<T>(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        while (await reader.ReadAsync(ct).ConfigureAwait(false)) list.Add(getItem(reader));
        return list;
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        while (await reader.ReadAsync(ct).ConfigureAwait(false)) list.Add(getItem(reader));
        return list;
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async Task<List<T>> ExecuteQueryEagerAsyncWithSyncRead<T>(SqlConnection? existingConn,
      SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        while (reader.Read()) list.Add(getItem(reader));
        return list;
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        while (reader.Read()) list.Add(getItem(reader));
        return list;
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static List<T> ExecuteQueryEager<T>(SqlConnection? existingConn, SqlTransaction? tran, string? connStr,
      Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals,
      Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
    {
      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        while (reader.Read()) list.Add(getItem(reader));
        return list;
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        conn.Open();
        LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        var list = new List<T>();
        if (!reader.HasRows) return list;
        initOrdinals(reader);
        while (reader.Read()) list.Add(getItem(reader));
        return list;
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async IAsyncEnumerable<T> ExecuteQueryLazyAsync<T>(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      [EnumeratorCancellation] CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        while (await reader.ReadAsync(ct).ConfigureAwait(false)) yield return getItem(reader);
      }
      else if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        while (await reader.ReadAsync(ct).ConfigureAwait(false)) yield return getItem(reader);
      }
      else
      {
        throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
      }
    }

    public static async IAsyncEnumerable<T> ExecuteQueryLazyAsyncWithSyncRead<T>(SqlConnection? existingConn,
      SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      [EnumeratorCancellation] CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        while (reader.Read()) yield return getItem(reader);
      }
      else if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult, ct).ConfigureAwait(false);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        while (reader.Read()) yield return getItem(reader);
      }
      else
      {
        throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
      }
    }

    public static IEnumerable<T> ExecuteQueryLazy<T>(SqlConnection? existingConn, SqlTransaction? tran, string? connStr,
      Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, Action<SqlDataReader> initOrdinals,
      Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
    {
      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        while (reader.Read()) yield return getItem(reader);
      }
      else if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        conn.Open();
        LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        if (!reader.HasRows) yield break;
        initOrdinals(reader);
        while (reader.Read()) yield return getItem(reader);
      }
      else
      {
        throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
      }
    }

    public static async Task<FSharpOption<T>> ExecuteQuerySingleAsync<T>(SqlConnection? existingConn,
      SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd
          .ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
        if (!await reader.ReadAsync(ct).ConfigureAwait(false)) return FSharpOption<T>.None;
        initOrdinals(reader);
        return FSharpOption<T>.Some(getItem(reader));
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd
          .ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
        if (!await reader.ReadAsync(ct).ConfigureAwait(false)) return FSharpOption<T>.None;
        initOrdinals(reader);
        return FSharpOption<T>.Some(getItem(reader));
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async Task<FSharpValueOption<T>> ExecuteQuerySingleAsyncVoption<T>(SqlConnection? existingConn,
      SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = await cmd
          .ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
        if (!await reader.ReadAsync(ct).ConfigureAwait(false)) return FSharpValueOption<T>.None;
        initOrdinals(reader);
        return FSharpValueOption<T>.Some(getItem(reader));
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct).ConfigureAwait(false);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = await cmd
          .ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow, ct).ConfigureAwait(false);
        if (!await reader.ReadAsync(ct).ConfigureAwait(false)) return FSharpValueOption<T>.None;
        initOrdinals(reader);
        return FSharpValueOption<T>.Some(getItem(reader));
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static FSharpOption<T> ExecuteQuerySingle<T>(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
    {
      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
        if (!reader.Read()) return FSharpOption<T>.None;
        initOrdinals(reader);
        return FSharpOption<T>.Some(getItem(reader));
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        conn.Open();
        LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
        if (!reader.Read()) return FSharpOption<T>.None;
        initOrdinals(reader);
        return FSharpOption<T>.Some(getItem(reader));
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static FSharpValueOption<T> ExecuteQuerySingleVoption<T>(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData)
    {
      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
        if (!reader.Read()) return FSharpValueOption<T>.None;
        initOrdinals(reader);
        return FSharpValueOption<T>.Some(getItem(reader));
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        conn.Open();
        LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
        if (!reader.Read()) return FSharpValueOption<T>.None;
        initOrdinals(reader);
        return FSharpValueOption<T>.Some(getItem(reader));
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async Task<(SqlConnection?, SqlCommand, SqlDataReader)> ExecuteReaderAsync(
      SqlConnection? existingConn, SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn,
      Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData, bool singleRow, CancellationToken ct)
    {
      var commandBehavior =
        singleRow ? CommandBehavior.SingleResult | CommandBehavior.SingleRow : CommandBehavior.SingleResult;

      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        var cmd = existingConn.CreateCommand();
        try
        {
          ConfigureCommand(cmd, tran, configureCmd);
          var reader = await cmd.ExecuteReaderAsync(commandBehavior, ct).ConfigureAwait(false);
          return (null, cmd, reader);
        }
        catch (Exception)
        {
          cmd.Dispose();
          throw;
        }
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        var conn = new SqlConnection(connStr);
        try
        {
          configureNewConn(conn);
          await conn.OpenAsync(ct).ConfigureAwait(false);
          await LoadTempTablesAsync(conn, tempTableData, tran, ct);
          var cmd = conn.CreateCommand();
          try
          {
            configureCmd(cmd);
            var reader = await cmd.ExecuteReaderAsync(commandBehavior, ct).ConfigureAwait(false);
            return (conn, cmd, reader);
          }
          catch (Exception)
          {
            cmd.Dispose();
            throw;
          }
        }
        catch (Exception)
        {
          conn.Dispose();
          throw;
        }
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static (SqlConnection?, SqlCommand, SqlDataReader) ExecuteReader(
      SqlConnection? existingConn, SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn,
      Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData, bool singleRow)
    {
      var commandBehavior =
        singleRow ? CommandBehavior.SingleResult | CommandBehavior.SingleRow : CommandBehavior.SingleResult;

      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        var cmd = existingConn.CreateCommand();
        try
        {
          ConfigureCommand(cmd, tran, configureCmd);
          var reader = cmd.ExecuteReader(commandBehavior);
          return (null, cmd, reader);
        }
        catch (Exception)
        {
          cmd.Dispose();
          throw;
        }
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        var conn = new SqlConnection(connStr);
        try
        {
          configureNewConn(conn);
          conn.Open();
          LoadTempTables(conn, tempTableData, tran);
          var cmd = conn.CreateCommand();
          try
          {
            configureCmd(cmd);
            var reader = cmd.ExecuteReader(commandBehavior);
            return (null, cmd, reader);
          }
          catch (Exception)
          {
            cmd.Dispose();
            throw;
          }
        }
        catch (Exception)
        {
          conn.Dispose();
          throw;
        }
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async Task<int> ExecuteNonQueryAsync(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      IEnumerable<TempTableData> tempTableData, CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        await conn.OpenAsync(ct);
        await LoadTempTablesAsync(conn, tempTableData, tran, ct);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static int ExecuteNonQuery(SqlConnection? existingConn, SqlTransaction? tran, string? connStr,
      Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData)
    {
      if (existingConn is not null)
      {
        LoadTempTables(existingConn, tempTableData, tran);
        using var cmd = existingConn.CreateCommand();
        ConfigureCommand(cmd, tran, configureCmd);
        return cmd.ExecuteNonQuery();
      }

      // ReSharper disable once InvertIf
      if (connStr is not null)
      {
        using var conn = new SqlConnection(connStr);
        configureNewConn(conn);
        conn.Open();
        LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        return cmd.ExecuteNonQuery();
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }
  }
}
