CREATE PROCEDURE [dbo].[ProcWithMaxLengthTypes]
  @nvarchar NVARCHAR(MAX),
  @varbinary VARBINARY(MAX),
  @varchar VARCHAR(MAX)
AS

SELECT
  [nvarchar] = @nvarchar,
  [varbinary] = @varbinary,
  [varchar] = @varchar
