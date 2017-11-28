using System;
using Core.Contracts;
using BinanceSharp;
using BittrexSharp;
using HitbtcSharp;
using Core.Models;
using Core.Config;
using GdaxSharp;
using OkexSharp;

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
                case "Okex": return new Okex(ExchangeConfig);
                default: return null;
            }
        }

        public static ISocketExchange GetSocketInstance(ExchangeConfig ExchangeConfig)
        {
            switch (ExchangeConfig.Name)
            {
                case "Binance": return new BinanceSocket(ExchangeConfig);
                case "Hitbtc": return new HitbtcSocket(ExchangeConfig);
                case "Gdax": return new GdaxSocket(ExchangeConfig);
                case "Okex": return new OkexSocket(ExchangeConfig);
                default: return null;
            }
        }
    }

}