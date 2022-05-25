DECLARE @sql NVARCHAR(MAX) = 'SELECT * FROM dbo.Table1'

IF 1 = 1 SET @sql += ' ORDER BY TableCol1'
IF 1 = 0 SET @sql += ' ORDER BY TableCol2'

EXEC sp_executesql @sql, N''
