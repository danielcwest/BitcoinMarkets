using System;
using BMCore.Contracts;
using BinanceSharp;
using BittrexSharp;
using HitbtcSharp;
using BMCore.Models;
using BMCore.Config;
using GdaxSharp;

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
                case "Hitbtc": return new Hitbtc(ExchangeConfig);
                case "Gdax": return new Gdax(ExchangeConfig);
                default: return null;
            }
        }
    }

}