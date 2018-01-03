CREATE PROCEDURE [dbo].[InsertBalanceSnapshot]
	@currency NVARCHAR(50),
	@baseExchange NVARCHAR(50),
	@counterExchange NVARCHAR(50),
	@total DECIMAL (18, 8),
	@price DECIMAL (18, 8),
	@processId INT
AS
BEGIN
    INSERT INTO dbo.BalanceSnapshot
        (Currency, BaseExchange, CounterExchange, Total, Price, BtcValue, ProcessId)
    VALUES
        (@currency, @baseExchange, @counterExchange, @total, @price, @total * @price, @processId)
END