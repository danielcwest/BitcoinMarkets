CREATE PROCEDURE [dbo].[UpdateOrder]
	@id int,
	@baseUuid NVARCHAR(128),
	@counterUuid NVARCHAR(128),
	@baseRate DECIMAL  (18, 8),
	@counterRate DECIMAL  (18, 8),
	@status NVARCHAR(50),
	@baseQuantityFilled DECIMAL  (18, 8),
	@counterQuantityFilled DECIMAL  (18, 8),
	@baseCost DECIMAL  (18, 8),
	@counterCost DECIMAL  (18, 8),
	@commission DECIMAL  (18, 8),
	@meta NVARCHAR(MAX)
AS
BEGIN
	UPDATE dbo.[MakerOrder] Set 
		BaseOrderUuid = IIF(@baseUuid = '', BaseOrderUuid, @baseUuid),
		CounterOrderUuid = IIF(@counterUuid = '', CounterOrderUuid, @counterUuid),
		BaseRate = IIF(@baseRate = 0, BaseRate, @baseRate),
		CounterRate = IIF(@counterRate = 0, CounterRate, @counterRate),
		BaseQuantityFilled = IIF(@baseQuantityFilled = 0, BaseQuantityFilled, @baseQuantityFilled), 
		CounterQuantityFilled = IIF(@counterQuantityFilled = 0, CounterQuantityFilled, @counterQuantityFilled), 
		BaseCostProceeds = IIF(@baseCost = 0, BaseCostProceeds, @baseCost),
		CounterCostProceeds = IIF(@counterCost = 0, CounterCostProceeds, @counterCost), 
		Commission = IIF(@commission = 0, Commission, @commission), 
		Status = IIF(@status = '', Status, @status),
		Metadata = IIF(@meta = '', Metadata, @meta)
	Where Id = @id
END