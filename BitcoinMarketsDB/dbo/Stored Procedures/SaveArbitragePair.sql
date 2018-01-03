CREATE PROCEDURE [dbo].[SaveArbitragePair]
	@id int,
    @status NVARCHAR(50),
	@type NVARCHAR(50),
	@tradeThreshold DECIMAL  (18, 8),
	@increment DECIMAL  (18, 8),
	@tickSize DECIMAL  (18, 8),
	@askSpread DECIMAL  (18, 8),
	@bidSpread DECIMAL  (18, 8)
AS
BEGIN
	UPDATE dbo.ArbitragePair SET
		[Status] = IIF(@status = '', [Status], @status),
		[Type] = IIF(@type = '', [Type], @type),
		TradeThreshold = IIF(@tradeThreshold = 0, TradeThreshold, @tradeThreshold),
		Increment = IIF(@increment = 0, Increment, @increment), 
		TickSize = IIF(@tickSize = 0, TickSize, @tickSize),
		AskSpread = IIF(@askSpread = 0, AskSpread, @askSpread), 
		BidSpread = IIF(@bidSpread = 0, BidSpread, @bidSpread)
	Where Id = @id
END
