CREATE TABLE [dbo].[ArbitragePair]
(
	[Id]					INT             IDENTITY (1, 1) PRIMARY KEY,
    [BaseExchange]			NVARCHAR (50)   DEFAULT (N'') NOT NULL,
    [CounterExchange]		NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[Symbol]				NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[Status]			NVARCHAR (50)   DEFAULT (N'active') NOT NULL, --active, error, disabled
	[Type]				NVARCHAR (50)   DEFAULT (N'report') NOT NULL, --log, arbitrage, market
	[Increment]			DECIMAL (18, 8)  DEFAULT ((0.00000001)) NOT NULL, -- Order Increment Size
	[TickSize]			DECIMAL (18, 8)  DEFAULT ((0.00000001)) NOT NULL, -- Order Tick Size
	[AskSpread]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL, -- Spread Above the Market
	[BidSpread]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL, -- Spread Below the Market
	[AskMultiplier]			DECIMAL (18, 8)  DEFAULT ((1)) NOT NULL, -- Ask Size adjustment
	[BidMultiplier]			DECIMAL (18, 8)  DEFAULT ((1)) NOT NULL, -- Bid Size adjustment
	[TradeThreshold]			DECIMAL (18, 8)  DEFAULT ((0.005)) NOT NULL,
	[BaseSymbol]		NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[CounterSymbol]		NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[BaseCurrency]		NVARCHAR (50)   DEFAULT (N'') NOT NULL,
    [MarketCurrency]	NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[BaseExchangeFee]			DECIMAL (18, 8)  DEFAULT ((0.001)) NOT NULL,
	[CounterExchangeFee]			DECIMAL (18, 8)  DEFAULT ((0.001)) NOT NULL,
	[BaseBaseWithdrawalFee]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
	[BaseMarketWithdrawalFee]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
	[CounterBaseWithdrawalFee]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
	[CounterMarketWithdrawalFee]			DECIMAL (18, 8)  DEFAULT ((0)) NOT NULL,
	[CreatedUtc]			DATETIME        DEFAULT (getutcdate()) NOT NULL,
	[LastUpdatedUtc]			DATETIME        DEFAULT ('2000-01-01') NOT NULL,
	[DecimalPlaces] INT DEFAULT (8) NOT NULL,
	[Metadata]				NVARCHAR(MAX)   DEFAULT (N'') NOT NULL,
	UNIQUE	CLUSTERED ([BaseExchange] ASC, [CounterExchange] ASC, [Symbol] ASC)

)
