DECLARE @sql NVARCHAR(MAX) =
    CASE WHEN @countOnly = 1 THEN N'SELECT COUNT(*)' ELSE N'SELECT Id, Name' END;

{{include "Predicates.sql"}}

EXEC sp_executesql @sql, N'@minId INT', @minId = @minId;
