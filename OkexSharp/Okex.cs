using Core.Contracts;
using System;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Config;
using RestEase;
using System.Linq;
using Core;
using System.Security.Cryptography;
using System.Text;

namespace OkexSharp
{
    public class Okex : IExchange
    {
        IOkexApi _okex;
        ExchangeConfig _config;
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Okex(ExchangeConfig config)
        {
            _config = config;
            name = config.Name;
            _okex = RestClient.For<IOkexApi>("https://www.okex.com/");
        }

        public Task CancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> CancelOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<IOrder> CheckOrder(string uuid, string symbol = "")
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> FillOrKill(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICurrencyBalance>> GetBalances()
        {
            throw new NotImplementedException();
        }

        public Task<IDepositAddress> GetDepositAddress(string currency)
        {
            throw new NotImplementedException();
        }

        public Task<IWithdrawal> GetWithdrawal(string uuid)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> ImmediateOrCancel(string side, string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> LimitBuy(string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> LimitSell(string symbol, decimal quantity, decimal price)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> MarketBuy(string symbol, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> MarketSell(string symbol, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderBook> OrderBook(string symbol)
        {
            var book = new OrderBook(symbol);

            if (!symbol.Contains("_"))
                symbol = GetMarketNameFromSymbol(symbol);

            var depth = await _okex.GetOrderBook(symbol);

            book.asks = depth.asks.Select((a, index) => new OrderBookEntry() { price = a[0], quantity = a[1] }).ToList();
            book.bids = depth.bids.Select((a, index) => new OrderBookEntry() { price = a[0], quantity = a[1] }).ToList();

            book.asks = Helper.SumOrderEntries(book.asks);
            book.bids = Helper.SumOrderEntries(book.bids);

            return book;
        }

        public Task<IEnumerable<ISymbol>> Symbols()
        {
            throw new NotImplementedException();
        }

        public Task<ITicker> Ticker(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<IAcceptedAction> Withdraw(string currency, decimal quantity, string address, string paymentId = null)
        {
            throw new NotImplementedException();
        }

        //return OKEX market name in the form of BTC-XMR or ETH-XRP
        public static string GetMarketNameFromSymbol(string symbol)
        {
            if (symbol.EndsWith("BTC"))
            {
                return string.Format("{0}_btc", symbol.Replace("BTC", "").ToLowerInvariant());
            }
            else if (symbol.EndsWith("ETH"))
            {
                return string.Format("{0}_eth", symbol.Replace("ETH", "").ToLowerInvariant());
            }
            else
            {
                return null;
            }
        }

        public string CalculateSignature(string parameters)
        {
            string parametersAndSecret = string.Format("{0}&secret_key={1}", parameters, _config.Secret);
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(parameters));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();

            }
        }

        public ISocketExchange GetSocket()
        {
            return new OkexSocket(_config);
        }

        public Task<IEnumerable<ITicker>> MarketSummaries()
        {
            throw new NotImplementedException();
        }
    }
}
