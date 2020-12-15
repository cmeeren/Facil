CREATE PROCEDURE ProcWithSingleNullColumn
  @foo INT = NULL
AS

SELECT @foo
