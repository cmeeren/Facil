DECLARE @sql NVARCHAR(MAX) = '
INSERT INTO dbo.DesignTimeExecuteTest VALUES (NEWID())
SELECT * FROM dbo.DesignTimeExecuteTest
'

IF 1 = 0 SET @sql += 'not used, but will force execute to get columns'

EXEC sp_executesql @sql, N''
