using System;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using Core.Contracts;
using Core.DbService;

namespace Core.Models
{
    public class ArbitrageGroup
    {
        public string BaseExchange { get; set; }
        public string CounterExchange { get; set; }
        public IEnumerable<ArbitragePair> Markets { get; set; }
    }
}