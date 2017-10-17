using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HitbtcSharp;
using HitbtcSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using RestEase;
using BMCore.Contracts;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class HitbtcController : Controller
    {
        IConfiguration _iconfiguration;
        Hitbtc _hitbtc;
        public HitbtcController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            var exchanges = new List<ConfigExchange>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Hitbtc");
            _hitbtc = new Hitbtc(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _hitbtc.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _hitbtc.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _hitbtc.OrderBook(symbol);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _hitbtc.MarketSummaries();
            return summaries.Select(s => s.MarketName);
        }

        [HttpPost]
        public async Task<IOrder> Sell(string marketCurrency, string baseCurrency, decimal quantity, decimal price)
        {
            //Has Balance to Sell
            var b = await _hitbtc.GetBalance(marketCurrency);

            if (b.Available < quantity)
                throw new Exception("Insufficient Balance");

            //Sell On Exchange
            var result = await _hitbtc.Sell("", string.Format("{0}-{1}", baseCurrency, marketCurrency), quantity, price);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            var order = await _hitbtc.CheckOrder(result.Uuid);

            return await Task.FromResult<IOrder>(order);
        }

        [HttpPost]
        public async Task<IOrder> Buy(string marketCurrency, string baseCurrency, decimal quantity, decimal price)
        {
            //Has Balance to Sell
            var b = await _hitbtc.GetBalance(marketCurrency);

            if (b.Available < quantity)
                throw new Exception("Insufficient Balance");

            //Sell On Exchange
            var result = await _hitbtc.Buy("", string.Format("{0}-{1}", baseCurrency, marketCurrency), quantity, price);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            var order = await _hitbtc.CheckOrder(result.Uuid);

            return await Task.FromResult<IOrder>(order);
        }

        [HttpGet]
        public async Task<IWithdrawal> Withdraw(string currency, decimal quantity, string address)
        {
            //Has Balance to Sell
            var b = await _hitbtc.GetBalance(currency);

            if (b.Available < quantity)
                throw new Exception("Insufficient Balance");

            //Sell On Exchange
            var result = await _hitbtc.Withdraw(currency, quantity, address);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            return await _hitbtc.GetWithdrawal(result.Uuid);
        }

    }
}