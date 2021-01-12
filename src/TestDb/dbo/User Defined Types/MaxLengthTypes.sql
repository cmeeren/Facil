CREATE TYPE [dbo].[MaxLengthTypes] AS TABLE
(
  [nvarchar] NVARCHAR(MAX) NOT NULL,
  [varbinary] VARBINARY(MAX) NOT NULL,
  [varchar] VARCHAR(MAX) NOT NULL
)
