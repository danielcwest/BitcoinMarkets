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

namespace Engine
{
    public class ExchangeFactory
    {
        public static IExchange GetInstance(ConfigExchange configExchange)
        {
            switch (configExchange.Name)
            {
                case "Bittrex": return new Bittrex(configExchange);
                case "Binance": return new Binance(configExchange);
                case "Bitz": return new Bitz(configExchange);
                case "CoinExchange": return null;
                case "EtherDelta": return null;
                case "Hitbtc": return new Hitbtc(configExchange);
                case "Liqui": return new Liqui(configExchange);
                case "Livecoin": return new Livecoin(configExchange);
                case "Nova": return new Nova(configExchange);
                case "Poloniex": return new Poloniex(configExchange);
                case "Tidex": return new Tidex(configExchange);
                default: return null;
            }
        }
    }

}