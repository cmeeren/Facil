DECLARE @sql NVARCHAR(MAX) =
  'SELECT * FROM dbo.Table1 WHERE TableCol2 IN (SELECT * FROM @tvp)'

DECLARE @paramList NVARCHAR(MAX) =
  '@tvp dbo.SingleColNonNull READONLY'

EXEC sp_executesql @sql, @paramList, @tvp
