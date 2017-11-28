using System;
using System.Collections.Generic;
using Core.Contracts;
using Newtonsoft.Json;

namespace HitbtcSharp.Models
{

    public class ExecutionReport : IAcceptedAction
    {
        [JsonProperty("orderId")]
        public string Uuid { get; set; }
        public string clientOrderId { get; set; }
        public string execReportType { get; set; }
        public string orderStatus { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public long? timestamp { get; set; }
        public double price { get; set; }
        public int? quantity { get; set; }
        public string type { get; set; }
        public string timeInForce { get; set; }
        public int? lastQuantity { get; set; }
        public int? lastPrice { get; set; }
        public int? leavesQuantity { get; set; }
        public int? cumQuantity { get; set; }
        public int? averagePrice { get; set; }
    }

    public class ExecutionResponse
    {
        public ExecutionReport ExecutionReport { get; set; }
    }

    public class ExecutionResponseMany
    {
        public List<ExecutionReport> ExecutionReport { get; set; }
    }


}