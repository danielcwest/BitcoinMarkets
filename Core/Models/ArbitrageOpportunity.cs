using System;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using Core.Contracts;
using Core.DbService;

namespace Core.Models
{
    public class ArbitrageOpportunity
    {
        public string Type { get; set; } //basebuy, basesell
        public decimal BasePrice { get; set; }
        public decimal CounterPrice { get; set; }
        public decimal Spread { get; set; }
    }
}