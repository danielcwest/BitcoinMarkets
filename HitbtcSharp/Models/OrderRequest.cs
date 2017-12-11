using System;
using System.Collections.Generic;
using System.Text;

namespace HitbtcSharp.Models
{
    public class OrderRequest
    {
        public string clientOrderId { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public string timeInForce { get; set; }
        public bool strictValidate { get; set; }
    }

    public class OrderReplaceRequest
    {
        public string clientOrderId { get; set; }
        public string requestClientId { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
    }
}
