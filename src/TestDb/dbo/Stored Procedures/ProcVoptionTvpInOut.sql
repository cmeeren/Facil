CREATE PROCEDURE [dbo].[ProcVoptionTvpInOut]
  @tvp dbo.MultiColNullVoption READONLY
AS

SELECT * FROM @tvp
