CREATE TYPE [dbo].[SpatialTypesTableType] AS TABLE
(
  [shape] GEOMETRY NOT NULL,
  [nullableShape] GEOMETRY NULL,
  [location] GEOGRAPHY NOT NULL,
  [nullableLocation] GEOGRAPHY NULL
)
