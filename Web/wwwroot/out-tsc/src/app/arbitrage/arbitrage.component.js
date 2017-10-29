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
var context_service_1 = require("../services/context.service");
var exchange_service_1 = require("../services/exchange.service");
var arbitrage_service_1 = require("../services/arbitrage.service");
var ArbitrageComponent = (function () {
    function ArbitrageComponent(arbitrageService, exchangeService, contextService) {
        this.arbitrageService = arbitrageService;
        this.exchangeService = exchangeService;
        this.contextService = contextService;
        this.exchanges = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];
        this.arbitrageMarkets = [];
        this.sortProperty = 'symbol';
        this.sortAscending = true;
        this.isLoading = false;
    }
    ArbitrageComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.refreshMarkets();
        });
    };
    ArbitrageComponent.prototype.refreshMarkets = function () {
        var _this = this;
        this.isLoading = true;
        this.arbitrageService.refreshMarkets().then(function (response) {
            _this.arbitrageMarkets = response;
            _this.isLoading = false;
        });
    };
    ArbitrageComponent.prototype.processMarkets = function () {
        var _this = this;
        var arbMkts = [];
        this.baseExchangeMarkets.forEach(function (market) {
            if (_this.arbExchangeMarkets.containsKey(market) && _this.arbExchangeMarkets.getValue(market).volume > 10) {
                arbMkts.push(new arbitrage_market_1.ArbitrageMarket(_this.baseExchangeMarkets.getValue(market), _this.arbExchangeMarkets.getValue(market)));
            }
        });
        this.arbitrageMarkets = arbMkts;
    };
    ArbitrageComponent.prototype.changeSort = function (sortProp) {
        if (this.sortProperty == sortProp) {
            this.sortAscending = !this.sortAscending;
        }
        else {
            this.sortAscending = true;
            this.sortProperty = sortProp;
        }
    };
    ArbitrageComponent.prototype.onArbExchangeChange = function (exchange) {
        if (exchange != this.context.selectedBaseExchange) {
            this.contextService.setArbExchange(exchange);
        }
        else {
            console.log('ERROR');
        }
    };
    ArbitrageComponent.prototype.onBaseExchangeChange = function (exchange) {
        if (exchange != this.context.selectedArbExchange) {
            this.contextService.setBaseExchange(exchange);
        }
        else {
            console.log('ERROR');
        }
    };
    return ArbitrageComponent;
}());
ArbitrageComponent = __decorate([
    core_1.Component({
        selector: 'app-arbitrage',
        templateUrl: './arbitrage.component.html',
        styleUrls: ['./arbitrage.component.css']
    }),
    __metadata("design:paramtypes", [arbitrage_service_1.ArbitrageService, exchange_service_1.ExchangeService, context_service_1.ContextService])
], ArbitrageComponent);
exports.ArbitrageComponent = ArbitrageComponent;
//# sourceMappingURL=arbitrage.component.js.map