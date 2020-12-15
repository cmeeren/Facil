DECLARE @_col1Filter NVARCHAR(42) = @col1Filter

DECLARE @sql NVARCHAR(MAX) =
  'SELECT * FROM dbo.Table1 WHERE TableCol1 = @col1Filter'

DECLARE @paramList NVARCHAR(MAX) =
  '@col1Filter NVARCHAR(42)'

EXEC sp_executesql @sql, @paramList, @_col1Filter

WITH RESULT SETS
(
  (
    TableCol1 NVARCHAR (42) NOT NULL,
    TableCol2 INT NULL
  )
)
