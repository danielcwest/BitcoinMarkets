CREATE PROCEDURE [dbo].[InsertOpportunity]
	@PairId INT,
	@basePrice DECIMAL(18, 8),
    @arbPrice DECIMAL(18, 8),
    @spread DECIMAL(18,8),
	@threshold DECIMAL(18,8)
AS
BEGIN
    INSERT INTO dbo.Opportunity
        (PairId, BasePrice, ArbPrice, Spread, TxThreshold)
    VALUES
        (@PairId, @basePrice, @arbPrice, @spread, @threshold)
END
