CREATE TABLE [dbo].[MaxLengthTypes]
(
  [key] INT NOT NULL PRIMARY KEY,
  [nvarchar] NVARCHAR(MAX) NOT NULL,
  [varbinary] VARBINARY(MAX) NOT NULL,
  [varchar] VARCHAR(MAX) NOT NULL
)
