CREATE PROCEDURE [dbo].[ProcWithAllTypesFromTvpNonNull]
  @params dbo.AllTypesNonNull READONLY
AS

SELECT * FROM @params
