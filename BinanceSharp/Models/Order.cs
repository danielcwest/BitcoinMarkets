using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceSharp.Models
{
    public class BinanceOrder
    {
        public string symbol { get; set; }
        public int orderId { get; set; }
        public string clientOrderId { get; set; }
        public decimal price { get; set; }
        public decimal origQty { get; set; }
        public decimal executedQty { get; set; }
        public string status { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public string stopPrice { get; set; }
        public string icebergQty { get; set; }
        public long time { get; set; }
    }

    public class Order : IOrder
    {
        public string Uuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityFilled { get; set; }
        public decimal CostProceeds { get; set; }
        public decimal AvgRate { get; set; }
        public decimal Fees { get; set; }
        public bool IsFilled { get; set; }
        public bool IsClosed { get; set; }
        public string Side { get; set; }

        public Order(BinanceOrder order)
        {
            this.Uuid = order.orderId.ToString();
            this.Exchange = "Binance";
            this.Symbol = order.symbol;
            this.Quantity = order.origQty;
            this.QuantityFilled = order.executedQty;
            this.CostProceeds = order.executedQty * order.price - ((order.executedQty * order.price) * 0.001m);
            this.AvgRate = order.price;
            this.Fees = 0m;
            this.Side = order.side;
            this.IsFilled = order.status == "FILLED";
            this.IsClosed = order.status == "FILLED" || order.status == "CANCELED" || order.status == "REJECTED" || order.status == "EXPIRED";
        }

        public Order(BinanceOrder order, IEnumerable<BinanceTrade> trades)
        {
            this.Uuid = order.orderId.ToString();
            this.Exchange = "Binance";
            this.Symbol = order.symbol;
            this.Quantity = order.origQty;
            this.QuantityFilled = order.executedQty;
            this.CostProceeds = order.executedQty * order.price;
            this.AvgRate = order.price;
            this.Fees = 0m;

            if (trades.Any())
            {
                this.Quantity = trades.Sum(t => t.qty);
                this.AvgRate = trades.Average(t => t.price);
                this.Fees = trades.Sum(t => t.commission);

                if (order.side == "BUY")
                {
                    this.CostProceeds = (this.Quantity * this.AvgRate) + (this.Quantity * this.AvgRate) * 0.001m;
                }
                else
                {
                    this.CostProceeds = (this.Quantity * this.AvgRate) - (this.Quantity * this.AvgRate) * 0.001m;
                }
            }

            this.Side = order.side.ToLowerInvariant();
            this.IsFilled = order.status == "FILLED";
            this.IsClosed = order.status == "FILLED" || order.status == "CANCELED" || order.status == "REJECTED" || order.status == "EXPIRED";
        }
    }
}
