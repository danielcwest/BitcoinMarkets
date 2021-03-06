using System;
using HitbtcSharp.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestEase;
using System.Collections.Generic;

namespace HitbtcSharp
{
    public interface IHitbtcApi
    {
        [Header("Authorization")]
        string Authorization { get; set; }
        [Get("/api/2/public/symbol")]
        Task<IEnumerable<SymbolV2>> GetSymbols();

        [Get("/api/2/public/ticker")]
        Task<IEnumerable<Ticker>> GetTickers();

        [Get("/api/2/public/ticker/{symbol}")]
        Task<Ticker> GetTicker([Path] string symbol);

        [Get("/api/2/public/orderbook/{symbol}?limit=25")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);


        [Get("/api/2/account/crypto/address/{currency}")]
        Task<Address> GetAddress([Path] string currency);

        [Get("/api/2/account/balance")]
        Task<IEnumerable<HitBalance>> GetMainBalances();

        [Get("/api/2/trading/balance")]
        Task<IEnumerable<HitBalance>> GetTradingBalances();

        [Post("/api/2/order")]
        Task<HitbtcOrder> PlaceOrder([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Delete("/api/2/order/{clientOrderId}")]
        Task<HitbtcOrder> CancelOrder([Path] string clientOrderId);

        [Delete("/api/2/order?symbol={symbol}")]
        Task<IEnumerable<HitbtcOrder>> CancelOrders([Path] string symbol);

        [Get("/api/2/history/order?limit=100&offset={offset}")]
        Task<IEnumerable<HitbtcOrder>> GetOrders([Path] int offset);

        [Get("/api/2/history/trades?symbol={symbol}")]
        Task<IEnumerable<Trade>> GetTrades([Path] string symbol);

        [Get("/api/2/history/order/{orderId}/trades")]
        Task<IEnumerable<Trade>> GetTradesForOrder([Path] string orderId);

        [Get("/api/2/order/{clientOrderId}")]
        Task<HitbtcOrder> GetOrder([Path] string clientOrderId);

        [Post("/api/2/account/crypto/withdraw")]
        Task<CryptoTransaction> Withdraw([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Delete("/api/2/account/crypto/withdraw/{id}")]
        Task<CryptoTransaction> DeleteWithdrawal([Path] string id);

        [Get("/api/2/account/transactions/{id}")]
        Task<Transaction> GetWithdrawal([Path] string id);

        [Post("/api/2/account/transfer")]
        Task<CryptoTransaction> Transfer([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/payout")]
        Task<PayoutTransaction> WithdrawV1([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/transfer_to_trading")]
        Task<PayoutTransaction> TransferToTrading([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/transfer_to_main")]
        Task<PayoutTransaction> TransferToMain([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Get("/api/1/trading/orders/recent?nonce={nonce}&apikey={apiKey}&max_results={limit}")]
        Task<OrderResponse> GetOrdersV1([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] int limit);


    }
}