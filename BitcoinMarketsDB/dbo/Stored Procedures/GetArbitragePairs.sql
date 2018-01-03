CREATE PROCEDURE [dbo].[GetArbitragePairs]
	@type NVARCHAR(50) = '',
	@baseExchange NVARCHAR(50) = '',
    @arbExchange NVARCHAR(50) = ''
AS
BEGIN
	SELECT a.Id, a.BaseExchange, a.CounterExchange, a.Symbol, a.Status, a.Type, a.BaseSymbol, a.CounterSymbol, a.BaseCurrency, a.MarketCurrency, IIF(a.TradeThreshold <> 0, a.TradeThreshold, b.TradeThreshold) as TradeThreshold, b.SpreadThreshold, b.WithdrawalThreshold, a.BaseExchangeFee, a.CounterExchangeFee, a.BaseBaseWithdrawalFee, a.BaseMarketWithdrawalFee, a.CounterBaseWithdrawalFee, a.CounterMarketWithdrawalFee, a.AskSpread, a.BidSpread, (a.AskSpread + a.BidSpread) as MarketSpread, DecimalPlaces, AskMultiplier, BidMultiplier, Increment, TickSize
	FROM dbo.ArbitragePair a
	inner join dbo.BaseCurrency b on b.Id = a.BaseCurrency
	where (@baseExchange = '' OR a.BaseExchange = @baseExchange) AND (@arbExchange = '' OR a.CounterExchange = @arbExchange) 
	AND (@type = '' OR (@type = Type AND a.Status != 'error')) 
END
