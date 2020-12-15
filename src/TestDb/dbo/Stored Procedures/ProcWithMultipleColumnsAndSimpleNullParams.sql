CREATE PROCEDURE ProcWithMultipleColumnsAndSimpleNullParams
  @foo INT = NULL,
  @bar NVARCHAR(50) = NULL
AS

SELECT
  Foo = @foo,
  Bar = @bar
