CREATE PROCEDURE [dbo].[ProcWithRetVal]
  @baseRetVal INT
AS

RETURN @baseRetVal + 1
