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
var order_service_1 = require("../../services/order.service");
var coinmarketcap_service_1 = require("../../services/coinmarketcap.service");
var DetailComponent = /** @class */ (function () {
    function DetailComponent(contextService, route, router, arbitrageService, orderService, coincap, exchangeService) {
        this.contextService = contextService;
        this.route = route;
        this.router = router;
        this.arbitrageService = arbitrageService;
        this.orderService = orderService;
        this.coincap = coincap;
        this.exchangeService = exchangeService;
        this.orders = [];
    }
    DetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        var pairId = Number(this.route.snapshot.paramMap.get('pairId'));
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.refreshPair();
        });
    };
    DetailComponent.prototype.refreshPair = function () {
        var _this = this;
        this.isLoading = true;
        var pairId = Number(this.route.snapshot.paramMap.get('pairId'));
        this.arbitrageService.getArbitragePair(pairId).then(function (pair) {
            _this.pair = pair;
            _this.exchangeService.getBalances().then(function (balances) {
                if (balances && balances[pair.baseExchange] && balances[pair.baseExchange][pair.marketCurrency]) {
                    _this.baseBalance = balances[pair.baseExchange][pair.marketCurrency].available;
                }
                if (balances && balances[pair.counterExchange] && balances[pair.counterExchange][pair.marketCurrency]) {
                    _this.counterBalance = balances[pair.counterExchange][pair.marketCurrency].available;
                }
                _this.isLoading = false;
            });
            _this.arbitrageService.getHeroStats().then(function (heroStats) {
                _this.heroStat = heroStats.getValue(_this.pair.symbol);
            });
        });
    };
    DetailComponent.prototype.save = function () {
        var _this = this;
        this.isLoading = true;
        this.arbitrageService.saveArbitragePair(this.pair).then(function (result) {
            _this.refreshPair();
        });
    };
    DetailComponent = __decorate([
        core_1.Component({
            selector: 'app-detail',
            templateUrl: './detail.component.html',
            styleUrls: ['./detail.component.css']
        }),
        __metadata("design:paramtypes", [context_service_1.ContextService,
            router_1.ActivatedRoute,
            router_1.Router,
            arbitrage_service_1.ArbitrageService,
            order_service_1.OrderService,
            coinmarketcap_service_1.CoinMarketCapService,
            exchange_service_1.ExchangeService])
    ], DetailComponent);
    return DetailComponent;
}());
exports.DetailComponent = DetailComponent;
//# sourceMappingURL=detail.component.js.map