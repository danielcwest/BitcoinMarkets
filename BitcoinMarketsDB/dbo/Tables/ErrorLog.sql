CREATE TABLE [dbo].[ErrorLog] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [BaseExchange] NVARCHAR (50)  DEFAULT (N'') NOT NULL,
    [ArbExchange]  NVARCHAR (50)  DEFAULT (N'') NOT NULL,
    [Symbol]       NVARCHAR (50)  DEFAULT (N'') NOT NULL,
    [Method]       NVARCHAR (50)  DEFAULT (N'') NOT NULL,
    [Message]      NVARCHAR (250) DEFAULT (N'') NOT NULL,
    [Exception]    NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [CreatedAtUtc] DATETIME       DEFAULT (getutcdate()) NOT NULL,
	[ProcessId] INT DEFAULT (-1) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

