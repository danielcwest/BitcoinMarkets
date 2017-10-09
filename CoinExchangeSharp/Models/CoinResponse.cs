using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoinExchangeSharp.Models
{
    public class CoinResponse<T>
    {
        public string success { get; set; }
        public string request { get; set; }
        public string message { get; set; }
        public List<T> result { get; set; }
    }
}