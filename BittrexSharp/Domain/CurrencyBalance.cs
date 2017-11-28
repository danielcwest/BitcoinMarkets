using System;
using System.Collections.Generic;
using System.Text;
using Core.Contracts;

namespace BittrexSharp
{
    public class BittrexBalance
    {
        public string Currency { get; set; }
        public decimal Amouunt { get; set; }
        public decimal Available { get; set; }
        public decimal Pending { get; set; }
        public string CryptoAddress { get; set; }
        public bool Requested { get; set; }
        public string Uuid { get; set; }
    }

    public class CurrencyBalance : ICurrencyBalance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Held { get; set; }

        public CurrencyBalance(BittrexBalance b)
        {
            Currency = b.Currency;
            Available = b.Available;
            Held = b.Pending;

        }
    }
}
