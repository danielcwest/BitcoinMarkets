-- Create a new table called 'TableName' in schema 'SchemaName'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.TradeLog', 'U') IS NOT NULL
DROP TABLE dbo.TradeLog
GO
-- Create the table in the specified schema
CREATE TABLE dbo.TradeLog
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [BaseExchange] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [ArbExchange] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [Symbol] NVARCHAR(50) DEFAULT (N'') NOT NULL,
    [BasePrice] DECIMAL(18, 8) DEFAULT 0 NOT NULL,
    [ArbPrice] DECIMAL(18, 8) DEFAULT 0 NOT NULL,
    [Spread] DECIMAL(5,2) DEFAULT 0 NOT NULL,
    [CreatedAtUtc] DATETIME DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);
GO