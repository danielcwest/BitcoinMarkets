using BMCore.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class GdaxOrder
    {
        public Guid id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public string product_id { get; set; }
        public string side { get; set; }
        public string stp { get; set; }
        public string type { get; set; }
        public string time_in_force { get; set; }
        public string post_only { get; set; }
        public string created_at { get; set; }
        public string done_reason { get; set; }
        public decimal fill_fees { get; set; }
        public decimal filled_size { get; set; }
        public decimal executed_value { get; set; }
        public string status { get; set; }
        public bool settled { get; set; }
        public string specified_funds { get; set; }
    }

    public class Order : IOrder, IAcceptedAction
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
        public string ExchangeStatus { get; set; }

        public Order(GdaxOrder o)
        {
            Uuid = o.id.ToString();
            Exchange = "Gdax";
            Symbol = o.product_id;
            Side = o.side;
            Quantity = o.size == 0 ? o.filled_size : o.size;
            QuantityFilled = o.filled_size;
            IsFilled = o.settled;
            IsClosed = o.settled;
            CostProceeds = o.executed_value + o.fill_fees;
            ExchangeStatus = o.status;
            Fees = o.fill_fees;
            AvgRate = o.price;
        }
    }
}
