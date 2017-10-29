"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var arbitrage_market_1 = require("../models/arbitrage-market");
var context_service_1 = require("./context.service");
var exchange_service_1 = require("./exchange.service");
var Collections = require("typescript-collections");
var ArbitrageService = (function () {
    function ArbitrageService(contextService, exchangeService) {
        var _this = this;
        this.contextService = contextService;
        this.exchangeService = exchangeService;
        this.exchanges = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];
        // TODO: make dynamic
        this.exchangeFees = { 'Bittrex': .0025, 'Hitbtc': .0010, 'Poloniex': .0025, 'Binance': .0010 };
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.refreshMarkets();
        });
    }
    ArbitrageService.prototype.refreshMarkets = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var promises = [];
            promises.push(_this.exchangeService.getMarketSummaries(_this.context.selectedBaseExchange));
            promises.push(_this.exchangeService.getMarketSummaries(_this.context.selectedArbExchange));
            Promise.all(promises).then(function (response) {
                var baseExchangeMarkets = response[0];
                var arbExchangeMarkets = response[1];
                var dic = new Collections.Dictionary();
                baseExchangeMarkets.forEach(function (market) {
                    if (arbExchangeMarkets.containsKey(market)) {
                        dic.setValue(market, new arbitrage_market_1.ArbitrageMarket(baseExchangeMarkets.getValue(market), arbExchangeMarkets.getValue(market)));
                    }
                });
                _this.arbitrageMarkets = dic;
                resolve(_this.arbitrageMarkets.values());
            });
        });
    };
    ArbitrageService.prototype.refreshSymbols = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var promises = [];
            _this.exchanges.forEach(function (exchange) { return promises.push(_this.exchangeService.getSymbols(exchange)); });
            Promise.all(promises).then(function (response) {
                var i = 0;
                _this.exchanges.forEach(function (exchange) {
                    _this.exchangeMarketSymbols.setValue(exchange, response[i++]);
                });
                resolve(true);
            });
        });
    };
    ArbitrageService.prototype.getArbitrageMarket = function (symbol) {
        return this.arbitrageMarkets.getValue(symbol);
    };
    return ArbitrageService;
}());
ArbitrageService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [context_service_1.ContextService, exchange_service_1.ExchangeService])
], ArbitrageService);
exports.ArbitrageService = ArbitrageService;
//# sourceMappingURL=arbitrage.service.js.map