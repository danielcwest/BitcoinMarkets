using System;

namespace BMCore.Models
{
    public interface IOrder
    {
        string OrderUuid { get; set; }
        string Exchange { get; set; }
        string Symbol { get; set; }
        string Type { get; set; }
        decimal Quantity { get; set; }
        decimal QuantityRemaining { get; set; }
        decimal Price { get; set; }
        bool IsOpen { get; set; }
        bool IsFilled { get; set; }
    }
}