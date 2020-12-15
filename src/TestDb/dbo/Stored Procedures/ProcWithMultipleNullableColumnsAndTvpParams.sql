CREATE PROCEDURE [dbo].[ProcWithMultipleNullableColumnsAndTvpParams]
  @single dbo.SingleColNull READONLY,
  @multi dbo.MultiColNull READONLY
AS

SELECT * FROM @multi
