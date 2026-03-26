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
    private sealed class TempTableCleanupScope : IDisposable, IAsyncDisposable
    {
      private readonly SqlConnection _conn;
      private readonly SqlTransaction? _tran;
      private List<string>? _tempTableNames;

      public TempTableCleanupScope(SqlConnection conn, SqlTransaction? tran, List<string> tempTableNames)
      {
        _conn = conn;
        _tran = tran;
        _tempTableNames = tempTableNames;
      }

      public void Dispose()
      {
        var tempTableNames = Interlocked.Exchange(ref _tempTableNames, null);

        if (tempTableNames is not null)
          DropTempTables(_conn, tempTableNames, _tran);
      }

      public ValueTask DisposeAsync()
      {
        var tempTableNames = Interlocked.Exchange(ref _tempTableNames, null);

        return tempTableNames is null
          ? default
          : new ValueTask(DropTempTablesAsync(_conn, tempTableNames, _tran));
      }
    }

    private static async Task DropTempTablesAsync(SqlConnection conn, IReadOnlyList<string> tempTableNames,
      SqlTransaction? tran)
    {
      for (var i = tempTableNames.Count - 1; i >= 0; i--)
      {
        try
        {
          using var cmd = conn.CreateCommand();
          if (tran != null) cmd.Transaction = tran;
          cmd.CommandText = $"DROP TABLE {tempTableNames[i]}";
          await cmd.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
        }
        catch
        {
        }
      }
    }

    private static void DropTempTables(SqlConnection conn, IReadOnlyList<string> tempTableNames, SqlTransaction? tran)
    {
      for (var i = tempTableNames.Count - 1; i >= 0; i--)
      {
        try
        {
          using var cmd = conn.CreateCommand();
          if (tran != null) cmd.Transaction = tran;
          cmd.CommandText = $"DROP TABLE {tempTableNames[i]}";
          cmd.ExecuteNonQuery();
        }
        catch
        {
        }
      }
    }

    private static async Task<TempTableCleanupScope> LoadTempTablesAsync(SqlConnection conn,
      IEnumerable<TempTableData> tempTableData,
      SqlTransaction? tran, CancellationToken ct)
    {
      var createdTempTableNames = new List<string>();

      try
      {
        foreach (var data in tempTableData)
        {
          using var cmd = conn.CreateCommand();
          if (tran != null) cmd.Transaction = tran;
          // Note: If there is ever a need for letting users configure the command,
          // do not use the configureCmd parameter passed to methods on this class,
          // which also sets any parameters.
          cmd.CommandText = data.Definition;
          await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
          createdTempTableNames.Add(data.DestinationTableName);

          using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
            { DestinationTableName = data.DestinationTableName };
          data.ConfigureBulkCopy(bulkCopy);
          using var reader = new TempTableLoader(data.ColumnNames, data.NumFields, data.Data);
          await bulkCopy.WriteToServerAsync(reader, ct).ConfigureAwait(false);
        }
      }
      catch
      {
        await DropTempTablesAsync(conn, createdTempTableNames, tran).ConfigureAwait(false);
        throw;
      }

      return new TempTableCleanupScope(conn, tran, createdTempTableNames);
    }

    private static TempTableCleanupScope LoadTempTables(SqlConnection conn, IEnumerable<TempTableData> tempTableData,
      SqlTransaction? tran)
    {
      var createdTempTableNames = new List<string>();

      try
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
          createdTempTableNames.Add(data.DestinationTableName);

          using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
            { DestinationTableName = data.DestinationTableName };
          data.ConfigureBulkCopy(bulkCopy);
          using var reader = new TempTableLoader(data.ColumnNames, data.NumFields, data.Data);
          bulkCopy.WriteToServer(reader);
        }
      }
      catch
      {
        DropTempTables(conn, createdTempTableNames, tran);
        throw;
      }

      return new TempTableCleanupScope(conn, tran, createdTempTableNames);
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

    public static async IAsyncEnumerable<T> HandleSqlExceptionCancellation<T>(IAsyncEnumerable<T> items,
      [EnumeratorCancellation] CancellationToken ct)
    {
      await using var e = items.GetAsyncEnumerator(ct);

      while (true)
      {
        bool hasItem;

        try
        {
          hasItem = await e.MoveNextAsync().ConfigureAwait(false);
        }
        catch (SqlException ex) when (ct.IsCancellationRequested && ex.Number == 0 && ex.State == 0 && ex.Class == 11)
        {
          throw new OperationCanceledException(null, ex, ct);
        }

        if (!hasItem)
          yield break;

        yield return e.Current;
      }
    }

    public static async Task<List<T>> ExecuteQueryEagerAsync<T>(SqlConnection? existingConn, SqlTransaction? tran,
      string? connStr, Action<SqlConnection> configureNewConn, Action<SqlCommand> configureCmd,
      Action<SqlDataReader> initOrdinals, Func<SqlDataReader, T> getItem, IEnumerable<TempTableData> tempTableData,
      CancellationToken ct)
    {
      if (existingConn is not null)
      {
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        using var tempTables = LoadTempTables(existingConn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(conn, tempTableData, tran);
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        using var tempTables = LoadTempTables(existingConn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(conn, tempTableData, tran);
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        using var tempTables = LoadTempTables(existingConn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(conn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(existingConn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        using var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow);
        if (!reader.Read()) return FSharpValueOption<T>.None;
        initOrdinals(reader);
        return FSharpValueOption<T>.Some(getItem(reader));
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }

    public static async Task<(SqlConnection?, SqlCommand, SqlDataReader, IDisposable)> ExecuteReaderAsync(
      SqlConnection? existingConn, SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn,
      Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData, bool singleRow, CancellationToken ct)
    {
      var commandBehavior =
        singleRow ? CommandBehavior.SingleResult | CommandBehavior.SingleRow : CommandBehavior.SingleResult;

      if (existingConn is not null)
      {
        var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
        var cmd = existingConn.CreateCommand();
        try
        {
          ConfigureCommand(cmd, tran, configureCmd);
          var reader = await cmd.ExecuteReaderAsync(commandBehavior, ct).ConfigureAwait(false);
          return (null, cmd, reader, tempTables);
        }
        catch (Exception)
        {
          cmd.Dispose();
          await tempTables.DisposeAsync().ConfigureAwait(false);
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
          var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
          var cmd = conn.CreateCommand();
          try
          {
            configureCmd(cmd);
            var reader = await cmd.ExecuteReaderAsync(commandBehavior, ct).ConfigureAwait(false);
            return (conn, cmd, reader, tempTables);
          }
          catch (Exception)
          {
            cmd.Dispose();
            await tempTables.DisposeAsync().ConfigureAwait(false);
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

    public static (SqlConnection?, SqlCommand, SqlDataReader, IDisposable) ExecuteReader(
      SqlConnection? existingConn, SqlTransaction? tran, string? connStr, Action<SqlConnection> configureNewConn,
      Action<SqlCommand> configureCmd, IEnumerable<TempTableData> tempTableData, bool singleRow)
    {
      var commandBehavior =
        singleRow ? CommandBehavior.SingleResult | CommandBehavior.SingleRow : CommandBehavior.SingleResult;

      if (existingConn is not null)
      {
        var tempTables = LoadTempTables(existingConn, tempTableData, tran);
        var cmd = existingConn.CreateCommand();
        try
        {
          ConfigureCommand(cmd, tran, configureCmd);
          var reader = cmd.ExecuteReader(commandBehavior);
          return (null, cmd, reader, tempTables);
        }
        catch (Exception)
        {
          cmd.Dispose();
          tempTables.Dispose();
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
          var tempTables = LoadTempTables(conn, tempTableData, tran);
          var cmd = conn.CreateCommand();
          try
          {
            configureCmd(cmd);
            var reader = cmd.ExecuteReader(commandBehavior);
            return (conn, cmd, reader, tempTables);
          }
          catch (Exception)
          {
            cmd.Dispose();
            tempTables.Dispose();
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
        await using var tempTables = await LoadTempTablesAsync(existingConn, tempTableData, tran, ct);
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
        await using var tempTables = await LoadTempTablesAsync(conn, tempTableData, tran, ct);
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
        using var tempTables = LoadTempTables(existingConn, tempTableData, tran);
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
        using var tempTables = LoadTempTables(conn, tempTableData, tran);
        using var cmd = conn.CreateCommand();
        configureCmd(cmd);
        return cmd.ExecuteNonQuery();
      }

      throw new Exception($"{nameof(existingConn)} and {nameof(connStr)} may not both be null");
    }
  }
}
