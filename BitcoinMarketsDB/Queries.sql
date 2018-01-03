
select getutcdate()





				--ENGINE LOG--
Select top 1000 StartedUtc, RunType, BaseExchange, ArbExchange, Duration, Metadata, ErrorCount, ResultStatus from dbo.EngineLog order by StartedUtc desc




				--ARBITRAGE PAIRS--

select * from dbo.ArbitragePair where Symbol = 'ETHBTC'

select * from dbo.ArbitragePair where Status = 'trade'  and BaseCurrency = 'ETH'

update dbo.ArbitragePair SET BaseBaseWithdrawalFee = 0.005, CounterBaseWithdrawalFee = 0.0035 where BaseCurrency = 'ETH'

update dbo.ArbitragePair SET BaseMarketWithdrawalFee = 0.005, CounterMarketWithdrawalFee = 0.509 where Id = 55

update dbo.ArbitragePair SET TradeThreshold = 0.01 where BaseCurrency = 'ETH'
select * from dbo.ArbitragePair where ID = 116

--insert into dbo.ArbitragePair (BaseExchange, CounterExchange, Symbol, BaseSymbol, CounterSymbol, BaseCurrency, MarketCurrency, Type) VALUES ('Hitbtc', 'Gdax', 'LTCBTC', 'LTCBTC', 'LTC-BTC', 'BTC', 'LTC', 'test')

 select * from dbo.ArbitragePair 

 exec dbo.GetArbitragePairs @type = 'market'

				-- ORDERS --
update dbo.ArbitragePair set AskSpread = 0.01, BidSpread = 0.01, TradeThreshold = 0.00025
update dbo.ArbitragePair set AskSpread = 0.015, BidSpread = 0.015 Where BaseCurrency = 'ETH'

--update dbo.ArbitragePair Set Type = 'log' where Status in ('open', 'pending') 
select * from dbo.MakerOrder order by CreatedUtc desc
select * from dbo.MakerOrder order by CreatedUtc desc

-- delete from dbo.MakerOrder where PairId = 118


	--update dbo.MakerOrder SET Status = 'filled' where PairId = 7 AND  CreatedUtc > DATEADD(Hour, -24,  getutcdate())


	select Status, Count(*) from dbo.MakerOrder 
	where PairId = 11 AND  CreatedUtc > DATEADD(Hour, -1,  getutcdate()) 
	group by Status
	--order by CreatedUtc desc



--delete from dbo.MakerOrder where Id < 2
--delete from dbo.MakerOrder where Status <> 'complete'
select * from dbo.MakerOrder where Status <> 'closed' order by CreatedUtc desc
--update dbo.MakerOrder Set Type = 'log' where Id > 23
				--ERROR LOG--
select * from dbo.ErrorLog where CreatedAtUtc > DATEADD(Hour, -1,  getutcdate()) order by CreatedAtUtc desc 
--delete from dbo.ErrorLog

select a.Symbol, SUM(Commission) * 1 as 'USD Commission', Count(*) as 'Trade Count' from dbo.MakerOrder o 
inner join dbo.ArbitragePair a on a.Id = o.PairId
where BaseQuantityFilled = CounterQuantityFilled AND o.CreatedUtc > DATEADD(Hour, -24,  getutcdate())
Group By a.Symbol

exec dbo.GetHeroStats 24

select Id, Symbol, Status, Type, Increment, TickSize, AskSpread, BidSpread, AskMultiplier, BidMultiplier, TradeThreshold  from dbo.ArbitragePair 
update dbo.ArbitragePair Set Type = 'log' where Symbol = 'STRATBTC'

--update dbo.ArbitragePair Set AskSpread = 0.005, BidSpread = 0.005
			-- BALANCES --

select Id, Currency, Total, Price, BtcValue, DATEDIFF (ss ,'1970-01-01' , CreatedUtc) as g from dbo.BalanceSnapshot 
order by CreatedUtc desc 
--delete from dbo.BalanceSnapshot

select Id, Currency, Total, Price, BtcValue, CreatedUtc from dbo.BalanceSnapshot 
where CreatedUtc > DATEADD(Hour, -12,  getutcdate())
order by CreatedUtc desc 

select DATEDIFF (ss ,'1970-01-01' , CreatedUtc) as Time, Sum(BtcValue) as BtcValue, Sum(BtcValue) * 9600 as UTCValue from dbo.BalanceSnapshot 
group by DATEDIFF (ss ,'1970-01-01' , CreatedUtc)
order by DATEDIFF (ss ,'1970-01-01' , CreatedUtc) desc


select * from dbo.MakerOrder where Status = 'notexists'

select * from dbo.MakerOrder where Status <> 'closed' AND CreatedUtc > DATEADD(Hour, -24,  getutcdate()) AND BaseQuantityFilled <> CounterQuantityFilled 

select * from dbo.MakerOrder where Status <> 'closed' AND CreatedUtc > DATEADD(Hour, -4,  getutcdate()) order by CreatedUtc desc

	select Symbol, BaseOrderUuid, CounterOrderUuid, BaseQuantityFilled, CounterQuantityFilled, BaseRate, CounterRate, BaseCostProceeds, CounterCostProceeds, Commission, m.Status, m.Type, m.CreatedUtc, ProcessId from dbo.MakerOrder m
	inner join dbo.ArbitragePair a on a.Id = m.PairId
	where m.Status <> 'closed' AND  m.CreatedUtc > DATEADD(Hour, -4,  getutcdate()) --AND a.Symbol = 'EOSBTC' --AND BaseQuantityFilled = CounterQuantityFilled
	order by CreatedUtc desc

--order by CreatedUtc desc
--update dbo.MakerOrder Set Status = 'filled' where CreatedUtc > DATEADD(Hour, -1,  getutcdate()) 
