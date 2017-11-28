using System;
using System.Collections.Generic;
using System.Text;

namespace GdaxSharp.Models
{
    public class SocketRequest
    {
        public string type { get; set; }
        public List<string> product_ids { get; set; }
        public List<string> channels { get; set; }
        public string signature { get; set; }
        public string key { get; set; }
        public string passphrase { get; set; }
        public string timestamp { get; set; }
    }
}
