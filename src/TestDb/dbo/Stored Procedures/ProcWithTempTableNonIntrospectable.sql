CREATE PROCEDURE [dbo].[ProcWithTempTableNonIntrospectable]
AS

-- The goal of this sproc is to fail both sp_describe_first_result_set and SET FMTONLY ON, requiring execution.

-- Note that SQL server seems to cache the result set in a way that makes SET FMTONLY ON work after the first execution.
-- Re-deploy with meaningful changes to "reset" this cache and force execution again.


CREATE TABLE #foobar
(
  Col1 int NOT NULL PRIMARY KEY,
  Col2 nvarchar(42) NOT NULL
);

MERGE INTO #foobar AS T
USING
(SELECT Col1, Col2 FROM #tempTable) AS S
ON (T.Col1 = S.Col1)
WHEN MATCHED THEN UPDATE SET T.Col2 = S.Col2
;
