CREATE TABLE [dbo].[Transaction]
(
	[Id]			INT			IDENTITY (1, 1)			PRIMARY KEY,
	[PairId]		INT									NOT NULL,
	[BaseOrderUuid] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[CounterOrderUuid] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[BaseWithdrawalUuid] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[CounterWithdrawalUuid] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[BaseTxId] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[CounterTxId] NVARCHAR (128)   DEFAULT (N'') NOT NULL,
	[Status]     NVARCHAR (50)   DEFAULT (N'') NOT NULL, -- orderpending, ordercomplete, withdrawalpending, complete
	[Type]     NVARCHAR (50)   DEFAULT (N'') NOT NULL, --basebuy, basesell
	[CreatedUtc]	DATETIME    DEFAULT (getutcdate())	NOT NULL,
	[ClosedUtc]	DATETIME    DEFAULT ('2000-01-01')	NOT NULL,
	[Commission]       DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
    [Metadata] NVARCHAR(MAX) NOT NULL DEFAULT (N'')
	CONSTRAINT [FK_Transaction_ArbitragePair] FOREIGN KEY ([PairId]) REFERENCES [dbo].[ArbitragePair] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
