CREATE PROCEDURE [dbo].[InsertError]
    @baseExchange NVARCHAR(50),
    @arbExchange NVARCHAR(50),
    @symbol NVARCHAR(50),
    @method NVARCHAR(50),
    @message NVARCHAR(250),
    @exception NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO dbo.ErrorLog
        (BaseExchange, ArbExchange, Symbol, Method, [Message], Exception)
    VALUES
        (@baseExchange, @arbExchange, @symbol, @method, @message, @exception)
END