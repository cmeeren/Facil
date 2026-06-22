CREATE PROCEDURE [dbo].[ProcWithSpatialTypesFromTvp]
  @params dbo.SpatialTypesTableType READONLY
AS

SELECT * FROM @params
