CREATE PROCEDURE [dbo].[ProcWithSpecialCasing]
  @PARAM1 INT,
  @Param2 INT,
  @param3 INT
AS

SELECT
  COL1 = @PARAM1,
  Col2 = @Param2,
  col3 = @param3
