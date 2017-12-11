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
var http_1 = require("@angular/http");
var context_service_1 = require("./context.service");
var coinmarketcap_service_1 = require("./coinmarketcap.service");
var Collections = require("typescript-collections");
var ArbitrageService = /** @class */ (function () {
    function ArbitrageService(http, contextService, coincap) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.coincap = coincap;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
        });
    }
    ArbitrageService.prototype.getArbitragePairs = function () {
        var _this = this;
        return this.http.get("/api/arbitrage/get").toPromise().then(function (response) {
            var result = response.json();
            var dic = new Collections.Dictionary();
            result.forEach(function (pair) {
                dic.setValue(pair.id, pair);
            });
            _this.arbitragePairs = dic;
            return result;
        }).catch(this.handleError);
    };
    ArbitrageService.prototype.getArbitragePair = function (pairId) {
        var _this = this;
        return this.http.get("/api/arbitrage/getpair?id=" + pairId).toPromise().then(function (response) {
            var result = response.json();
            _this.arbitragePairs.setValue(result.id, result);
            return result;
        }).catch(this.handleError);
    };
    ArbitrageService.prototype.saveArbitragePair = function (pair) {
        return this.http.post("/api/arbitrage/save", pair).toPromise().then(function (result) { return true; });
    };
    ArbitrageService.prototype.getStats = function () {
        return this.http.get("/api/arbitrage/getherostats?interval=" + this.context.interval).toPromise().then(function (response) {
            var result = response.json();
            var dic = new Collections.Dictionary();
            result.forEach(function (stat) {
                dic.setValue(stat.symbol, stat);
            });
            return dic;
        }).catch(this.handleError);
    };
    ArbitrageService.prototype.getHeroStats = function () {
        var _this = this;
        var baseTickers = new Collections.Dictionary();
        return this.coincap.getBaseTickers().then(function (tickers) {
            tickers.forEach(function (t) {
                if (t.symbol == 'BTC' || t.symbol == 'ETH')
                    baseTickers.setValue(t.symbol, t);
            });
            var total = 0;
            var trades = 0;
            return _this.getStats().then(function (stats) {
                stats.forEach(function (s) {
                    var stat = stats.getValue(s);
                    if (stat.symbol.endsWith('BTC'))
                        stat.commission = stat.commission * baseTickers.getValue('BTC').price_usd;
                    else if (stat.symbol.endsWith('ETH'))
                        stat.commission = stat.commission * baseTickers.getValue('ETH').price_usd;
                    total += stat.commission;
                    trades += stat.tradeCount;
                });
                var totalStat = { symbol: 'TOTAL', commission: total, tradeCount: trades };
                stats.setValue(totalStat.symbol, totalStat);
                return stats;
            });
        });
    };
    ArbitrageService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    ArbitrageService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http, context_service_1.ContextService, coinmarketcap_service_1.CoinMarketCapService])
    ], ArbitrageService);
    return ArbitrageService;
}());
exports.ArbitrageService = ArbitrageService;
//# sourceMappingURL=arbitrage.service.js.map