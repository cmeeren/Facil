CREATE TABLE [dbo].[TableWithComputedCol]
(
  Id INT NOT NULL PRIMARY KEY,
  Foo BIGINT NOT NULL,
  Bar AS 2
)
