CREATE PROCEDURE [dbo].[ProcWithMaxLengthTypesFromTvp]
  @tvp dbo.MaxLengthTypes READONLY
AS

SELECT * FROM @tvp
