CREATE TABLE [dbo].[BaseCurrency]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[TradeThreshold] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[SpreadThreshold] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[WithdrawalThreshold] DECIMAL (18, 8) DEFAULT ((0)) NOT NULL,
	[Enabled] BIT DEFAULT(0) NOT NULL
)
