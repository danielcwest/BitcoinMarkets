using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkexSharp.Models
{
    public class SocketRequest
    {
        [JsonProperty("event")]
        public string Event { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
