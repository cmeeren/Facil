CREATE PROCEDURE [dbo].[ProcWithHierarchyId]
  @path HIERARCHYID,
  @nullablePath HIERARCHYID = NULL
AS

SELECT
  [path] = @path,
  [nullablePath] = @nullablePath
