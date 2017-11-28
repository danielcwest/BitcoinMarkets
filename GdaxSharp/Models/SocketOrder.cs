using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class SocketOrder : SocketResponse
    {
        public DateTime time { get; set; }
        public string product_id { get; set; }
        public int sequence { get; set; }
        public string order_id { get; set; }
        public decimal size { get; set; }
        public decimal price { get; set; }
        public string side { get; set; }
        public string order_type { get; set; }
    }
}
