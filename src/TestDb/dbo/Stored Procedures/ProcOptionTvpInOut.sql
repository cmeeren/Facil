CREATE PROCEDURE [dbo].[ProcOptionTvpInOut]
  @tvp dbo.MultiColNull READONLY
AS

SELECT * FROM @tvp
