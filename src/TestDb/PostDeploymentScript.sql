/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


DELETE FROM Table1

INSERT INTO Table1
  (TableCol1, TableCol2)
VALUES
  ('test1', 1),
  ('test2', 2)



DELETE FROM OptionTableWithDto

INSERT INTO OptionTableWithDto
  (Col1, Col2)
VALUES
  ('test1', 1),
  (NULL, NULL)



DELETE FROM OptionTableWithoutDto

INSERT INTO OptionTableWithoutDto
  (Col1, Col2)
VALUES
  ('test1', 1),
  (NULL, NULL)



DELETE FROM VoptionTableWithDto

INSERT INTO VoptionTableWithDto
  (Col1, Col2)
VALUES
  ('test1', 1),
  (NULL, NULL)



DELETE FROM VoptionTableWithoutDto

INSERT INTO VoptionTableWithoutDto
  (Col1, Col2)
VALUES
  ('test1', 1),
  (NULL, NULL)



DELETE FROM TableWithNullabilityOverride

INSERT INTO TableWithNullabilityOverride
  (NotNull1, NotNull2, Null1, Null2)
VALUES
  (1, 2, 3, 4)
