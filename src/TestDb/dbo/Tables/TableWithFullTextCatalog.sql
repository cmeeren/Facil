CREATE TABLE [dbo].[TableWithFullTextCatalog]
(
  [Col1] NVARCHAR (100) NOT NULL
  CONSTRAINT [PK_TableWithFullTextCatalog] PRIMARY KEY ([Col1])
)

GO

CREATE FULLTEXT INDEX
  ON [dbo].[TableWithFullTextCatalog] ([Col1])
  KEY INDEX [PK_TableWithFullTextCatalog]
  ON [FullTextCatalog]
  WITH CHANGE_TRACKING AUTO

GO
