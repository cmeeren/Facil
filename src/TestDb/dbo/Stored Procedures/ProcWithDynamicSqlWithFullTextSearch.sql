CREATE PROCEDURE [dbo].[ProcWithDynamicSqlWithFullTextSearch]
  @fullTextPredicate NVARCHAR(1000)
AS


DECLARE @sql NVARCHAR(MAX) = '
SELECT
  TableWithFullTextCatalog.Col1
FROM
  dbo.TableWithFullTextCatalog
'

IF @fullTextPredicate IS NOT NULL
  SET @sql += '
INNER JOIN
  CONTAINSTABLE(dbo.TableWithFullTextCatalog, *, @fullTextPredicate, LANGUAGE 0)
  AS FullTextSearch
  ON FullTextSearch.[KEY] = TableWithFullTextCatalog.Col1
'

SET @sql += '
ORDER BY FullTextSearch.RANK DESC
'

DECLARE @paramList NVARCHAR(MAX) = '
@fullTextPredicate NVARCHAR(1000)
'

EXEC sp_executesql
  @sql,
  @paramList,
  @fullTextPredicate
