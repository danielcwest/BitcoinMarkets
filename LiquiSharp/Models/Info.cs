using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquiSharp.Models
{
    public class LiquiSymbol
    {
        public int? decimal_places { get; set; }
        public decimal? min_price { get; set; }
        public decimal? max_price { get; set; }
        public decimal? min_amount { get; set; }
        public int? hidden { get; set; }
        public decimal? fee { get; set; }
    }
    public class Info
    {
        public long server_time { get; set; }
        public Dictionary<string, LiquiSymbol> pairs { get; set; }
    }
}