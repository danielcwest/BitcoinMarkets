using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;

namespace BMCore.Models
{
    public class ArbitrageGroup
    {
        public string BaseExchange { get; set; }
        public string CounterExchange { get; set; }
        public IEnumerable<ArbitragePair> Markets { get; set; }
    }
}