﻿CREATE TABLE [dbo].[LengthTypes]
(
  [key] INT NOT NULL PRIMARY KEY,
  [binary] BINARY(3) NOT NULL,
  [char] CHAR(3) NOT NULL,
  [nchar] NCHAR(3) NOT NULL,
  [nvarchar] NVARCHAR(3) NOT NULL,
  [varbinary] VARBINARY(3) NOT NULL,
  [varchar] VARCHAR(3) NOT NULL
)
