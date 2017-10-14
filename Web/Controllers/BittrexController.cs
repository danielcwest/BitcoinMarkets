using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BittrexSharp;
using BittrexSharp.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BMCore.Models;
using BMCore.Contracts;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class BittrexController : Controller
    {
        IConfiguration _iconfiguration;
        Bittrex _bittrex;
        public BittrexController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;

            var exchanges = new List<ConfigExchange>();
            _iconfiguration.GetSection("Exchanges").Bind(exchanges);
            var config = exchanges.Find(e => e.Name == "Bittrex");
            _bittrex = new Bittrex(config);
        }

        [HttpGet]
        public async Task<IEnumerable<IMarket>> MarketSummaries()
        {
            return await _bittrex.MarketSummaries();
        }

        [HttpGet]
        public async Task<IMarket> MarketSummary(string symbol)
        {
            return await _bittrex.MarketSummary(symbol);
        }

        [HttpGet]
        public async Task<BMCore.Models.OrderBook> OrderBook(string symbol)
        {
            return await _bittrex.OrderBook(symbol);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Symbols()
        {
            var summaries = await _bittrex.Symbols();
            return summaries.Select(s => s.LocalSymbol);
        }

        [HttpPost]
        public async Task<IOrder> Sell(string marketCurrency, string baseCurrency, decimal quantity, decimal price)
        {
            //Has Balance to Sell
            var b = await _bittrex.GetBalance(marketCurrency);

            if (b.Balance < quantity)
                throw new Exception("Insufficient Balance");

            //Sell On Exchange
            var result = await _bittrex.Sell("", string.Format("{0}-{1}", baseCurrency, marketCurrency), quantity, price);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            var order = await _bittrex.CheckOrder(result.Uuid);

            return await Task.FromResult<IOrder>(order);
        }

        [HttpPost]
        public async Task<IOrder> Buy(string marketCurrency, string baseCurrency, decimal quantity, decimal price)
        {
            //Has Balance to Sell
            var b = await _bittrex.GetBalance(marketCurrency);

            if (b.Balance < quantity)
                throw new Exception("Insufficient Balance");

            //Buy On Exchange 
            var result = await _bittrex.Buy("", string.Format("{0}-{1}", baseCurrency, marketCurrency), quantity, price);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            var order = await _bittrex.CheckOrder(result.Uuid);

            return await Task.FromResult<IOrder>(order);
        }

        [HttpPost]
        public async Task<IWithdrawal> Withdraw(string currency, decimal quantity, string address)
        {
            //Has Balance to Sell
            var b = await _bittrex.GetBalance(currency);

            if (b.Balance < quantity)
                throw new Exception("Insufficient Balance");

            //Sell On Exchange
            var result = await _bittrex.Withdraw(currency, quantity, address);

            //If Order went through, Withdraw Base Currency to Arb Exchange (only amount bought on other exchange)
            var w = await _bittrex.GetWithdrawal(result.Uuid);

            return await Task.FromResult<IWithdrawal>(w);
        }
    }
}