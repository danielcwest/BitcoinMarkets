
select getutcdate()
--ERROR LOG--
select *
from dbo.ErrorLog
where CreatedAtUtc > DATEADD(Hour, -1,  getutcdate())
order by CreatedAtUtc desc
--delete from dbo.ErrorLog
--ARBITRAGE PAIRS--
exec dbo.GetArbitragePairs 'trade'
exec dbo.GetArbitragePairs 'log'
exec dbo.GetArbitragePairs 'error'
--RECENT OPPORTUNITIES
select a.BaseExchange, a.CounterExchange, a.Symbol, o.Spread, o.BasePrice, o.ArbPrice, o.CreatedUtc, b.TradeThreshold, b.SpreadThreshold
from dbo.Opportunity o inner join dbo.ArbitragePair a on a.Id = o.PairId inner join dbo.BaseCurrency b on b.Id = a.BaseCurrency
where  o.CreatedUtc > DATEADD(Hour, -1, getutcdate())
--a.Symbol <> 'QTUMETH' AND
order by o.CreatedUtc desc
--ENGINE LOG--
Select top 1000
    StartedUtc, RunType, BaseExchange, ArbExchange, Duration, Metadata, ErrorCount, ResultStatus
from dbo.EngineLog
order by StartedUtc desc
--RECENTLY RUN PAIRS--
Select *
from dbo.ArbitragePair
where LastRunUtc > DATEADD(minute, -15, getutcdate())
order by LastRunUtc desc
--RECENT Opportunities--
Select *
from dbo.ArbitragePair
where LastOpportunityUtc > DATEADD(hour, -1, getutcdate())
order by LastRunUtc desc
--RECENT TRADES--
Select *
from dbo.ArbitragePair
where LastTradeUtc > DATEADD(MINUTE, -60, getutcdate())
order by LastTradeUtc desc
--TRANSACTIONS--
select t.Id, a.Symbol, t.Commission, BaseOrderUuid, CounterOrderUuid, BaseWithdrawalUuid, CounterWithdrawalUuid, BaseTxId, CounterTxId, t.Status, t.Type, t.CreatedUtc, t.Metadata
from dbo.[Transaction] t inner join dbo.ArbitragePair a on a.Id = t.PairId
order by CreatedUtc desc
--delete from dbo.[Transaction] where Id = 3
--BASE CURRENCY--
select *
from dbo.BaseCurrency
--update dbo.BaseCurrency Set TradeThreshold = .1 where Id = 'BTC'
--MISC--
select *
from dbo.ArbitragePair
where Id = 11
--0.00215
--ADX--
update dbo.ArbitragePair Set BaseBaseWithdrawalFee = 0.005, CounterBaseWithdrawalFee = 0.0035 where Id = 11
--SNT--
update dbo.ArbitragePair Set BaseBaseWithdrawalFee = 0.005, CounterBaseWithdrawalFee = 0.0035 where Id = 8
--FUN--
update dbo.ArbitragePair Set BaseBaseWithdrawalFee = 0.005, CounterBaseWithdrawalFee = 0.0035 where Id = 9
select *
from dbo.ArbitragePair
where BaseCurrency = 'BTC' and CounterExchange = 'Hitbtc'
delete from dbo.ArbitragePair where BaseExchange = 'Binance'