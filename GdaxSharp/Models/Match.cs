using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class Match : IMatch
    {
        public string Uuid { get; set; }
        public string Side { get; set; }
        public string Symbol { get; set; }
        public decimal QuantityFilled { get; set; }

        //Since we are the taker the Side is opposit
        public Match(SocketTrade trade)
        {
            Uuid = trade.taker_order_id;
            Side = trade.side == "buy" ? "sell" : "buy";
            Symbol = trade.product_id.Replace("-", "");
            QuantityFilled = trade.size;
        }
    }
}
