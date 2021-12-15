DECLARE @sql NVARCHAR(MAX) = '
INSERT INTO dbo.DesignTimeExecuteTest VALUES (NEWID())
SELECT * FROM dbo.DesignTimeExecuteTest
'

EXEC sp_executesql @sql, N''
