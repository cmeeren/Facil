DECLARE @numCols INT = 600
DECLARE @sql NVARCHAR(MAX) = 'SELECT '

DECLARE @colNo INT = 1

WHILE (@colNo <= @numCols)
BEGIN
  SET @sql += 'Column' + CAST(@colNo AS NVARCHAR) + ' = NULL' + CASE @colNo WHEN @numCols THEN '' ELSE ', ' END
  SET @colNo += 1
END

EXEC sp_executesql @sql
