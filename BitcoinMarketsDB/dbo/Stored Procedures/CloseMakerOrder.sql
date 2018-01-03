CREATE PROCEDURE [dbo].[CloseMakerOrder]
	@Id int
AS
BEGIN
	update dbo.MakerOrder Set Status = 'complete'
	where Id = @Id
END