using System;

namespace BMCore.Contracts
{
    public interface IOrder
    {
        string Uuid { get; set; }
        string Exchange { get; set; }
        string Symbol { get; set; }
        decimal Quantity { get; set; }
        decimal QuantityFilled { get; set; }
        decimal CostProceeds { get; set; }
        decimal AvgRate { get; set; }
        decimal Fees { get; set; }
        bool IsFilled { get; set; }
        bool IsClosed { get; set; }
        string Side { get; set; }
    }
}