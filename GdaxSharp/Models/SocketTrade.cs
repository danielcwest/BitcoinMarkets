using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class SocketTrade : SocketResponse
    {
        public int trade_id { get; set; }
        public int sequence { get; set; }
        public string maker_order_id { get; set; }
        public string taker_order_id { get; set; }
        public DateTime time { get; set; }
        public string product_id { get; set; }
        public decimal size { get; set; }
        public decimal price { get; set; }
        public string side { get; set; }
    }
}
