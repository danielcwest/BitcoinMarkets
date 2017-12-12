using Core.Domain;
using Core.Engine;
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
        string ClientOrderId { get; set; }
    }
}
