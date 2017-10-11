using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitzSharp.Models
{
    public class Depth
    {
        public List<List<decimal>> asks { get; set; }
        public List<List<decimal>> bids { get; set; }
        public long date { get; set; }
    }
}