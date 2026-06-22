CREATE TYPE [dbo].[HierarchyIdTableType] AS TABLE
(
  [path] HIERARCHYID NOT NULL,
  [nullablePath] HIERARCHYID NULL
)
