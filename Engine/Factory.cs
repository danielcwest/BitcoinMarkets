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
                case "Bittrex": return new Bittrex(configExchange.ApiKey, configExchange.Secret);
                case "Binance": return new Binance(configExchange.ApiKey, configExchange.Secret);
                case "Bitz": return new Bitz(configExchange.ApiKey, configExchange.Secret);
                case "CoinExchange": return null;
                case "EtherDelta": return null;
                case "Hitbtc": return new Hitbtc(configExchange.ApiKey, configExchange.Secret);
                case "Liqui": return new Liqui(configExchange.ApiKey, configExchange.Secret);
                case "Livecoin": return new Livecoin(configExchange.ApiKey, configExchange.Secret);
                case "Nova": return new Nova(configExchange.ApiKey, configExchange.Secret);
                case "Poloniex": return new Poloniex(configExchange.ApiKey, configExchange.Secret);
                case "Tidex": return new Tidex(configExchange.ApiKey, configExchange.Secret);
                default: return null;
            }
        }
    }

}