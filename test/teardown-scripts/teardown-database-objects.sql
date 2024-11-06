begin transaction




    /*
-- Drop all foreign key constraints
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ' DROP CONSTRAINT ' + QUOTENAME(f.name) + ';'
FROM sys.foreign_keys AS f
INNER JOIN sys.tables AS t ON f.parent_object_id = t.object_id
INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop all tables
SET @sql = N'';
SELECT @sql += 'DROP TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ';'
FROM sys.tables AS t
INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop all views
SET @sql = N'';
SELECT @sql += 'DROP VIEW ' + QUOTENAME(s.name) + '.' + QUOTENAME(v.name) + ';'
FROM sys.views AS v
INNER JOIN sys.schemas AS s ON v.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop all stored procedures
SET @sql = N'';
SELECT @sql += 'DROP PROCEDURE ' + QUOTENAME(s.name) + '.' + QUOTENAME(p.name) + ';'
FROM sys.procedures AS p
INNER JOIN sys.schemas AS s ON p.schema_id = s.schema_id;
EXEC sp_executesql @sql;

-- Drop all functions
SET @sql = N'';
SELECT @sql += 'DROP FUNCTION ' + QUOTENAME(s.name) + '.' + QUOTENAME(f.name) + ';'
FROM sys.objects AS f
INNER JOIN sys.schemas AS s ON f.schema_id = s.schema_id
WHERE f.type IN ('FN', 'IF', 'TF');
EXEC sp_executesql @sql;
     */

rollback transaction