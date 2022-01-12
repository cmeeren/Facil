CREATE PROCEDURE [dbo].[ProcWithTempTable]
  @param INT
AS

SELECT * FROM #tempTable
