using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using Core.Contracts;
using Newtonsoft.Json;
using Core.Engine;

namespace HitbtcSharp.Models
{
    public class SocketOrder : ITrade
    {
        public string Uuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal CostProceeds { get; set; }
        public decimal AvgRate { get; set; }
        public decimal Fees { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFilled { get; set; }
        public OrderSide Side { get; set; }
        public string ClientOrderId { get; set; }
        public decimal QuantityFilled { get; set; }
        public bool IsClosed { get; set; }

        public int TradeId { get; set; }
        public decimal TradeQuantityFilled { get; set; }

        public SocketOrder(HitbtcSharp.Models.HitbtcOrderV2 order)
        {
            this.Uuid = order.Uuid.ToString();
            this.Exchange = "Hitbtc";
            this.Symbol = order.symbol;
            this.Type = order.type;
            this.Quantity = order.quantity;
            this.QuantityFilled = order.cumQuantity;
            this.CostProceeds = 0m;
            this.AvgRate = 0m;
            this.Fees = 0m;
            this.Side = order.side == "buy" ? OrderSide.buy : OrderSide.sell; 
            this.ClientOrderId = order.clientOrderId;
            this.IsOpen = order.status == "new" || order.status == "partiallyFilled";
            this.IsFilled = order.status == "filled";
            this.IsClosed = order.status == "filled" || order.status == "canceled" || order.status == "expired";
            this.TradeId = order.tradeId;
            this.TradeQuantityFilled = order.tradeQuantity;
        }
    }

    public class Order : IOrder
    {
        public string Uuid { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal CostProceeds { get; set; }
        public decimal AvgRate { get; set; }
        public decimal Fees { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFilled { get; set; }
        public OrderSide Side { get; set; }
        public string ClientOrderId { get; set; }
        public decimal QuantityFilled { get; set; }
        public bool IsClosed { get; set; }

        public Order(HitbtcSharp.Models.HitbtcOrder order)
        {
            this.Uuid = order.Uuid.ToString();
            this.Exchange = "Hitbtc";
            this.Symbol = order.symbol;
            this.Type = order.type;
            this.Quantity = order.quantity.HasValue ? order.quantity.Value : 0m;
            this.QuantityFilled = order.cumQuantity.HasValue ? order.cumQuantity.Value : 0m;
            this.CostProceeds = 0m;
            this.AvgRate = 0m;
            this.Fees = 0m;
            this.Side = order.side == "buy" ? OrderSide.buy : OrderSide.sell; 
            this.ClientOrderId = order.clientOrderId;
            this.IsOpen = order.status == "new" || order.status == "partiallyFilled";
            this.IsFilled = order.status == "filled";
            this.IsClosed = order.status == "filled" || order.status == "canceled" || order.status == "expired";
        }

        public Order(HitbtcSharp.Models.HitbtcOrderV2 order)
        {
            this.Uuid = order.Uuid.ToString();
            this.Exchange = "Hitbtc";
            this.Symbol = order.symbol;
            this.Type = order.type;
            this.Quantity = order.quantity;
            this.QuantityFilled = order.cumQuantity;
            this.CostProceeds = 0m;
            this.AvgRate = 0m;
            this.Fees = 0m;
            this.Side = order.side == "buy" ? OrderSide.buy : OrderSide.sell;
            this.ClientOrderId = order.clientOrderId;
            this.IsOpen = order.status == "new" || order.status == "partiallyFilled";
            this.IsFilled = order.status == "filled";
            this.IsClosed = order.status == "filled" || order.status == "canceled" || order.status == "expired";
        }

        public Order(HitbtcSharp.Models.HitbtcOrder order, IEnumerable<Trade> trades)
        {
            this.Uuid = order.Uuid.ToString();
            this.Exchange = "Hitbtc";
            this.Symbol = order.symbol;
            this.QuantityFilled = order.cumQuantity.HasValue ? order.cumQuantity.Value : 0m;

            if (trades.Any())
            {
                this.Quantity = trades.Sum(t => t.quantity);
                this.AvgRate = trades.Average(t => t.price);
                this.Fees = trades.Sum(t => t.fee);

                //this.Cost = trades.Sum(t => (t.quantity * t.price) + t.fee);

                if (order.side == "buy")
                {
                    this.CostProceeds = trades.Sum(t => (t.quantity * t.price) + t.fee);
                }
                else
                {
                    this.CostProceeds = trades.Sum(t => (t.quantity * t.price) - t.fee);
                }
            }


            this.IsOpen = order.status == "new" || order.status == "partiallyFilled";
            this.IsFilled = order.status == "filled";
            this.IsClosed = order.status == "filled" || order.status == "canceled" || order.status == "expired";
            this.Side = order.side == "buy" ? OrderSide.buy : OrderSide.sell;
        }

        public Order(IEnumerable<Trade> trades)
        {
            if (trades.Any())
            {
                this.Uuid = trades.FirstOrDefault().orderId;
                this.Exchange = "Hitbtc";
                this.Symbol = trades.FirstOrDefault().symbol;
                this.Side = trades.FirstOrDefault().side == "buy" ? OrderSide.buy : OrderSide.sell;
                this.QuantityFilled = trades.Sum(t => t.quantity);
                this.Quantity = trades.Sum(t => t.quantity);
                this.AvgRate = trades.Average(t => t.price);
                this.Fees = trades.Sum(t => t.fee);

                //this.Cost = trades.Sum(t => (t.quantity * t.price) + t.fee);

                if (this.Side == OrderSide.buy)
                {
                    this.CostProceeds = trades.Sum(t => (t.quantity * t.price) + t.fee);
                }
                else
                {
                    this.CostProceeds = trades.Sum(t => (t.quantity * t.price) - t.fee);
                }

                this.IsOpen = false;
                this.IsFilled = true;
                this.IsClosed = true;
            }
        }
    }
}