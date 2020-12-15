CREATE PROCEDURE [dbo].[ProcWithAllTypesFromTvpNull]
  @params dbo.AllTypesNull READONLY
AS

SELECT * FROM @params
