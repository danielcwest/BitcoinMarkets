using System;
using System.Collections.Generic;

namespace BMCore.Config
{
    public class CurrencyConfig
    {
        public string Name { get; set; }
        public decimal TradeThreshold { get; set; }
        public decimal SpreadThreshold { get; set; }
        public decimal WithdrawalThreshold { get; set; }
        public bool Enabled { get; set; }
    }

    public class GmailConfig
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string To { get; set; }
    }

    public class ExchangeConfig
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public decimal Fee { get; set; }
        public bool Enabled { get; set; }
    }

    public class ArbitrageConfig
    {
        public List<CurrencyConfig> BaseCurrencies { get; set; }
        public GmailConfig Gmail { get; set; }
        public List<ExchangeConfig> Exchanges { get; set; }
    }
}