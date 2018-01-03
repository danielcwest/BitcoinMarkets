CREATE PROCEDURE [dbo].[GetMakerOrdersByStatus]
	@status NVARCHAR(50),
	@pairId INT = 0
AS
BEGIN
	SELECT o.Id, a.BaseExchange, a.CounterExchange, a.BaseSymbol, a.CounterSymbol, a.BaseCurrency, a.MarketCurrency, o.BaseOrderUuid, o.CounterOrderUuid, o.Type, b.TradeThreshold, b.WithdrawalThreshold, a.CounterExchangeFee,  a.CounterBaseWithdrawalFee, a.CounterMarketWithdrawalFee, o.BaseQuantityFilled, o.CounterQuantityFilled, a.AskSpread, a.BidSpread, o.ProcessId, o.Commission
	from dbo.MakerOrder o
	inner join dbo.ArbitragePair a on a.Id = o.PairId
	inner join dbo.BaseCurrency b on a.BaseCurrency = b.Id
	where (@status = '' OR o.Status = @status) AND (@pairId = 0 OR o.PairId = @pairId)
	order by o.CreatedUtc
END
