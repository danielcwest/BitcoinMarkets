CREATE PROCEDURE [dbo].[EndEngineProcess]
	@id int,
	@result NVARCHAR(50),
	@meta NVARCHAR(MAX) = ''
AS
BEGIN
	DECLARE @date DATETIME = getutcdate();

	Declare @@errorCount INT = (Select Count(*) from dbo.[ErrorLog] where ProcessId = @id) 

	UPDATE dbo.EngineLog Set ResultStatus = @result, Metadata = @meta, CompletedUtc = @date, Duration = DATEDIFF( second, StartedUtc, @date), ErrorCount = @@errorCount
	where Id = @id
END
