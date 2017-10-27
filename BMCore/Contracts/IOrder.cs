using System;

namespace BMCore.Contracts
{
    public interface IOrder
    {
        string OrderUuid { get; set; }
        string Exchange { get; set; }
        string Symbol { get; set; }
        decimal Quantity { get; set; }
        decimal Cost { get; set; }
        bool IsFilled { get; set; }
    }
}