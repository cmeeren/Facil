CREATE TABLE [dbo].[TableWithSkippedUnsupportedColumn]
(
  [SupportedCol1] NVARCHAR (42) NOT NULL,
  [SupportedCol2] INT NOT NULL,
  [UnsupportedCol] HIERARCHYID NOT NULL
)
