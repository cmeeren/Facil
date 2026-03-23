using System;
using System.Collections.Generic;
using System.Data;

namespace Facil.Runtime.CSharp
{
  internal class TempTableLoader : IDataReader
  {
    private readonly string[] _columnNames;
    private readonly Dictionary<string, int> _ordinalsByName;
    private readonly IEnumerator<object[]> _enumerator;
    private bool _isClosed;

    public TempTableLoader(int fieldCount, IEnumerable<object[]> items)
      : this(Array.Empty<string>(), fieldCount, items)
    {
    }

    public TempTableLoader(string[] columnNames, int fieldCount, IEnumerable<object[]> items)
    {
      if (columnNames.Length != 0 && columnNames.Length != fieldCount)
        throw new ArgumentException("Column names must match the field count", nameof(columnNames));

      FieldCount = fieldCount;
      _columnNames = columnNames;
      _ordinalsByName = new Dictionary<string, int>(columnNames.Length, StringComparer.OrdinalIgnoreCase);

      for (var i = 0; i < columnNames.Length; i++)
        _ordinalsByName.Add(columnNames[i], i);

      _enumerator = items.GetEnumerator();
    }

    public int FieldCount { get; }

    public bool Read()
    {
      return _enumerator.MoveNext();
    }

    public object GetValue(int i)
    {
      return _enumerator.Current[i];
    }

    public bool NextResult()
    {
      throw new NotImplementedException();
    }

    public object this[int i] => GetValue(i);

    public object this[string name] => GetValue(GetOrdinal(name));

    public int Depth => 0;

    public bool IsClosed => _isClosed;

    public int RecordsAffected => -1;


    public void Close()
    {
      Dispose();
    }

    public void Dispose()
    {
      if (_isClosed) return;
      _isClosed = true;
      _enumerator.Dispose();
    }

    public bool GetBoolean(int i)
    {
      throw new NotImplementedException();
    }

    public byte GetByte(int i)
    {
      throw new NotImplementedException();
    }

    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length)
    {
      throw new NotImplementedException();
    }

    public char GetChar(int i)
    {
      throw new NotImplementedException();
    }

    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length)
    {
      throw new NotImplementedException();
    }

    public IDataReader GetData(int i)
    {
      throw new NotImplementedException();
    }

    public string GetDataTypeName(int i)
    {
      throw new NotImplementedException();
    }

    public DateTime GetDateTime(int i)
    {
      throw new NotImplementedException();
    }

    public decimal GetDecimal(int i)
    {
      throw new NotImplementedException();
    }

    public double GetDouble(int i)
    {
      throw new NotImplementedException();
    }

    public Type GetFieldType(int i)
    {
      throw new NotImplementedException();
    }

    public float GetFloat(int i)
    {
      throw new NotImplementedException();
    }

    public Guid GetGuid(int i)
    {
      throw new NotImplementedException();
    }

    public short GetInt16(int i)
    {
      throw new NotImplementedException();
    }

    public int GetInt32(int i)
    {
      throw new NotImplementedException();
    }

    public long GetInt64(int i)
    {
      throw new NotImplementedException();
    }

    public string GetName(int i)
    {
      if (_columnNames.Length == 0)
        throw new NotSupportedException("Source column names are not available.");

      return _columnNames[i];
    }

    public int GetOrdinal(string name)
    {
      if (_ordinalsByName.TryGetValue(name, out var ordinal))
        return ordinal;

      throw new IndexOutOfRangeException($"The column '{name}' does not exist.");
    }

    public DataTable GetSchemaTable()
    {
      throw new NotImplementedException();
    }

    public string GetString(int i)
    {
      throw new NotImplementedException();
    }

    public int GetValues(object[] values)
    {
      var current = _enumerator.Current;
      var count = Math.Min(values.Length, current.Length);
      Array.Copy(current, values, count);
      return count;
    }

    public bool IsDBNull(int i)
    {
      var value = GetValue(i);
      return value is null || value == DBNull.Value;
    }
  }
}
