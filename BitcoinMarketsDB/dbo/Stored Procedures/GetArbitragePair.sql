CREATE PROCEDURE [dbo].[GetArbitragePair]
	@id int = 0
AS
BEGIN
	SELECT top 1 a.Id, a.BaseExchange, a.CounterExchange, a.Symbol, a.Status, a.Type, a.BaseSymbol, a.CounterSymbol, a.BaseCurrency, a.MarketCurrency, IIF(a.TradeThreshold <> 0, a.TradeThreshold, b.TradeThreshold) as TradeThreshold, b.SpreadThreshold, b.WithdrawalThreshold, a.BaseExchangeFee, a.CounterExchangeFee, a.BaseBaseWithdrawalFee, a.BaseMarketWithdrawalFee, a.CounterBaseWithdrawalFee, a.CounterMarketWithdrawalFee, a.AskSpread, a.BidSpread, (a.AskSpread + a.BidSpread) as MarketSpread, DecimalPlaces, AskMultiplier, BidMultiplier, Increment
	FROM dbo.ArbitragePair a
	inner join dbo.BaseCurrency b on b.Id = a.BaseCurrency
	where a.Id = @id
END
