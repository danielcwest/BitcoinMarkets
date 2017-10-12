using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Models
{
    public interface ICurrencyBalance
    {
        string Currency { get; set; }
        decimal Balance { get; set; }
    }
}
