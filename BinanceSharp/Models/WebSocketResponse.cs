using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceSharp.Models
{
    public interface IWebSocketResponse
    {
        string EventType { get; set; }
        long EventTime { get; set; }
    }

    public class WebSocketResponse
    {
        public string EventType { get; set; }

        public long EventTime { get; set; }

    }
}
