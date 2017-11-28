using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface ICurrencyBalance
    {
        string Currency { get; set; }
        decimal Available { get; set; }
        decimal Held { get; set; }
    }
}
