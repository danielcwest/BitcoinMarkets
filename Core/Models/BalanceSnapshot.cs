using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class BalanceSnapshot
    {
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public decimal BtcPrice { get; set; }

        public BalanceSnapshot(ICurrencyBalance b, ITicker ticker)
        {
            this.Currency = b.Currency;
            this.Total = b.Available + b.Held;
            this.BtcPrice = ticker.Last;
        }
    }
}
