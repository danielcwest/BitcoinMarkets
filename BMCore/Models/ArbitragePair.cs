using System;
using System.Collections.Generic;
using System.Linq;
using BMCore.Config;
using BMCore.Contracts;
using BMCore.DbService;

namespace BMCore.Models
{
    public class ArbitrageSymbol : ISymbol
    {
        public string LocalSymbol { get; set; }
        public string ExchangeSymbol { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
    }

    public class ArbitragePair
    {
        public int Id { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrency { get; set; }
        public string Symbol { get; set; }
        public string BaseSymbol { get; set; }
        public string CounterSymbol { get; set; }
        public DateTime LastRunUtc { get; set; }
        public decimal TradeThreshold { get; set; }
        public decimal SpreadThreshold { get; set; }
        public decimal BaseBaseWithdrawalFee { get; set; }
        public decimal BaseMarketWithdrawalFee { get; set; }
        public decimal CounterBaseWithdrawalFee { get; set; }
        public decimal CounterMarketWithdrawalFee { get; set; }
        public decimal BaseExchangeFee { get; set; }
        public decimal CounterExchangeFee { get; set; }

        //Market Data
        public IMarket baseMarket { get; set; }
        public OrderBook baseBook { get; set; }
        public IMarket counterMarket { get; set; }
        public OrderBook counterBook { get; set; }

        public ArbitragePair(DbArbitragePair dbPair)
        {
            this.Id = dbPair.Id;
            this.BaseCurrency = dbPair.BaseCurrency;
            this.MarketCurrency = dbPair.MarketCurrency;
            this.Symbol = dbPair.Symbol;
            this.BaseSymbol = dbPair.BaseSymbol;
            this.CounterSymbol = dbPair.CounterSymbol;
            this.LastRunUtc = dbPair.LastRunUtc;
            this.TradeThreshold = dbPair.TradeThreshold;
            this.SpreadThreshold = dbPair.SpreadThreshold;
            this.BaseExchangeFee = dbPair.BaseExchangeFee;
            this.CounterExchangeFee = dbPair.CounterExchangeFee;
            this.BaseBaseWithdrawalFee = dbPair.BaseBaseWithdrawalFee;
            this.BaseMarketWithdrawalFee = dbPair.BaseMarketWithdrawalFee;
            this.CounterBaseWithdrawalFee = dbPair.CounterBaseWithdrawalFee;
            this.CounterMarketWithdrawalFee = dbPair.CounterMarketWithdrawalFee;
        }

        public ISymbol GetBaseSymbol()
        {
            return new ArbitrageSymbol()
            {
                BaseCurrency = this.BaseCurrency,
                MarketCurrency = this.MarketCurrency,
                ExchangeSymbol = this.BaseSymbol,
                LocalSymbol = this.Symbol
            };
        }

        public ISymbol GetCounterSymbol()
        {
            return new ArbitrageSymbol()
            {
                BaseCurrency = this.BaseCurrency,
                MarketCurrency = this.MarketCurrency,
                ExchangeSymbol = this.CounterSymbol,
                LocalSymbol = this.Symbol
            };
        }

    }
}