CREATE TABLE [dbo].[TableWithHierarchyId]
(
  [key] INT NOT NULL PRIMARY KEY,
  [path] HIERARCHYID NOT NULL,
  [nullablePath] HIERARCHYID NULL
)
