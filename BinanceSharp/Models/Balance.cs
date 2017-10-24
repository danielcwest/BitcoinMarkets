using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCore.Contracts;
using Newtonsoft.Json;

namespace BinanceSharp.Models
{
    public class BiBalance : ICurrencyBalance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
    }
}
