DECLARE @_col1Filter NVARCHAR(42) = @col1Filter

DECLARE @sql NVARCHAR(MAX) =
  'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

DECLARE @paramList NVARCHAR(MAX) =
  '@col1Filter NVARCHAR(42)'

EXEC sp_executesql @sql, @paramList, @_col1Filter
