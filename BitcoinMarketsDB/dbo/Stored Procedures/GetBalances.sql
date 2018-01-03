CREATE PROCEDURE [dbo].[GetBalances]
	@currency NVARCHAR(50) = '',
	@baseExchange NVARCHAR(50) = '',
	@counterExchange NVARCHAR(50) = '',
	@fromDate DATETIME = '2000-01-01',
	@processId INT = 0
AS
BEGIN
	select Currency, BaseExchange, CounterExchange, Total, BtcValue, ProcessId, CreatedUtc from dbo.BalanceSnapshot
	WHERE (@currency = '' OR @currency = Currency)
	AND (@baseExchange = '' OR @baseExchange = BaseExchange)
	AND (@counterExchange = '' OR @counterExchange = CounterExchange)
	AND (@processId = 0 OR @processId = ProcessId)
	AND CreatedUtc > @fromDate
	order by CreatedUtc desc
END
