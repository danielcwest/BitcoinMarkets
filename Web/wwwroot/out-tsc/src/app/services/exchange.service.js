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
var Collections = require("typescript-collections");
var ExchangeService = (function () {
    function ExchangeService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    }
    ExchangeService.prototype.getMarketSummaries = function (exchange) {
        return this.http.get("/api/" + exchange + "/marketsummaries/").toPromise().then(function (response) {
            var result = response.json();
            var dic = new Collections.Dictionary();
            result.forEach(function (market) { return dic.setValue(market.symbol, market); });
            return dic;
        });
    };
    ExchangeService.prototype.getMarketSummary = function (exchange, symbol) {
        return this.http.get("/api/" + exchange + "/marketsummary?symbol=" + symbol).toPromise().then(function (response) {
            var result = response.json();
            return result;
        });
    };
    ExchangeService.prototype.getOrderBook = function (exchange, symbol) {
        return this.http.get("/api/" + exchange + "/orderbook?symbol=" + symbol).toPromise().then(function (response) {
            var result = response.json();
            return result;
        });
    };
    ExchangeService.prototype.getSymbols = function (exchange) {
        return this.http.get("/api/" + exchange + "/symbols").toPromise().then(function (response) {
            var result = response.json();
            return result;
        });
    };
    ExchangeService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    return ExchangeService;
}());
ExchangeService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http, context_service_1.ContextService])
], ExchangeService);
exports.ExchangeService = ExchangeService;
//# sourceMappingURL=exchange.service.js.map