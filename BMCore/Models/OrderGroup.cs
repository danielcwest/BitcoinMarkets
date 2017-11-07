using BMCore.DbService;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMCore.Models
{
    public class OrderGroup
    {
        public string BaseExchange { get; set; }
        public string CounterExchange { get; set; }
        public IEnumerable<DbMakerOrder> Orders { get; set; }
    }
}
