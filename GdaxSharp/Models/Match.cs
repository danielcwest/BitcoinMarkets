using Core.Contracts;
using Core.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class Match : IMatch
    {
        public string Uuid { get; set; }
        public OrderSide Side { get; set; }
        public string Symbol { get; set; }
        public decimal QuantityFilled { get; set; }
        public string ClientOrderId { get; set; }

        //Since we are the taker the Side is opposit
        public Match(SocketTrade trade)
        {
            Uuid = trade.taker_order_id;
            Side = trade.side == "buy" ? OrderSide.sell : OrderSide.buy;
            Symbol = trade.product_id.Replace("-", "");
            QuantityFilled = trade.size;
        }
    }
}
