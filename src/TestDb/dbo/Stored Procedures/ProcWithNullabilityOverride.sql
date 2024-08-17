CREATE PROCEDURE [dbo].[ProcWithNullabilityOverride]
AS

SELECT
  [NotNull1],
  [NotNull2],
  [Null1],
  [Null2]
FROM
  [TableWithNullabilityOverride]
