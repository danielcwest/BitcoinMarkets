CREATE PROCEDURE [dbo].[UpdateOrderUuid]
	@id int,
	@baseUuid NVARCHAR(128),
	@counterUuid NVARCHAR(128),
	@counterRate DECIMAL  (18, 8),
	@status NVARCHAR(50)
AS
BEGIN
	UPDATE dbo.[MakerOrder] Set 
		BaseOrderUuid = IIF(@baseUuid = '', BaseOrderUuid, @baseUuid),
		CounterOrderUuid = IIF(@counterUuid = '', CounterOrderUuid, @counterUuid),
		CounterRate = IIF(@counterRate = 0, CounterRate, @counterRate),
		Status = IIF(@status = '', Status, @status)
	Where Id = @id
END