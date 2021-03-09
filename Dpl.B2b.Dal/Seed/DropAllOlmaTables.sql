BEGIN TRANSACTION

DECLARE @sqlDropKeys NVARCHAR(MAX);
SET @sqlDropKeys = N'';

SELECT @sqlDropKeys = @sqlDropKeys + N'
  ALTER TABLE ' + QUOTENAME(s.name) + N'.'
  + QUOTENAME(t.name) + N' DROP CONSTRAINT '
  + QUOTENAME(c.name) + ';'
FROM sys.objects AS c
INNER JOIN sys.tables AS t
ON c.parent_object_id = t.[object_id]
INNER JOIN sys.schemas AS s 
ON t.[schema_id] = s.[schema_id]
WHERE c.[type] IN ('D','C','F','PK','UQ') and  S.[Name]  = 'dbo'
ORDER BY c.[type];

EXEC sys.sp_executesql @sqlDropKeys;


DECLARE @sqlDropTables NVARCHAR(MAX);
SET @sqlDropTables = N'';

SELECT @sqlDropTables = @sqlDropTables + N'
  DROP TABLE ' + QUOTENAME(s.name) + N'.'
  + QUOTENAME(t.name) + ';'
FROM  sys.tables AS t
INNER JOIN sys.schemas AS s 
ON t.[schema_id] = s.[schema_id]
WHERE S.[Name]  = 'dbo';

DECLARE @sqlTruncateTables NVARCHAR(MAX) = REPLACE(@sqlDropTables,'DELETE','TRUNCATE')

--EXEC sys.sp_executesql  @sqlTruncateTables;
EXEC sys.sp_executesql  @sqlDropTables;

DROP VIEW IF EXISTS [dbo].[LmsOrders_View];

COMMIT TRANSACTION