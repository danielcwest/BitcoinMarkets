
--delete from dbo.TradeLog
select *
from dbo.ErrorLog;
select *
from dbo.TradeLog
order by CreatedAtUtc desc