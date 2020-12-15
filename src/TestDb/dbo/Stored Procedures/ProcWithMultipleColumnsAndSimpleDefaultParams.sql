CREATE PROCEDURE ProcWithMultipleColumnsAndSimpleDefaultParams
  @foo INT = 123,
  @bar NVARCHAR(50) = 'test'
AS

SELECT
  Foo = @foo,
  Bar = @bar
