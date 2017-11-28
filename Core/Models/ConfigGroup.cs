using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class ConfigGroup
    {
        public string name { get; set; }
        public string description { get; set; }
        public string[] symbols { get; set; }
    }
}