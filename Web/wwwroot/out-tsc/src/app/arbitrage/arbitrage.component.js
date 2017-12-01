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
var arbitrage_service_1 = require("../services/arbitrage.service");
var coinmarketcap_service_1 = require("../services/coinmarketcap.service");
var ArbitrageComponent = (function () {
    function ArbitrageComponent(arbitrageService, contextService, coincap) {
        this.arbitrageService = arbitrageService;
        this.contextService = contextService;
        this.coincap = coincap;
        this.sortProperty = 'symbol';
        this.sortAscending = true;
        this.isLoading = false;
        this.arbitragePairs = [];
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
        this.arbitrageService.getHeroStats().then(function (heroStats) { return _this.heroStats = heroStats; });
    };
    ArbitrageComponent.prototype.processMarkets = function () {
    };
    ArbitrageComponent.prototype.getTradeCount = function (pair) {
        if (!this.heroStats || !this.heroStats.size)
            return '';
        if (this.heroStats.containsKey(pair.symbol)) {
            return this.heroStats.getValue(pair.symbol).tradeCount;
        }
        else {
            return '';
        }
    };
    ArbitrageComponent.prototype.getCommission = function (pair) {
        if (!this.heroStats || !this.heroStats.size)
            return 0;
        if (this.heroStats.containsKey(pair.symbol)) {
            return this.heroStats.getValue(pair.symbol).commission;
        }
        else {
            return 0;
        }
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
    return ArbitrageComponent;
}());
ArbitrageComponent = __decorate([
    core_1.Component({
        selector: 'app-arbitrage',
        templateUrl: './arbitrage.component.html',
        styleUrls: ['./arbitrage.component.css']
    }),
    __metadata("design:paramtypes", [arbitrage_service_1.ArbitrageService, context_service_1.ContextService, coinmarketcap_service_1.CoinMarketCapService])
], ArbitrageComponent);
exports.ArbitrageComponent = ArbitrageComponent;
//# sourceMappingURL=arbitrage.component.js.map