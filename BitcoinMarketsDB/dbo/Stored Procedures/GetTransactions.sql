CREATE PROCEDURE [dbo].[GetTransactions]
	@status NVARCHAR(50),
	@pairId INT = 0
AS
BEGIN
	SELECT t.Id, a.BaseExchange, a.CounterExchange, a.BaseSymbol, a.CounterSymbol, a.BaseCurrency, a.MarketCurrency, t.BaseOrderUuid, t.CounterOrderUuid, t.BaseWithdrawalUuid, t.CounterWithdrawalUuid, t.BaseTxId, t.CounterTxId, t.Type, b.TradeThreshold, a.CounterExchangeFee, a.BaseBaseWithdrawalFee, a.BaseMarketWithdrawalFee, a.CounterBaseWithdrawalFee, a.CounterMarketWithdrawalFee
	from dbo.[Transaction] t
	inner join dbo.ArbitragePair a on a.Id = t.PairId
	inner join dbo.BaseCurrency b on a.BaseCurrency = b.Id
	where (@status = '' OR t.Status = @status) AND (@pairId = 0 OR t.PairId = @pairId)
END
