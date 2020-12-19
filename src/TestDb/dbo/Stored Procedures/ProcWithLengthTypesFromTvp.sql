CREATE PROCEDURE [dbo].[ProcWithLengthTypesFromTvp]
  @tvp dbo.LengthTypes READONLY
AS

SELECT * FROM @tvp
