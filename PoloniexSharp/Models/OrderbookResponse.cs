using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoloniexSharp.Models
{
    public class OrderbookResponse
    {
        public List<List<decimal>> asks { get; set; }
        public List<List<decimal>> bids { get; set; }
        public int isFrozen { get; set; }
        public int seq { get; set; }
    }
}