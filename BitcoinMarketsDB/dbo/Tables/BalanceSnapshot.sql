CREATE TABLE [dbo].[BalanceSnapshot]
(
	[Id]	INT IDENTITY (1, 1) PRIMARY KEY,
	[BaseExchange]			NVARCHAR (50)   DEFAULT (N'') NOT NULL,
    [CounterExchange]		NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[Currency] NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[Total] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[Price] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[BtcValue] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[ProcessId] INT DEFAULT(0) NOT NULL,
	[CreatedUtc]	DATETIME        DEFAULT (getutcdate()) NOT NULL
)
