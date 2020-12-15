CREATE PROCEDURE ProcWithMultipleColumnsAndTvpParams
  @single dbo.SingleColNonNull READONLY,
  @multi dbo.MultiColNonNull READONLY
AS

SELECT * FROM @multi
