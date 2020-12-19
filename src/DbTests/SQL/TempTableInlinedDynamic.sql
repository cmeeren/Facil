DECLARE @sql NVARCHAR(MAX) =
  'SELECT * FROM #tempTableInlined'

EXEC sp_executesql @sql, N''
