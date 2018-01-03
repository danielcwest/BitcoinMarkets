CREATE PROCEDURE [dbo].[GetRecentOpportunities]
	@hours int = 1
AS
BEGIN

	select a.BaseExchange, a.CounterExchange, a.Symbol, o.Spread, o.BasePrice, o.ArbPrice, o.CreatedUtc, b.TradeThreshold, b.SpreadThreshold from dbo.Opportunity o
	inner join dbo.ArbitragePair a on a.Id = o.PairId
	inner join dbo.BaseCurrency b on b.Id = a.BaseCurrency
	where  o.CreatedUtc > DATEADD(Hour, -1 * @hours, getutcdate())
	order by o.CreatedUtc desc

END
