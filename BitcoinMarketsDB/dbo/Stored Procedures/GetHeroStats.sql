CREATE PROCEDURE [dbo].[GetHeroStats]
	@hours int = 0
AS
BEGIN
	select a.Symbol, SUM(Commission) as 'Commission', Count(*) as 'TradeCount' from dbo.MakerOrder o 
	inner join dbo.ArbitragePair a on a.Id = o.PairId
	where BaseQuantityFilled = CounterQuantityFilled AND o.CreatedUtc > DATEADD(Hour, -1 * @hours,  getutcdate())
	AND o.Status = 'complete'
	Group By a.Symbol
END
