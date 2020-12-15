CREATE PROCEDURE [dbo].[ProcWithSpecialCasingExtended]
  @PARAM1 INT,
  @Param2 INT,
  @param3 INT
AS

-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

SELECT
  COL1 = @PARAM1,
  Col2 = @Param2,
  col3 = @param3
