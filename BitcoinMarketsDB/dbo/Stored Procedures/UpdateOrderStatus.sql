CREATE PROCEDURE [dbo].[UpdateOrderStatus]
	@id int,
	@status NVARCHAR(50),
	@baseQuantityFilled DECIMAL  (18, 8),
	@counterQuantityFilled DECIMAL  (18, 8)
AS
BEGIN

	IF @status = 'closed' 
		DELETE from dbo.MakerOrder where Id = @id
	Else
		UPDATE dbo.[MakerOrder] Set 
		[Status] = @status, 
		BaseQuantityFilled = IIF(@baseQuantityFilled = 0, BaseQuantityFilled, @baseQuantityFilled), 
		CounterQuantityFilled = IIF(@counterQuantityFilled = 0, CounterQuantityFilled, @counterQuantityFilled) 
		where Id = @id
END