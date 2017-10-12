using System;
using System.Collections.Generic;
using System.Linq;

namespace BMCore.Models
{
    public class ConfigExchange
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public decimal Fee { get; set; }
        public bool Enabled { get; set; }
    }
}