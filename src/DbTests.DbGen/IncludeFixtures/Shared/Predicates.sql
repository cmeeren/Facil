SET @sql += N'
FROM (VALUES (1, N''One''), (2, N''Two'')) AS Items(Id, Name)
WHERE Id >= @minId';
