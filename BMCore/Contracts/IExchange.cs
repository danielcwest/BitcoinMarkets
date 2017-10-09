using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCore.Models;

namespace BMCore.Contracts
{
    public interface IExchange
    {

        Task<IEnumerable<IMarket>> MarketSummaries();

        Task<OrderBook> OrderBook(string symbol);

        Task<IMarket> MarketSummary(string symbol);
    }

}