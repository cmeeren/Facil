using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Facil.Runtime.CSharp
{
  // ReSharper disable once ClassNeverInstantiated.Global
  public class TempTableData
  {
    public TempTableData(string destinationTableName, string definition, IEnumerable<object[]> data, int numFields,
      Action<SqlBulkCopy> configureBulkCopy)
      : this(destinationTableName, definition, data, Array.Empty<string>(), numFields, configureBulkCopy)
    {
    }

    public TempTableData(string destinationTableName, string definition, IEnumerable<object[]> data, string[] columnNames,
      int numFields,
      Action<SqlBulkCopy> configureBulkCopy)
    {
      if (columnNames.Length != 0 && columnNames.Length != numFields)
        throw new ArgumentException("Column names must match the field count", nameof(columnNames));

      DestinationTableName = destinationTableName;
      Definition = definition;
      Data = data;
      ColumnNames = columnNames;
      NumFields = numFields;
      ConfigureBulkCopy = configureBulkCopy;
    }

    public string DestinationTableName { get; }
    public string Definition { get; }
    public IEnumerable<object[]> Data { get; }
    public string[] ColumnNames { get; }
    public int NumFields { get; }
    public Action<SqlBulkCopy> ConfigureBulkCopy { get; }
  }
}
