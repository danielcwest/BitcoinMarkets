using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NovaExchangeSharp.Models
{
    public class NovaResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Ticker> markets { get; set; }
    }
}