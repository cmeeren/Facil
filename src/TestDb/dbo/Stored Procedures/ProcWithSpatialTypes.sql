CREATE PROCEDURE [dbo].[ProcWithSpatialTypes]
  @shape GEOMETRY,
  @nullableShape GEOMETRY = NULL,
  @location GEOGRAPHY,
  @nullableLocation GEOGRAPHY = NULL
AS

SELECT
  [shape] = @shape,
  [nullableShape] = @nullableShape,
  [location] = @location,
  [nullableLocation] = @nullableLocation
