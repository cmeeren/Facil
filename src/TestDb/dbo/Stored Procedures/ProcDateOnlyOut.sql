CREATE PROCEDURE [dbo].[ProcDateOnlyOut]
  @dateParam DATE,
  @dateOut DATE = NULL OUTPUT
AS

SET @dateOut = @dateParam
SELECT DateResult = @dateParam
