using GdaxSharp.Models;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GdaxSharp
{

    [Header("User-Agent", "NetCore API")]
    public interface IGdaxApi
    {
        [Header("CB-ACCESS-KEY")]
        string ApiKey { get; set; }

        [Header("CB-ACCESS-PASSPHRASE")]
        string PassPhrase { get; set; }

        #region Public
        [Get("/products")]
        Task<IEnumerable<Product>> Products();

        [Get("/products/{symbol}/ticker")]
        Task<Ticker> Ticker([Path] string symbol);

        [Get("/products/{symbol}/book?level=2")]
        Task<OrderBookResponse> GetOrderBook([Path] string symbol);
        #endregion

        #region Accounts
        [Get("/accounts")]
        Task<IEnumerable<Account>> Accounts([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp);

        [Get("/coinbase-accounts")]
        Task<IEnumerable<CoinbaseAccount>> CoinbaseAccounts([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp);

        #endregion

        #region Orders  

        [Get("/orders/{orderId}")]
        Task<GdaxOrder> Order([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Path] string orderId);

        [Get("/orders?status=all")]
        Task<IEnumerable<GdaxOrder>> Orders([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp);

        [Post("/orders")]
        Task<GdaxOrder> NewOrder([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Body] object data);

        [Delete("/orders/{orderId}")]
        Task CancelOrder([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Path] string orderId);

        [Delete("/orders?product_id={symbol}")]
        Task<IEnumerable<string>> CancelOrders([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Path] string symbol);

        [Get("/fills?order_id={orderId}")]
        Task<IEnumerable<Fill>> FillsForOrder([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Path] string orderId);
        #endregion

        #region Transfers

        [Post("/withdrawals/crypto")]
        Task<WithdrawalResponse> Withdraw([Header("CB-ACCESS-SIGN")] string sig, [Header("CB-ACCESS-TIMESTAMP")] double timestamp, [Body] object data);


        #endregion

    }
}
