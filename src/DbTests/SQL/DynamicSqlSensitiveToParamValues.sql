DECLARE @sql NVARCHAR(MAX) = 'SELECT * FROM dbo.Table1 ORDER BY ' + @orderBy
EXEC sp_executesql @sql, N''
