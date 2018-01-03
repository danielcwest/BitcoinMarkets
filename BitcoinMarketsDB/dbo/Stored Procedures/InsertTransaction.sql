CREATE PROCEDURE [dbo].[InsertTransaction]
	@pairId int,
	@type NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT into dbo.[Transaction] (PairId, Type) 
	OUTPUT Inserted.Id
	VALUES (@pairId, @type)
END
