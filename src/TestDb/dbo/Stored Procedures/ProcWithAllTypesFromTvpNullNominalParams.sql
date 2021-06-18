CREATE PROCEDURE [dbo].[ProcWithAllTypesFromTvpNullNominalParams]
  @params dbo.AllTypesNull READONLY
AS

SELECT * FROM @params
