CREATE PROCEDURE [dbo].[ProcWithLengthTypes]
  @binary BINARY(3),
  @char CHAR(3),
  @nchar NCHAR(3),
  @nvarchar NVARCHAR(3),
  @varbinary VARBINARY(3),
  @varchar VARCHAR(3)
AS

SELECT
  [binary] = @binary,
  [char] = @char,
  [nchar] = @nchar,
  [nvarchar] = @nvarchar,
  [varbinary] = @varbinary,
  [varchar] = @varchar
