CREATE PROCEDURE [dbo].[ProcColumnInheritance]
AS

DECLARE @hid HIERARCHYID

SELECT
  Foo = 1,
  Bar = 2,
  Baz = @hid
