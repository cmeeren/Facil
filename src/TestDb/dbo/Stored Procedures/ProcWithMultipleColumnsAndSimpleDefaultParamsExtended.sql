CREATE PROCEDURE [dbo].[ProcWithMultipleColumnsAndSimpleDefaultParamsExtended]
  @foo INT = 123,
  @bar NVARCHAR(50) = 'test'
AS

-- Use an extended sproc to force sp_describe_first_result_set to fail
-- and parser to fallback to SET FMTONLY ON
EXEC sp_getapplock 'TestLock', 'Update'

SELECT
  Foo = @foo,
  Bar = @bar
