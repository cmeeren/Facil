CREATE PROCEDURE [dbo].[ProcWithResultsAndRetVal]
  @baseRetVal INT
AS

SELECT Foo = 1, Bar = 2

RETURN @baseRetVal + 1
