CREATE PROCEDURE [dbo].[ProcWithHierarchyIdFromTvp]
  @params dbo.HierarchyIdTableType READONLY
AS

SELECT * FROM @params
