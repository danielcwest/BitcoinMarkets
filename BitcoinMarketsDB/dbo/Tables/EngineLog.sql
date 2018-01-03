CREATE TABLE [dbo].[EngineLog]
(
	[Id]           INT             IDENTITY (1, 1) NOT NULL,
    [BaseExchange] NVARCHAR (50)   DEFAULT (N'') NOT NULL,
    [ArbExchange]  NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[RunType] NVARCHAR (50)   DEFAULT (N'') NOT NULL,
	[BaseCurrency] NVARCHAR (MAX)   DEFAULT (N'') NOT NULL,
	[StartedUtc] DATETIME        DEFAULT (getutcdate()) NOT NULL, 
	[CompletedUtc] DATETIME        DEFAULT '9999-12-31' NOT NULL,
	[Duration] INT DEFAULT (0) NOT NULL,
	[ResultStatus] NVARCHAR(50)  DEFAULT (N'') NOT NULL,
	[ErrorCount] INT DEFAULT (0) NOT NULL,
	[Metadata] NVARCHAR(MAX) NOT NULL DEFAULT (N'')
)
