CREATE PROCEDURE [dbo].[InsertTrade]
    @baseExchange NVARCHAR(50),
    @arbExchange NVARCHAR(50),
    @symbol NVARCHAR(50),
    @basePrice DECIMAL(18, 8),
    @arbPrice DECIMAL(18, 8),
    @spread DECIMAL(5,2)
AS
BEGIN
    INSERT INTO dbo.TradeLog
        (BaseExchange, ArbExchange, Symbol, BasePrice, ArbPrice, Spread)
    VALUES
        (@baseExchange, @arbExchange, @symbol, @basePrice, @arbPrice, @spread)
END