CREATE PROCEDURE [dbo].[ProcWithAllTypesFromTvpNonNullExtended]
  @params dbo.AllTypesNonNull READONLY
AS

-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

SELECT * FROM @params
