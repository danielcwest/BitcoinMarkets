CREATE PROCEDURE [dbo].[UpdateTransactionOrderUuid]
	@id int,
	@baseUuid NVARCHAR(128),
	@counterUuid NVARCHAR(128),
	@meta NVARCHAR(MAX)
AS
BEGIN
	UPDATE dbo.[Transaction] Set 
		BaseOrderUuid = @baseUuid,
		CounterOrderUuid = @counterUuid,
		Status = 'orderpending',
		Metadata = IIF(@meta = '', Metadata, @meta)
	Where Id = @id
END
