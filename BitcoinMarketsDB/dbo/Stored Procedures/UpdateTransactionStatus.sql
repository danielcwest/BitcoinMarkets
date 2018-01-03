CREATE PROCEDURE [dbo].[UpdateTransactionStatus]
	@id int,
	@status NVARCHAR(50),
	@meta NVARCHAR(MAX)
AS
BEGIN
	UPDATE dbo.[Transaction] Set [Status] = @status, Metadata = IIF(@meta = '', Metadata, @meta) where Id = @id
END
