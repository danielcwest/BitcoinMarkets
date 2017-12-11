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
var asset_1 = require("../models/asset");
var CoinMarketCapService = /** @class */ (function () {
    function CoinMarketCapService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    }
    CoinMarketCapService.prototype.getAssets = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/ticker/?limit=250").toPromise().then(function (response) {
            var result = response.json();
            var assets = result.map(function (i) { return new asset_1.Asset(i); });
            return assets;
        }).catch(this.handleError);
    };
    CoinMarketCapService.prototype.getGlobal = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/global/").toPromise().then(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    CoinMarketCapService.prototype.getBaseTickers = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/ticker/?limit=5").toPromise().then(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    CoinMarketCapService.prototype.getBitcoinTicker = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/ticker/bitcoin/").toPromise().then(function (response) {
            var result = response.json();
            return result[0];
        }).catch(this.handleError);
    };
    CoinMarketCapService.prototype.getEthereumTicker = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/ticker/ethereum/").toPromise().then(function (response) {
            var result = response.json();
            return result[0];
        }).catch(this.handleError);
    };
    CoinMarketCapService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    CoinMarketCapService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http, context_service_1.ContextService])
    ], CoinMarketCapService);
    return CoinMarketCapService;
}());
exports.CoinMarketCapService = CoinMarketCapService;
//# sourceMappingURL=coinmarketcap.service.js.map