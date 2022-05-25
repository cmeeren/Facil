DECLARE @sql NVARCHAR(MAX) = 'SELECT * FROM dbo.Table1'

IF @splitParam IS NOT NULL
  SET @sql += '
  WHERE TableCol1 IN (SELECT VALUE FROM STRING_SPLIT(@splitParam, '',''))
'

DECLARE @paramList NVARCHAR(MAX) =
  '@splitParam NVARCHAR(100)'

EXEC sp_executesql @sql, @paramList, @splitParam
