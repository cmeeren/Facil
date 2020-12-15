CREATE PROCEDURE [dbo].[ProcWithNonFSharpFriendlyNames]
AS

SELECT
  [This is the first column] = 'foo',
  [!"#%&/()=?] = 2
