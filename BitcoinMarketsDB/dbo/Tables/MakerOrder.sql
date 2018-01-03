CREATE TABLE [dbo].[MakerOrder]
(
	[Id]					INT					IDENTITY (1, 1)			PRIMARY KEY,
	[PairId]				INT											NOT NULL,
	[BaseOrderUuid]			NVARCHAR (128)		DEFAULT (N'')			NOT NULL,
	[CounterOrderUuid]		NVARCHAR (128)		DEFAULT (N'')			NOT NULL,
	[BaseQuantityFilled]	DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,
	[CounterQuantityFilled]	DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,
	[OriginalBaseRate]		DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,
	[BaseRate]				DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,
	[CounterRate]			DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,

	[BaseCostProceeds]		DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL, -- if buy, its a cost
	[CounterCostProceeds]	DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,
	[Commission]			DECIMAL  (18, 8)	DEFAULT ((0))			NOT NULL,

	[Status]				NVARCHAR (50)		DEFAULT (N'')			NOT NULL, --open, closed, pending, complete
	[Type]					NVARCHAR (50)		DEFAULT (N'')			NOT NULL, --buy, sell
	[CreatedUtc]			DATETIME			DEFAULT (getutcdate())	NOT NULL,
	[ClosedUtc]				DATETIME			DEFAULT ('9999-12-31')	NOT NULL,
	[ProcessId]				INT					DEFAULT(0)				NOT NULL,
	[Metadata]				NVARCHAR(MAX) NOT NULL DEFAULT (N'')
	CONSTRAINT [FK_MakerOrder_ArbitragePair] FOREIGN KEY ([PairId]) REFERENCES [dbo].[ArbitragePair] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
