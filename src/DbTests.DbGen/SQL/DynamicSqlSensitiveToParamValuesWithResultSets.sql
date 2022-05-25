DECLARE @sql NVARCHAR(MAX) = 'SELECT * FROM dbo.Table1 ORDER BY ' + @orderBy
EXEC sp_executesql @sql, N''

WITH RESULT SETS (([TableCol1] NVARCHAR (42) NOT NULL, [TableCol2] INT NULL))
