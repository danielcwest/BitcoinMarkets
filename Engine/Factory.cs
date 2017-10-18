using System;
using BMCore.Contracts;
using BinanceSharp;
using BittrexSharp;
using BitzSharp;
using HitbtcSharp;
using LiquiSharp;
using LivecoinSharp;
using NovaExchangeSharp;
using PoloniexSharp;
using TidexSharp;
using CoinExchangeSharp;
using BMCore.Models;
using BMCore.Config;

namespace Engine
{
    public class ExchangeFactory
    {
        public static IExchange GetInstance(ExchangeConfig ExchangeConfig)
        {
            switch (ExchangeConfig.Name)
            {
                case "Bittrex": return new Bittrex(ExchangeConfig);
                case "Binance": return new Binance(ExchangeConfig);
                case "Bitz": return new Bitz(ExchangeConfig);
                case "CoinExchange": return null;
                case "EtherDelta": return null;
                case "Hitbtc": return new Hitbtc(ExchangeConfig);
                case "Liqui": return new Liqui(ExchangeConfig);
                case "Livecoin": return new Livecoin(ExchangeConfig);
                case "Nova": return new Nova(ExchangeConfig);
                case "Poloniex": return new Poloniex(ExchangeConfig);
                case "Tidex": return new Tidex(ExchangeConfig);
                default: return null;
            }
        }
    }

}