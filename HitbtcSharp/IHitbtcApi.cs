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
        [Get("/api/2/public/symbol")]
        Task<IEnumerable<SymbolV2>> GetSymbols();

        [Get("/api/2/public/ticker")]
        Task<IEnumerable<Ticker>> GetTickers();

        [Get("/api/2/public/ticker/{symbol}")]
        Task<Ticker> GetTicker([Path] string symbol);

        [Get("/api/2/public/orderbook/{symbol}?limit=25")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);


        [Get("/api/1/payment/address/{currency}?nonce={nonce}&apikey={apiKey}")]
        Task<Address> GetAddress([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] string currency);

        [Get("/api/1/payment/balance?nonce={nonce}&apikey={apiKey}")]
        Task<MultiCurrencyBalance> GetMainBalances([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Get("/api/1/trading/balance?nonce={nonce}&apikey={apiKey}")]
        Task<MultiCurrencyBalance> GetTradingBalances([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Post("/api/1/trading/new_order")]
        Task<HitbtcOrder> PlaceOrder([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/trading/cancel_order?nonce={nonce}&apikey={apiKey}&clientOrderId={clientOrderId}")]
        Task<ExecutionResponse> CancelOrder([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] string clientOrderId);

        [Post("/api/1/trading/cancel_orders?nonce={nonce}&apikey={apiKey}")]
        Task<ExecutionResponseMany> CancelOrders([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Get("/api/1/trading/orders/recent?nonce={nonce}&apikey={apiKey}&max_results={limit}")]
        Task<OrderResponse> GetOrders([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] int limit);

        [Post("/api/1/payment/payout")]
        Task<CryptoTransaction> Withdraw([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Get("/api/1/payment/transactions/{id}")]
        Task<TransactionObject> GetWithdrawal([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Path] string id);

        [Post("/api/1/payment/transfer_to_trading")]
        Task<CryptoTransaction> TransferToTrading([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/transfer_to_main")]
        Task<CryptoTransaction> TransferToMain([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

    }
}