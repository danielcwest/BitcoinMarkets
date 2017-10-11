-- Create a new table called 'TableName' in schema 'SchemaName'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.ErrorLog', 'U') IS NOT NULL
DROP TABLE dbo.ErrorLog
GO
-- Create the table in the specified schema
CREATE TABLE dbo.ErrorLog
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [BaseExchange] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [ArbExchange] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [Symbol] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [Method] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [Message] NVARCHAR(250) DEFAULT (N'') NOT NULL,
    [Exception] NVARCHAR(MAX) DEFAULT (N'') NOT NULL,
    [CreatedAtUtc] DATETIME DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO