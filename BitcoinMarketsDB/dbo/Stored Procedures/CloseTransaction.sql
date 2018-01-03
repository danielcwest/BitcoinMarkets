CREATE PROCEDURE [dbo].[CloseTransaction]
	@id int,
	@baseTxId NVARCHAR(128),
	@counterTxId NVARCHAR(128)
	AS
BEGIN
	UPDATE dbo.[Transaction] Set 
		BaseTxId = @baseTxId,
		CounterTxId = @counterTxId,
		ClosedUtc = getutcdate(),
		Status = 'complete'
	Where Id = @id
END