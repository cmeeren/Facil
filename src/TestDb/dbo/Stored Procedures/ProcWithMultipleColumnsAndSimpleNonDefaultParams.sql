CREATE PROCEDURE ProcWithMultipleColumnsAndSimpleNonDefaultParams
  @foo INT,
  @bar NVARCHAR(50)
AS

SELECT
  Foo = @foo,
  Bar = @bar
