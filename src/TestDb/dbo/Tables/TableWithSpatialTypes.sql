CREATE TABLE [dbo].[TableWithSpatialTypes]
(
  [key] INT NOT NULL PRIMARY KEY,
  [shape] GEOMETRY NOT NULL,
  [nullableShape] GEOMETRY NULL,
  [location] GEOGRAPHY NOT NULL,
  [nullableLocation] GEOGRAPHY NULL
)
