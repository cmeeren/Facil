CREATE PROCEDURE [dbo].[ProcColumnInheritance]
AS

DECLARE @hid HIERARCHYID

SELECT
  Col1 = 1,
  Col2 = 2,
  Col3 = @hid,
  Col4 = @hid
