using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface ITrade : IOrder
    {
        int TradeId { get; set; }
        decimal TradeQuantityFilled { get; set; }
    }
}
