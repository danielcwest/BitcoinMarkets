using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquiSharp.Models
{
    public class Info
    {
        public long server_time { get; set; }
        public Dictionary<string, object> pairs { get; set; }
    }
}