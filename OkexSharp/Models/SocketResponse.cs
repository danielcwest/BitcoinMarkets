using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkexSharp.Models
{
    public class SocketResponse
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("errorcode")]
        public string Error { get; set; }
        [JsonProperty("result")]
        public bool Result { get; set; }
    }

    public class SocketResponse<T>
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("errorcode")]
        public string Error { get; set; }
        [JsonProperty("data")]
        public T Payload { get; set; }
    }
}
