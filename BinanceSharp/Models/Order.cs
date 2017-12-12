using Core.Contracts;
using Core.Domain;
using Core.Engine;
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
            this.Side = order.side.ToLowerInvariant();
            this.IsFilled = order.status == "FILLED";
            this.IsClosed = order.status == "FILLED" || order.status == "CANCELED" || order.status == "REJECTED" || order.status == "EXPIRED";
        }

        public Order(BinanceOrder order, IEnumerable<BinanceTrade> trades, ITicker bnbTicker)
        {
            if (!trades.Any())
                throw new Exception("NO TRADES REMOVE ME MAYBE");

            this.Uuid = order.orderId.ToString();
            this.Exchange = "Binance";
            this.Symbol = order.symbol;
            this.Quantity = order.origQty;
            this.QuantityFilled = order.executedQty;
            this.CostProceeds = order.executedQty * order.price;
            this.AvgRate = order.price;
            this.Fees = 0m;
            this.Side = order.side.ToLowerInvariant();

            if (trades.Any())
            {
                this.Quantity = trades.Sum(t => t.qty);
                this.AvgRate = trades.Average(t => t.price);
                this.Fees = trades.Sum(t => t.commission);

                if(trades.FirstOrDefault().commissionAsset == "BNB")
                {
                    this.Fees *= bnbTicker.Last;

                }
                else if (trades.FirstOrDefault().commissionAsset != "BTC" && trades.FirstOrDefault().commissionAsset != "ETH")
                {
                    this.Fees *= this.AvgRate;
                }

                decimal rawCost = this.QuantityFilled * this.AvgRate;

                if (this.Side == OrderSide.Buy)
                {
                    this.CostProceeds = rawCost + Fees;
                }
                else
                {
                    this.CostProceeds = rawCost - Fees;
                }
            }

            this.IsFilled = order.status == "FILLED";
            this.IsClosed = order.status == "FILLED" || order.status == "CANCELED" || order.status == "REJECTED" || order.status == "EXPIRED";
        }
    }
}
