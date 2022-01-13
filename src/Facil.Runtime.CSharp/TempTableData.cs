using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Facil.Runtime.CSharp
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TempTableData
    {
        public TempTableData(string destinationTableName, string definition, IEnumerable<object[]> data, int numFields, Action<SqlBulkCopy> configureBulkCopy)
        {
            DestinationTableName = destinationTableName;
            Definition = definition;
            Data = data;
            NumFields = numFields;
            ConfigureBulkCopy = configureBulkCopy;
        }

        public string DestinationTableName { get; }
        public string Definition { get; }
        public IEnumerable<object[]> Data { get; }
        public int NumFields { get; }
        public Action<SqlBulkCopy> ConfigureBulkCopy { get; }
    }
}
