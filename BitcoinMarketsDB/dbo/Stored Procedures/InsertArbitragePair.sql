CREATE PROCEDURE [dbo].[InsertArbitragePair]
	@baseExchange NVARCHAR(50),
    @arbExchange NVARCHAR(50),
    @symbol NVARCHAR(50),
	@baseSymbol NVARCHAR(50),
	@counterSymbol NVARCHAR(50),
	@baseCurrency NVARCHAR(50), 
	@marketCurrency NVARCHAR(50)
AS
BEGIN
    INSERT INTO dbo.ArbitragePair
        (BaseExchange, CounterExchange, Symbol, BaseSymbol, CounterSymbol, BaseCurrency, MarketCurrency)
    VALUES
        (@baseExchange, @arbExchange, @symbol, @baseSymbol, @counterSymbol, @baseCurrency, @marketCurrency)
END