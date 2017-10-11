using System;

namespace NovaExchangeSharp.Models
{

    public class OpenOrder
    {
        public string currency { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
        public string basecurrency { get; set; }
        public decimal baseamount { get; set; }
    }
}