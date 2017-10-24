using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;

namespace BMCore.Models
{
    public class ArbitrageOpportunity
    {
        public string Type { get; set; } //basebuy, basesell
        public decimal BasePrice { get; set; }
        public decimal CounterPrice { get; set; }
        public decimal Spread { get; set; }
    }
}