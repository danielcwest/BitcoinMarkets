
Create PROCEDURE [dbo].[GetTrades]
    @count INT
AS
BEGIN
    SET NOCOUNT ON;
    Select top (@count)
        BaseExchange, ArbExchange, Symbol, BasePrice, ArbPrice, Spread, CreatedAtUtc
    from dbo.TradeLog
END
