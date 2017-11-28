using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class SocketResponse
    {
        public string type { get; set; }
    }

    public class Channel : SocketResponse
    {
        public string name { get; set; }
        public string[] product_ids { get; set; }
    }

    public class SnapShotResponse : SocketResponse
    {
        public string product_id { get; set; }
        public IEnumerable<string[]> bids { get; set; }
        public IEnumerable<string[]> asks { get; set; }
    }

    public class BookUpdateResponse : SocketResponse
    {
        public string product_id { get; set; }
        public IEnumerable<string[]> changes { get; set; }
    }

    public class TradeResponse : SocketResponse
    {
        public int trade_id { get; set; }
        public int sequence { get; set; }
        public string maker_order_id { get; set; }
        public string taker_order_id { get; set; }
        public DateTime time { get; set; }
        public string product_id { get; set; }
        public string size { get; set; }
        public string price { get; set; }
        public string side { get; set; }
    }
}
