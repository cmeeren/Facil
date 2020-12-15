CREATE PROCEDURE [dbo].[ProcWithMultipleColumnsAndSimpleNonDefaultParamsExtended]
  @foo INT,
  @bar NVARCHAR(50)
AS

-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

SELECT
  Foo = @foo,
  Bar = @bar
