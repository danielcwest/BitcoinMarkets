using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaExchangeSharp.Models
{
    public class OrderbookResponse
    {
        public string status { get; set; }
        public List<OpenOrder> buyorders { get; set; }
        public List<OpenOrder> sellorders { get; set; }
    }
}
