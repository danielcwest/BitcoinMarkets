CREATE PROCEDURE [dbo].[StartEngineProcess]
	@baseExchange NVARCHAR(50),
    @arbExchange NVARCHAR(50),
	@runType NVARCHAR(50),
	@baseCurrency NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.EngineLog (BaseExchange, ArbExchange, RunType, BaseCurrency) 
	OUTPUT Inserted.Id
	VALUES (@baseExchange, @arbExchange, @runType, @baseCurrency)
END
