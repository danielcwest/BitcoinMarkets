CREATE PROCEDURE [dbo].[GetRecentTransactions]
	@hours int = 1
AS
BEGIN
	SELECT t.Id, a.BaseExchange, a.CounterExchange, a.Symbol, a.BaseCurrency, a.MarketCurrency, t.BaseOrderUuid, t.CounterOrderUuid, t.BaseWithdrawalUuid, t.CounterWithdrawalUuid, t.BaseTxId, t.CounterTxId, t.Type, b.TradeThreshold, b.SpreadThreshold, t.Commission
	from dbo.[Transaction] t
	inner join dbo.ArbitragePair a on a.Id = t.PairId
	inner join dbo.BaseCurrency b on a.BaseCurrency = b.Id
	where t.CreatedUtc > DATEADD(hour, -1 * @hours, getutcdate())
END