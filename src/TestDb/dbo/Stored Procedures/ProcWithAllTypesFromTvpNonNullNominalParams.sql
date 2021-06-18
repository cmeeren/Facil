CREATE PROCEDURE [dbo].[ProcWithAllTypesFromTvpNonNullNominalParams]
  @params dbo.AllTypesNonNull READONLY
AS

SELECT * FROM @params
