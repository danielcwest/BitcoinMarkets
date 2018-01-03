CREATE PROCEDURE [dbo].[InsertMakerOrder]
	@pairId int,
	@type NVARCHAR(50),
	@baseUuid NVARCHAR(128),
	@counterUuid NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT into dbo.[MakerOrder] (PairId, Type, BaseOrderUuid, CounterOrderUuid, Status) 
	VALUES (@pairId, @type, @baseUuid, @counterUuid, N'filled')
END