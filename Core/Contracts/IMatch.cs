using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface IMatch
    {
        string Uuid { get; set; }
        string Side { get; set; }
        string Symbol { get; set; }
        decimal QuantityFilled { get; set; }
    }
}
