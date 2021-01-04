CREATE PROCEDURE [dbo].[ProcWithSkippedUnsupportedColumn]
AS

DECLARE @hid HIERARCHYID

SELECT
  SupportedCol1 = 1,
  SupportedCol2 = 'test',
  UnsupportedCol = @hid
