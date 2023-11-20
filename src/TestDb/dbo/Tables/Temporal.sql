﻿CREATE TABLE [dbo].[Temporal]
(
  [Id] INT NOT NULL,
  [Data] NVARCHAR(MAX),
  [ValidFrom] DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
  [ValidTo] DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
  PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),
  CONSTRAINT PK_Temporal PRIMARY KEY ([Id])
) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[Temporal_History]))
