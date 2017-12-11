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
var context_service_1 = require("../services/context.service");
var exchange_service_1 = require("../services/exchange.service");
var arbitrage_service_1 = require("../services/arbitrage.service");
var coinmarketcap_service_1 = require("../services/coinmarketcap.service");
var ArbitrageComponent = /** @class */ (function () {
    function ArbitrageComponent(arbitrageService, contextService, coincap, exchangeService) {
        this.arbitrageService = arbitrageService;
        this.contextService = contextService;
        this.coincap = coincap;
        this.exchangeService = exchangeService;
        this.sortProperty = 'symbol';
        this.sortAscending = true;
        this.isLoading = false;
        this.arbitragePairs = [];
        this.commission = 0;
    }
    ArbitrageComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.refresh();
        });
    };
    ArbitrageComponent.prototype.refresh = function () {
        var _this = this;
        this.isLoading = true;
        this.arbitrageService.getArbitragePairs().then(function (pairs) {
            _this.arbitragePairs = pairs;
            _this.isLoading = false;
        });
        this.arbitrageService.getHeroStats().then(function (heroStats) {
            _this.heroStats = heroStats;
            if (heroStats.containsKey('TOTAL'))
                _this.commission = heroStats.getValue('TOTAL').commission;
        });
        this.exchangeService.getBalances().then(function (balances) {
            _this.exchangeBalances = balances;
        });
    };
    ArbitrageComponent.prototype.processMarkets = function () {
    };
    ArbitrageComponent.prototype.getTradeCount = function (symbol) {
        if (!this.heroStats || !this.heroStats.size)
            return '';
        if (this.heroStats.containsKey(symbol)) {
            return this.heroStats.getValue(symbol).tradeCount;
        }
        else {
            return '';
        }
    };
    ArbitrageComponent.prototype.getCommission = function (symbol) {
        if (!this.heroStats || !this.heroStats.size)
            return 0;
        if (this.heroStats.containsKey(symbol)) {
            return this.heroStats.getValue(symbol).commission;
        }
        else {
            return 0;
        }
    };
    ArbitrageComponent.prototype.getBaseBalance = function (pair) {
        if (!this.exchangeBalances || !this.exchangeBalances[pair.baseExchange] || !this.exchangeBalances[pair.baseExchange][pair.marketCurrency])
            return 0;
        var balance = this.exchangeBalances[pair.baseExchange][pair.marketCurrency].available;
        return balance;
    };
    ArbitrageComponent.prototype.getCounterBalance = function (pair) {
        if (!this.exchangeBalances || !this.exchangeBalances[pair.counterExchange] || !this.exchangeBalances[pair.counterExchange][pair.marketCurrency])
            return 0;
        var balance = this.exchangeBalances[pair.counterExchange][pair.marketCurrency].available;
        return balance;
    };
    ArbitrageComponent.prototype.setInterval = function (interval) {
        this.contextService.setInterval(interval);
    };
    ArbitrageComponent.prototype.changeSort = function (sortProp) {
        if (sortProp && this.sortProperty == sortProp) {
            this.sortAscending = !this.sortAscending;
        }
        else if (sortProp) {
            this.sortAscending = true;
            this.sortProperty = sortProp;
        }
    };
    ArbitrageComponent = __decorate([
        core_1.Component({
            selector: 'app-arbitrage',
            templateUrl: './arbitrage.component.html',
            styleUrls: ['./arbitrage.component.css']
        }),
        __metadata("design:paramtypes", [arbitrage_service_1.ArbitrageService, context_service_1.ContextService, coinmarketcap_service_1.CoinMarketCapService, exchange_service_1.ExchangeService])
    ], ArbitrageComponent);
    return ArbitrageComponent;
}());
exports.ArbitrageComponent = ArbitrageComponent;
//# sourceMappingURL=arbitrage.component.js.map