using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Contracts
{
    public interface ICurrencyBalance
    {
        string Currency { get; set; }
        decimal Amount { get; set; }
    }
}
