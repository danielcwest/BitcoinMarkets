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
        [Get("/api/1/public/symbols")]
        Task<Symbols> GetSymbols();

        [Get("/api/1/public/ticker")]
        Task<Dictionary<string, Ticker>> GetTickers();

        [Get("/api/1/public/{symbol}/ticker")]
        Task<Ticker> GetTicker([Path] string symbol);

        [Get("/api/1/public/{symbol}/orderbook?format_price=number&format_amount=number")]
        Task<OrderbookResponse> GetOrderBook([Path] string symbol);


        [Get("/api/1/payment/address/{currency}?nonce={nonce}&apikey={apiKey}")]
        Task<Address> GetAddress([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] string currency);

        [Get("/api/1/payment/balance?nonce={nonce}&apikey={apiKey}")]
        Task<MultiCurrencyBalance> GetMainBalances([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Get("/api/1/trading/balance?nonce={nonce}&apikey={apiKey}")]
        Task<MultiCurrencyBalance> GetTradingBalances([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Post("/api/1/trading/new_order")]
        Task<ExecutionResponse> PlaceOrder([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/trading/cancel_order?nonce={nonce}&apikey={apiKey}&clientOrderId={clientOrderId}")]
        Task<ExecutionResponse> CancelOrder([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] string clientOrderId);

        [Post("/api/1/trading/cancel_orders?nonce={nonce}&apikey={apiKey}")]
        Task<ExecutionResponseMany> CancelOrders([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey);

        [Get("/api/1/trading/orders/recent?nonce={nonce}&apikey={apiKey}&max_results={limit}")]
        Task<OrderResponse> GetOrders([Header("X-Signature")] string sig, [Path] long nonce, [Path] string apiKey, [Path] int limit);

        [Post("/api/1/payment/payout")]
        Task<PayoutTransaction> Withdraw([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/transfer_to_trading")]
        Task<PayoutTransaction> TransferToTrading([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

        [Post("/api/1/payment/transfer_to_main")]
        Task<PayoutTransaction> TransferToMain([Header("X-Signature")] string sig, [Query("nonce")] long nonce, [Query("apikey")] string apikey, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);

    }
}