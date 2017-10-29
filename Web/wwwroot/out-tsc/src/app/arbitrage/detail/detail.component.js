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
var router_1 = require("@angular/router");
var exchange_service_1 = require("../../services/exchange.service");
var context_service_1 = require("../../services/context.service");
var arbitrage_service_1 = require("../../services/arbitrage.service");
var DetailComponent = (function () {
    function DetailComponent(contextService, route, router, exchangeService, arbitrageService) {
        this.contextService = contextService;
        this.route = route;
        this.router = router;
        this.exchangeService = exchangeService;
        this.arbitrageService = arbitrageService;
        this.exchanges = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];
        // Amount in base currency to measure accurate buy/sell price
        this.threshold = .25;
    }
    DetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.symbol = this.route.snapshot.paramMap.get('ticker');
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.baseExchangeFee = _this.arbitrageService.exchangeFees[_this.context.selectedBaseExchange];
            _this.arbExchangeFee = _this.arbitrageService.exchangeFees[_this.context.selectedArbExchange];
            _this.refreshMarkets();
        });
    };
    DetailComponent.prototype.refreshMarkets = function () {
        var _this = this;
        this.isLoading = true;
        return new Promise(function (resolve, reject) {
            var promises = [];
            promises.push(_this.exchangeService.getMarketSummary(_this.context.selectedBaseExchange, _this.symbol));
            promises.push(_this.exchangeService.getMarketSummary(_this.context.selectedArbExchange, _this.symbol));
            promises.push(_this.exchangeService.getOrderBook(_this.context.selectedBaseExchange, _this.symbol));
            promises.push(_this.exchangeService.getOrderBook(_this.context.selectedArbExchange, _this.symbol));
            Promise.all(promises).then(function (response) {
                _this.baseExchangeMarket = response[0];
                _this.arbExchangeMarket = response[1];
                _this.baseOrderBook = response[2];
                _this.arbOrderBook = response[3];
                _this.processMarkets();
                _this.isLoading = false;
                resolve(true);
            });
        });
    };
    DetailComponent.prototype.processMarkets = function () {
        // console.log(this.baseExchangeMarket);
        // console.log(this.arbExchangeMarket);
        // console.log(this.baseOrderBook);
        // console.log(this.arbOrderBook);
        this.spreadLast = Math.abs((this.baseExchangeMarket.last - this.arbExchangeMarket.last) / this.baseExchangeMarket.last);
        this.baseCurrency = this.baseExchangeMarket.baseCurrency;
        this.quoteCurrency = this.baseExchangeMarket.quoteCurrency;
        this.setThresholdPrices();
    };
    DetailComponent.prototype.setThresholdPrices = function () {
        var _this = this;
        // TODO: Optimize below
        // Base Exchange
        this.baseOrderBook.asks.some(function (e) {
            if (e.sumBase >= _this.threshold) {
                // console.log(e);
                _this.baseExactBuy = e.price;
                return true;
            }
        });
        this.baseOrderBook.bids.some(function (e) {
            if (e.sumBase >= _this.threshold) {
                // console.log(e);
                _this.baseExactSell = e.price;
                return true;
            }
        });
        // Arb Exchange
        this.arbOrderBook.asks.some(function (e) {
            if (e.sumBase >= _this.threshold) {
                // console.log(e);
                _this.arbExactBuy = e.price;
                return true;
            }
        });
        this.arbOrderBook.bids.some(function (e) {
            if (e.sumBase >= _this.threshold) {
                // console.log(e);
                _this.arbExactSell = e.price;
                return true;
            }
        });
        this.baseBuySpread = Math.abs((this.baseExactBuy - this.arbExactSell) / this.baseExactBuy);
        this.baseSellSpread = Math.abs((this.baseExactSell - this.arbExactBuy) / this.baseExactSell);
    };
    DetailComponent.prototype.onArbExchangeChange = function (exchange) {
        if (exchange !== this.context.selectedBaseExchange) {
            this.contextService.setArbExchange(exchange);
        }
        else {
            console.log('ERROR');
        }
    };
    DetailComponent.prototype.onBaseExchangeChange = function (exchange) {
        if (exchange !== this.context.selectedArbExchange) {
            this.contextService.setBaseExchange(exchange);
        }
        else {
            console.log('ERROR');
        }
    };
    DetailComponent.prototype.BuyBase = function () {
    };
    DetailComponent.prototype.SellBase = function () {
    };
    DetailComponent.prototype.BuyArb = function () {
    };
    DetailComponent.prototype.SellArb = function () {
    };
    return DetailComponent;
}());
DetailComponent = __decorate([
    core_1.Component({
        selector: 'app-detail',
        templateUrl: './detail.component.html',
        styleUrls: ['./detail.component.css']
    }),
    __metadata("design:paramtypes", [context_service_1.ContextService,
        router_1.ActivatedRoute,
        router_1.Router,
        exchange_service_1.ExchangeService,
        arbitrage_service_1.ArbitrageService])
], DetailComponent);
exports.DetailComponent = DetailComponent;
//# sourceMappingURL=detail.component.js.map