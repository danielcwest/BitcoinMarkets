using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Contracts
{
    public interface IOrderBook
    {
        List<List<decimal>> asks { get; set; }
        List<List<decimal>> bids { get; set; }
    }
}
