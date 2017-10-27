using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitbtcSharp.Models
{
    public class Trade
    {
        public int id { get; set; }
        public string clientOrderId { get; set; }
        public string orderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal fee { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class SpecifiedTrade
    {
        public object date { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
        public int tid { get; set; }
        public string side { get; set; }
    }

    public class Trades
    {
        public List<Trade> trades { get; set; }
    }

    public class SpecifiedTrades
    {
        public List<SpecifiedTrade> trades { get; set; }
    }
}
