using System;
using System.Collections.Generic;
using System.Data;

namespace Facil.Runtime.CSharp
{
  internal class TempTableLoader : IDataReader
  {
    private readonly IEnumerator<object[]> _enumerator;

    public TempTableLoader(int fieldCount, IEnumerable<object[]> items)
    {
      FieldCount = fieldCount;
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

    public object this[int i] => throw new NotImplementedException();

    public object this[string name] => throw new NotImplementedException();

    public int Depth => throw new NotImplementedException();

    public bool IsClosed => throw new NotImplementedException();

    public int RecordsAffected => throw new NotImplementedException();


    public void Close()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }

    public int GetOrdinal(string name)
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }

    public bool IsDBNull(int i)
    {
      throw new NotImplementedException();
    }
  }
}
