CREATE PROCEDURE [dbo].[UpdateTransactionWithdrawalUuid]
	@id int,
	@baseUuid NVARCHAR(128),
	@counterUuid NVARCHAR(128),
	@commission decimal (18, 8)
AS
BEGIN
	UPDATE dbo.[Transaction] Set 
		BaseWithdrawalUuid = @baseUuid,
		CounterWithdrawalUuid = @counterUuid,
		Commission = @commission,
		Status = 'withdrawalpending'
	Where Id = @id
END
