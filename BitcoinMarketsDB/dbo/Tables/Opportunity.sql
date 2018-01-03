CREATE TABLE [dbo].[Opportunity]
(
	[PairId]		INT NOT NULL,
	[TxThreshold]  DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
    [BasePrice]    DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
    [ArbPrice]     DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
    [Spread]       DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
    [CreatedUtc] DATETIME        DEFAULT (getutcdate()) NOT NULL,
	CONSTRAINT [FK_Opportunity_ArbitragePair] FOREIGN KEY ([PairId]) REFERENCES [dbo].[ArbitragePair] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

)
