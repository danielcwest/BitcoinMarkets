using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;

namespace GdaxSharp.Models
{
    public class GdaxBalance : ICurrencyBalance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Held { get; set; }

        public GdaxBalance(Account a)
        {
            Currency = a.currency;
            Available = a.available;
            Held = a.hold;
        }
    }
}
