webpackJsonp(["main"],{

/***/ "../../../../../src/$$_gendir lazy recursive":
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncatched exception popping up in devtools
	return Promise.resolve().then(function() {
		throw new Error("Cannot find module '" + req + "'.");
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "../../../../../src/$$_gendir lazy recursive";

/***/ }),

/***/ "../../../../../src/app/app-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__arbitrage_arbitrage_component__ = __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__arbitrage_detail_detail_component__ = __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var routes = [
    {
        path: '',
        redirectTo: '/arbitrage',
        pathMatch: 'full'
    },
    {
        path: 'arbitrage',
        component: __WEBPACK_IMPORTED_MODULE_2__arbitrage_arbitrage_component__["a" /* ArbitrageComponent */]
    },
    {
        path: 'arbitrage/:pairId',
        component: __WEBPACK_IMPORTED_MODULE_3__arbitrage_detail_detail_component__["a" /* DetailComponent */]
    }
];
var AppRoutingModule = (function () {
    function AppRoutingModule() {
    }
    return AppRoutingModule;
}());
AppRoutingModule = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["M" /* NgModule */])({
        imports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["c" /* RouterModule */].forRoot(routes)],
        exports: [__WEBPACK_IMPORTED_MODULE_1__angular_router__["c" /* RouterModule */]]
    })
], AppRoutingModule);

//# sourceMappingURL=app-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/app.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, ".interval-buttons {\r\n\theight: 100%;\r\n}\r\n\r\n.interval.active {\r\n\tfont-weight: bold;\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/app.component.html":
/***/ (function(module, exports) {

module.exports = "<nav class=\"navbar navbar-expand-lg navbar-light bg-light\">\r\n\t<a class=\"navbar-brand\" href=\"/\">Bitcoin Markets</a>\r\n\t<button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\"#navbarNavAltMarkup\" aria-controls=\"navbarNavAltMarkup\"\r\n\t    aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n      <span class=\"navbar-toggler-icon\"></span>\r\n    </button>\r\n\t<div class=\"collapse navbar-collapse\" id=\"navbarNavAltMarkup\">\r\n\t\t<div class=\"navbar-nav\">\r\n\t\t\t<a routerLink=\"/dashboard\" routerLinkActive=\"active\" class=\"nav-item nav-link\">Dashboard</a>\r\n\t\t\t<a routerLink=\"/arbitrage\" routerLinkActive=\"active\" class=\"nav-item nav-link\">Arbitrage</a>\r\n\t\t</div>\r\n\t</div>\r\n\t<div class=\"text-right\">\r\n\t\t<div class=\"btn-group interval-buttons\" role=\"group\" aria-label=\"Basic example\">\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('1h')\" [class.active]=\"context.interval == '1h'\">1h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('4h')\" [class.active]=\"context.interval == '4h'\">4h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('12h')\" [class.active]=\"context.interval == '12h'\">12h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('24h')\" [class.active]=\"context.interval == '24h'\">24h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('7d')\" [class.active]=\"context.interval == '7d'\">7d</button>\r\n\t\t</div>\r\n\t\t<button class=\"btn refresh right\" (click)=\"refreshPairs()\">Refresh</button>\r\n\t</div>\r\n</nav>\r\n<div class=\"container\">\r\n\t<router-outlet>\r\n\t</router-outlet>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/app.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_config_service__ = __webpack_require__("../../../../../src/app/services/config.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var AppComponent = (function () {
    function AppComponent(configService, contextService) {
        this.configService = configService;
        this.contextService = contextService;
    }
    AppComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    };
    AppComponent.prototype.refreshPairs = function () {
        this.contextService.notify();
    };
    AppComponent.prototype.setInterval = function (interval) {
        this.contextService.setInterval(interval);
    };
    return AppComponent;
}());
AppComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-root',
        template: __webpack_require__("../../../../../src/app/app.component.html"),
        styles: [__webpack_require__("../../../../../src/app/app.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_config_service__["a" /* ConfigService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_config_service__["a" /* ConfigService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_context_service__["a" /* ContextService */]) === "function" && _b || Object])
], AppComponent);

var _a, _b;
//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ "../../../../../src/app/app.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__("../../../platform-browser/@angular/platform-browser.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__services_services_module__ = __webpack_require__("../../../../../src/app/services/services.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__app_routing_module__ = __webpack_require__("../../../../../src/app/app-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared_shared_module__ = __webpack_require__("../../../../../src/app/shared/shared.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__arbitrage_arbitrage_component__ = __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__arbitrage_detail_detail_component__ = __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};










var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["M" /* NgModule */])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */],
            __WEBPACK_IMPORTED_MODULE_8__arbitrage_arbitrage_component__["a" /* ArbitrageComponent */],
            __WEBPACK_IMPORTED_MODULE_9__arbitrage_detail_detail_component__["a" /* DetailComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* BrowserModule */],
            __WEBPACK_IMPORTED_MODULE_3__angular_http__["b" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_5__services_services_module__["a" /* ServicesModule */],
            __WEBPACK_IMPORTED_MODULE_6__app_routing_module__["a" /* AppRoutingModule */],
            __WEBPACK_IMPORTED_MODULE_7__shared_shared_module__["a" /* SharedModule */],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["a" /* FormsModule */]
        ],
        providers: [],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */]]
    })
], AppModule);

//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ "../../../../../src/app/arbitrage/arbitrage.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/arbitrage/arbitrage.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"row mt-4\">\r\n\t<div class=\"col\">\r\n\t\t<h1>Arbitrage King <span class=\"float-right\">{{getCommission('TOTAL')  | numeral:'$0,0.00'}}</span></h1>\r\n\t</div>\r\n</div>\r\n<hr />\r\n<div *ngIf=\"isLoading\" class=\"load\">\r\n\t<div class=\"loader mx-auto\"></div>\r\n</div>\r\n<table class=\"table\" *ngIf=\"arbitragePairs.length > 0\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th>Symbol</th>\r\n\t\t\t<th>Status</th>\r\n\t\t\t<th>Type</th>\r\n\t\t\t<th>Trade Threshold</th>\r\n\t\t\t<th>Base Balance</th>\r\n\t\t\t<th>Counter Balance</th>\r\n\t\t\t<th>Bid Spread</th>\r\n\t\t\t<th>Ask Spread</th>\r\n\t\t\t<th>Commission</th>\r\n\t\t\t<th>Trade Count</th>\r\n\t\t</tr>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let pair of arbitragePairs\">\r\n\t\t\t<th scope=\"row\"><a [routerLink]=\"[ './', pair.id ]\">{{pair.symbol}}</a></th>\r\n\t\t\t<td><span class=\"badge\"\r\n\t\t  [class.badge-danger]=\"pair?.status != 'active'\"\r\n\t\t  [class.badge-success]=\"pair?.status == 'active' && pair?.type == 'market'\"\r\n\t\t  [class.badge-warning]=\"pair?.status == 'active' && pair?.type != 'market'\">\r\n\t\t{{pair?.status}}\r\n\t</span></td>\r\n\t\t\t<td>{{pair.type}}</td>\r\n\t\t\t<td>{{pair.tradeThreshold | numeral:'0.00%'}}</td>\r\n\t\t\t<td>{{getBaseBalance(pair) | numeral:'0.00'}}</td>\r\n\t\t\t<td>{{getCounterBalance(pair) | numeral:'0.00'}}</td>\r\n\t\t\t<td>{{pair.bidSpread | numeral:'0.00%'}}</td>\r\n\t\t\t<td>{{pair.askSpread | numeral:'0.00%'}}</td>\r\n\t\t\t<td>{{getCommission(pair.symbol)  | numeral:'$0,0.00'}}</td>\r\n\t\t\t<td>{{getTradeCount(pair.symbol)}}</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n</table>\r\n"

/***/ }),

/***/ "../../../../../src/app/arbitrage/arbitrage.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ArbitrageComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_arbitrage_service__ = __webpack_require__("../../../../../src/app/services/arbitrage.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_coinmarketcap_service__ = __webpack_require__("../../../../../src/app/services/coinmarketcap.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ArbitrageComponent = (function () {
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
    return ArbitrageComponent;
}());
ArbitrageComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-arbitrage',
        template: __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.html"),
        styles: [__webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_arbitrage_service__["a" /* ArbitrageService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_arbitrage_service__["a" /* ArbitrageService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_context_service__["a" /* ContextService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__services_coinmarketcap_service__["a" /* CoinMarketCapService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_coinmarketcap_service__["a" /* CoinMarketCapService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */]) === "function" && _d || Object])
], ArbitrageComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=arbitrage.component.js.map

/***/ }),

/***/ "../../../../../src/app/arbitrage/detail/detail.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n.success h1 {\r\n\tfont-size: 4.0rem;\r\n}\r\n\r\n.danger h1 {\r\n\tfont-size: 2.0rem;\r\n}\r\n\r\n.bordered-box {\r\n\tposition: relative;\r\n\tpadding: 1rem;\r\n\tmargin: 1rem -1rem;\r\n\tborder: solid #f7f7f9;\r\n\tborder-width: .2rem 0 0;\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/arbitrage/detail/detail.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"row mt-4\">\r\n\t<div class=\"col\">\r\n\t\t<h2 class=\"text-center pt-2\">{{pair?.baseExchange}}</h2>\r\n\t\t<h2 class=\"text-center pt-2\">{{baseBalance}}</h2>\r\n\t</div>\r\n\t\t<div class=\"col\">\r\n\t\t\t<h1 class=\"display-2 text-center\">{{pair?.symbol}}</h1>\r\n\t\t\t<h4 class=\"text-center\">\r\n\t\t\t\t<span class=\"badge\"\r\n\t\t\t\t\t  [class.badge-danger]=\"pair?.status != 'active'\"\r\n\t\t\t\t\t  [class.badge-success]=\"pair?.status == 'active' && pair?.type == 'market'\"\r\n\t\t\t\t\t  [class.badge-warning]=\"pair?.status == 'active' && pair?.type != 'market'\">\r\n\t\t\t\t\t{{pair?.status}}\r\n\t\t\t\t</span>\r\n\t\t\t</h4>\r\n\t\t\t<h1 class=\"display-4 text-center\">{{heroStat?.commission | numeral:'$0,0.00'}} / {{heroStat?.tradeCount}}</h1>\r\n\t\t</div>\r\n\t<div class=\"col\">\r\n\t\t<h2 class=\"text-center pt-2\">{{pair?.counterExchange}}</h2>\r\n\t\t<h2 class=\"text-center pt-2\">{{counterBalance}}</h2>\r\n\t</div>\r\n\t</div>\r\n<hr />\r\n\r\n<div *ngIf=\"isLoading\" class=\"load\">\r\n\t<div class=\"loader mx-auto\"></div>\r\n</div>\r\n\r\n<div class=\"row\" *ngIf=\"pair\">\r\n\t<div class=\"col\"></div>\r\n\t<div class=\"col\">\r\n\t\t<form #pairForm=\"ngForm\">\r\n\t\t\t<div class=\"form-group row\">\r\n\t\t\t\t<div class=\"col-6\">\r\n\t\t\t\t\t<label for=\"bidspread\">Bid Spread</label>\r\n\t\t\t\t\t<div class=\"input-group\">\r\n\t\t\t\t\t\t<span class=\"input-group-addon\">{{pair.bidSpread | numeral:'0.00%'}}</span>\r\n\t\t\t\t\t\t<input class=\"form-control\" type=\"number\" [(ngModel)]=\"pair.bidSpread\" id=\"bidspread\" name=\"bidSpread\" step=\"0.001\">\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n\t\t\t\t<div class=\"col-6\">\r\n\t\t\t\t\t<label for=\"askspread\">Ask Spread </label>\r\n\t\t\t\t\t<div class=\"input-group\">\r\n\t\t\t\t\t\t<input class=\"form-control\" type=\"number\" [(ngModel)]=\"pair.askSpread\" id=\"askspread\" name=\"askSpread\" step=\"0.001\">\r\n\t\t\t\t\t\t<span class=\"input-group-addon\">{{pair.askSpread | numeral:'0.00%'}}</span>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n\t\t\t</div>\r\n\t\t\t<div class=\"form-group row\">\r\n\t\t\t\t<label for=\"tradeThreshold\">Trade Threshold</label>\r\n\t\t\t\t<div class=\"input-group\">\r\n\t\t\t\t\t<span class=\"input-group-addon\">{{pair.tradeThreshold | numeral:'0.00%'}}</span>\r\n\t\t\t\t\t<input class=\"form-control\" type=\"number\" [(ngModel)]=\"pair.tradeThreshold\" id=\"tradeThreshold\" name=\"tradeThreshold\" step=\"0.0001\">\r\n\t\t\t\t</div>\r\n\t\t\t</div>\r\n\t\t\t<div class=\"form-group row\">\r\n\t\t\t\t<label for=\"type\">Pair Type</label>\r\n\t\t\t\t<div class=\"input-group\">\r\n\t\t\t\t\t<input class=\"form-control\" type=\"text\" [(ngModel)]=\"pair.type\" id=\"type\" name=\"type\">\r\n\t\t\t\t</div>\r\n\t\t\t</div>\r\n\t\t\t<div class=\"form-group row\">\r\n\t\t\t\t<label for=\"type\">Pair Status</label>\r\n\t\t\t\t<div class=\"input-group\">\r\n\t\t\t\t\t<input class=\"form-control\" type=\"text\" [(ngModel)]=\"pair.status\" id=\"status\" name=\"status\">\r\n\t\t\t\t</div>\r\n\t\t\t</div>\r\n\t\t\t<div class=\"form-group row\">\r\n\t\t\t\t<button type=\"button\" class=\"btn btn-primary btn-lg btn-block\" (click)=\"save()\">Save</button> \r\n\t\t\t</div>\r\n\t\t</form>\r\n\t</div>\r\n\t<div class=\"col\"></div>\r\n</div>\r\n\r\n<table class=\"table\" *ngIf=\"orders.length > 0\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th>Base Order</th>\r\n\t\t\t<th>Counter Order</th>\r\n\t\t\t<th>Base Quantity</th>\r\n\t\t\t<th>Counter Quantity</th>\r\n\t\t\t<th>Commission</th>\r\n\t\t</tr>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let order of orders\">\r\n\t\t\t<td>{{order.baseOrderUuid}}</td>\r\n\t\t\t<td>{{order.counterOrderUuid}}</td>\r\n\t\t\t<td>{{order.baseQuantityFilled}}</td>\r\n\t\t\t<td>{{order.counterQuantityFilled}}</td>\r\n\t\t\t<td>{{order.commission}}</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n</table>\r\n\r\n"

/***/ }),

/***/ "../../../../../src/app/arbitrage/detail/detail.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DetailComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__ = __webpack_require__("../../../../../src/app/services/arbitrage.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__services_order_service__ = __webpack_require__("../../../../../src/app/services/order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__services_coinmarketcap_service__ = __webpack_require__("../../../../../src/app/services/coinmarketcap.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var DetailComponent = (function () {
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
    return DetailComponent;
}());
DetailComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-detail',
        template: __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.html"),
        styles: [__webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* ActivatedRoute */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* ActivatedRoute */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_5__services_order_service__["a" /* OrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__services_order_service__["a" /* OrderService */]) === "function" && _e || Object, typeof (_f = typeof __WEBPACK_IMPORTED_MODULE_6__services_coinmarketcap_service__["a" /* CoinMarketCapService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__services_coinmarketcap_service__["a" /* CoinMarketCapService */]) === "function" && _f || Object, typeof (_g = typeof __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */]) === "function" && _g || Object])
], DetailComponent);

var _a, _b, _c, _d, _e, _f, _g;
//# sourceMappingURL=detail.component.js.map

/***/ }),

/***/ "../../../../../src/app/models/app-context.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppContext; });
var AppContext = (function () {
    function AppContext() {
        this.interval = '1h';
    }
    return AppContext;
}());

//# sourceMappingURL=app-context.js.map

/***/ }),

/***/ "../../../../../src/app/models/asset.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Asset; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_moment__ = __webpack_require__("../../../../moment/moment.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_moment___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_moment__);

var Asset = (function () {
    function Asset(item) {
        this.id = item.id;
        this.name = item.name;
        this.symbol = item.symbol;
        this.rank = Number(item.rank);
        this.price_usd = Number(item.price_usd);
        this.price_btc = Number(item.price_btc);
        this['24h_volume_usd'] = Number(item['24h_volume_usd']);
        this.market_cap_usd = Number(item.market_cap_usd);
        this.available_supply = Number(item.available_supply);
        this.total_supply = Number(item.total_supply);
        this.percent_change_1h = Number(item.percent_change_1h);
        this.percent_change_24h = Number(item.percent_change_24h);
        this.percent_change_7d = Number(item.percent_change_7d);
        this.last_updated = __WEBPACK_IMPORTED_MODULE_0_moment__["unix"](Number(item.last_updated)).toDate();
        //calculate what the market cap was 1 hour ago, 24 hours ago, 1 week ago
        var netChange1h = this.market_cap_usd * (Math.abs(this.percent_change_1h) / 100);
        this.market_cap_usd_1h = this.percent_change_1h >= 0 ? this.market_cap_usd - netChange1h : this.market_cap_usd + netChange1h;
        var netChange24h = this.market_cap_usd * (Math.abs(this.percent_change_24h) / 100);
        this.market_cap_usd_24h = this.percent_change_24h >= 0 ? this.market_cap_usd - netChange24h : this.market_cap_usd + netChange24h;
        var netChange7d = this.market_cap_usd * (Math.abs(this.percent_change_7d) / 100);
        this.market_cap_usd_7d = this.percent_change_7d >= 0 ? this.market_cap_usd - netChange7d : this.market_cap_usd + netChange7d;
        //console.log(`${this.symbol}: ${this.market_cap_usd.toLocaleString()} | 1hr ${this.percent_change_1h} ${this.market_cap_usd_1h.toLocaleString()} | 24hr ${this.percent_change_24h} ${this.market_cap_usd_24h.toLocaleString()} | 7d ${this.percent_change_7d} ${this.market_cap_usd_7d.toLocaleString()}`);
    }
    return Asset;
}());

//# sourceMappingURL=asset.js.map

/***/ }),

/***/ "../../../../../src/app/services/arbitrage.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ArbitrageService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__coinmarketcap_service__ = __webpack_require__("../../../../../src/app/services/coinmarketcap.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_typescript_collections__ = __webpack_require__("../../../../typescript-collections/dist/lib/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_typescript_collections___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_typescript_collections__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ArbitrageService = (function () {
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
            var dic = new __WEBPACK_IMPORTED_MODULE_4_typescript_collections__["Dictionary"]();
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
            var dic = new __WEBPACK_IMPORTED_MODULE_4_typescript_collections__["Dictionary"]();
            result.forEach(function (stat) {
                dic.setValue(stat.symbol, stat);
            });
            return dic;
        }).catch(this.handleError);
    };
    ArbitrageService.prototype.getHeroStats = function () {
        var _this = this;
        var baseTickers = new __WEBPACK_IMPORTED_MODULE_4_typescript_collections__["Dictionary"]();
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
    return ArbitrageService;
}());
ArbitrageService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__coinmarketcap_service__["a" /* CoinMarketCapService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__coinmarketcap_service__["a" /* CoinMarketCapService */]) === "function" && _c || Object])
], ArbitrageService);

var _a, _b, _c;
//# sourceMappingURL=arbitrage.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/coinmarketcap.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return CoinMarketCapService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_asset__ = __webpack_require__("../../../../../src/app/models/asset.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var CoinMarketCapService = (function () {
    function CoinMarketCapService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    }
    CoinMarketCapService.prototype.getAssets = function () {
        return this.http.get("https://api.coinmarketcap.com/v1/ticker/?limit=250").toPromise().then(function (response) {
            var result = response.json();
            var assets = result.map(function (i) { return new __WEBPACK_IMPORTED_MODULE_3__models_asset__["a" /* Asset */](i); });
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
    return CoinMarketCapService;
}());
CoinMarketCapService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object])
], CoinMarketCapService);

var _a, _b;
//# sourceMappingURL=coinmarketcap.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/config.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfigService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ConfigService = (function () {
    function ConfigService(http) {
        this.http = http;
    }
    ConfigService.prototype.getGroupConfig = function () {
        return this.http.get("/api/config/get").toPromise().then(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    ConfigService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    return ConfigService;
}());
ConfigService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object])
], ConfigService);

var _a;
//# sourceMappingURL=config.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/context.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ContextService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__ = __webpack_require__("../../../../rxjs/_esm5/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_app_context__ = __webpack_require__("../../../../../src/app/models/app-context.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ContextService = (function () {
    function ContextService() {
        //dashboard/market summary
        this.intervals = ['1h', '4h', '12h', '24h', '7d'];
        this.contextSource = new __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__["a" /* BehaviorSubject */](new __WEBPACK_IMPORTED_MODULE_2__models_app_context__["a" /* AppContext */]());
        this.context$ = this.contextSource.asObservable();
        this.context = new __WEBPACK_IMPORTED_MODULE_2__models_app_context__["a" /* AppContext */]();
    }
    ContextService.prototype.setInterval = function (interval) {
        if (this.intervals.indexOf(interval) != -1) {
            this.context.interval = interval;
            this.contextSource.next(this.context);
        }
    };
    ContextService.prototype.setAssetCount = function (count) {
        if (count && count > 0) {
            this.context.assetCount = count;
            this.contextSource.next(this.context);
        }
    };
    ContextService.prototype.notify = function () {
        this.contextSource.next(this.context);
    };
    return ContextService;
}());
ContextService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [])
], ContextService);

//# sourceMappingURL=context.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/exchange.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExchangeService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ExchangeService = (function () {
    function ExchangeService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    }
    ExchangeService.prototype.getBalances = function () {
        return this.http.get("/api/exchange/GetBalances").toPromise().then(function (response) {
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
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object])
], ExchangeService);

var _a, _b;
//# sourceMappingURL=exchange.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/order.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var OrderService = (function () {
    function OrderService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
        });
    }
    OrderService.prototype.getOrdersForPair = function (pairId) {
        return this.http.get("/api/order/get?pairId=" + pairId).toPromise().then(function (response) {
            var result = response.json();
            return result;
        }).catch(this.handleError);
    };
    OrderService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    return OrderService;
}());
OrderService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object])
], OrderService);

var _a, _b;
//# sourceMappingURL=order.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/services.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ServicesModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__coinmarketcap_service__ = __webpack_require__("../../../../../src/app/services/coinmarketcap.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__config_service__ = __webpack_require__("../../../../../src/app/services/config.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__arbitrage_service__ = __webpack_require__("../../../../../src/app/services/arbitrage.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__order_service__ = __webpack_require__("../../../../../src/app/services/order.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};








var ServicesModule = (function () {
    function ServicesModule() {
    }
    return ServicesModule;
}());
ServicesModule = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["M" /* NgModule */])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_common__["b" /* CommonModule */]
        ],
        declarations: [],
        providers: [__WEBPACK_IMPORTED_MODULE_2__coinmarketcap_service__["a" /* CoinMarketCapService */], __WEBPACK_IMPORTED_MODULE_3__config_service__["a" /* ConfigService */], __WEBPACK_IMPORTED_MODULE_4__context_service__["a" /* ContextService */], __WEBPACK_IMPORTED_MODULE_5__exchange_service__["a" /* ExchangeService */], __WEBPACK_IMPORTED_MODULE_6__arbitrage_service__["a" /* ArbitrageService */], __WEBPACK_IMPORTED_MODULE_7__order_service__["a" /* OrderService */]]
    })
], ServicesModule);

//# sourceMappingURL=services.module.js.map

/***/ }),

/***/ "../../../../../src/app/shared/btc-price.pipe.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return BtcPricePipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var BtcPricePipe = (function () {
    function BtcPricePipe() {
    }
    BtcPricePipe.prototype.transform = function (value, args) {
        if (Number.isNaN(value)) {
            return null;
        }
        return Number(value).toFixed(8);
    };
    return BtcPricePipe;
}());
BtcPricePipe = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["X" /* Pipe */])({
        name: 'btcPrice'
    })
], BtcPricePipe);

//# sourceMappingURL=btc-price.pipe.js.map

/***/ }),

/***/ "../../../../../src/app/shared/comma-separated-number.pipe.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return CommaSeparatedNumberPipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var CommaSeparatedNumberPipe = (function () {
    function CommaSeparatedNumberPipe() {
    }
    CommaSeparatedNumberPipe.prototype.transform = function (value, args) {
        return value.toLocaleString();
    };
    return CommaSeparatedNumberPipe;
}());
CommaSeparatedNumberPipe = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["X" /* Pipe */])({ name: 'commaSeparatedNumber' })
], CommaSeparatedNumberPipe);

//# sourceMappingURL=comma-separated-number.pipe.js.map

/***/ }),

/***/ "../../../../../src/app/shared/numeral.pipe.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NumeralPipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_numeral__ = __webpack_require__("../../../../numeral/numeral.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_numeral___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_numeral__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};


var NumeralPipe = (function () {
    function NumeralPipe() {
    }
    NumeralPipe.prototype.transform = function (value, format) {
        return __WEBPACK_IMPORTED_MODULE_1_numeral__(value).format(format);
    };
    return NumeralPipe;
}());
NumeralPipe = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["X" /* Pipe */])({
        name: 'numeral'
    })
], NumeralPipe);

//# sourceMappingURL=numeral.pipe.js.map

/***/ }),

/***/ "../../../../../src/app/shared/order-by.pipe.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderByPipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var OrderByPipe = (function () {
    function OrderByPipe() {
    }
    OrderByPipe.prototype.transform = function (array, orderField, orderAscending, skip) {
        if (skip === void 0) { skip = false; }
        if (!skip) {
            array.sort(function (a, b) {
                var aField = a[orderField];
                var bField = b[orderField];
                var comparison = 0;
                //take no null value larger than null value. 
                //empty string is false, so we do not use the virable direct as the boolean expression
                if (aField == null && bField != null) {
                    comparison = 1;
                }
                else if (aField != null && bField == null) {
                    comparison = -1;
                }
                else {
                    if (typeof aField == "string")
                        comparison = aField.localeCompare(bField);
                    else
                        comparison = aField - bField;
                }
                return orderAscending ? comparison : comparison * -1;
            });
        }
        return array;
    };
    return OrderByPipe;
}());
OrderByPipe = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["X" /* Pipe */])({ name: 'orderBy' })
], OrderByPipe);

//# sourceMappingURL=order-by.pipe.js.map

/***/ }),

/***/ "../../../../../src/app/shared/shared.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SharedModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__ = __webpack_require__("../../../../../src/app/shared/comma-separated-number.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__numeral_pipe__ = __webpack_require__("../../../../../src/app/shared/numeral.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__btc_price_pipe__ = __webpack_require__("../../../../../src/app/shared/btc-price.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__order_by_pipe__ = __webpack_require__("../../../../../src/app/shared/order-by.pipe.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};






var SharedModule = (function () {
    function SharedModule() {
    }
    return SharedModule;
}());
SharedModule = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["M" /* NgModule */])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_common__["b" /* CommonModule */]
        ],
        declarations: [__WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__["a" /* CommaSeparatedNumberPipe */], __WEBPACK_IMPORTED_MODULE_3__numeral_pipe__["a" /* NumeralPipe */], __WEBPACK_IMPORTED_MODULE_4__btc_price_pipe__["a" /* BtcPricePipe */], __WEBPACK_IMPORTED_MODULE_5__order_by_pipe__["a" /* OrderByPipe */]],
        exports: [__WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__["a" /* CommaSeparatedNumberPipe */], __WEBPACK_IMPORTED_MODULE_3__numeral_pipe__["a" /* NumeralPipe */], __WEBPACK_IMPORTED_MODULE_4__btc_price_pipe__["a" /* BtcPricePipe */], __WEBPACK_IMPORTED_MODULE_5__order_by_pipe__["a" /* OrderByPipe */]]
    })
], SharedModule);

//# sourceMappingURL=shared.module.js.map

/***/ }),

/***/ "../../../../../src/environments/environment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.
// The file contents for the current environment will overwrite these during build.
var environment = {
    production: false
};
//# sourceMappingURL=environment.js.map

/***/ }),

/***/ "../../../../../src/main.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__("../../../platform-browser-dynamic/@angular/platform-browser-dynamic.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__("../../../../../src/app/app.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/toPromise.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_toPromise__);





if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_23" /* enableProdMode */])();
}
Object(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */])
    .catch(function (err) { return console.log(err); });
//# sourceMappingURL=main.js.map

/***/ }),

/***/ "../../../../moment/locale recursive ^\\.\\/.*$":
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"./af": "../../../../moment/locale/af.js",
	"./af.js": "../../../../moment/locale/af.js",
	"./ar": "../../../../moment/locale/ar.js",
	"./ar-dz": "../../../../moment/locale/ar-dz.js",
	"./ar-dz.js": "../../../../moment/locale/ar-dz.js",
	"./ar-kw": "../../../../moment/locale/ar-kw.js",
	"./ar-kw.js": "../../../../moment/locale/ar-kw.js",
	"./ar-ly": "../../../../moment/locale/ar-ly.js",
	"./ar-ly.js": "../../../../moment/locale/ar-ly.js",
	"./ar-ma": "../../../../moment/locale/ar-ma.js",
	"./ar-ma.js": "../../../../moment/locale/ar-ma.js",
	"./ar-sa": "../../../../moment/locale/ar-sa.js",
	"./ar-sa.js": "../../../../moment/locale/ar-sa.js",
	"./ar-tn": "../../../../moment/locale/ar-tn.js",
	"./ar-tn.js": "../../../../moment/locale/ar-tn.js",
	"./ar.js": "../../../../moment/locale/ar.js",
	"./az": "../../../../moment/locale/az.js",
	"./az.js": "../../../../moment/locale/az.js",
	"./be": "../../../../moment/locale/be.js",
	"./be.js": "../../../../moment/locale/be.js",
	"./bg": "../../../../moment/locale/bg.js",
	"./bg.js": "../../../../moment/locale/bg.js",
	"./bm": "../../../../moment/locale/bm.js",
	"./bm.js": "../../../../moment/locale/bm.js",
	"./bn": "../../../../moment/locale/bn.js",
	"./bn.js": "../../../../moment/locale/bn.js",
	"./bo": "../../../../moment/locale/bo.js",
	"./bo.js": "../../../../moment/locale/bo.js",
	"./br": "../../../../moment/locale/br.js",
	"./br.js": "../../../../moment/locale/br.js",
	"./bs": "../../../../moment/locale/bs.js",
	"./bs.js": "../../../../moment/locale/bs.js",
	"./ca": "../../../../moment/locale/ca.js",
	"./ca.js": "../../../../moment/locale/ca.js",
	"./cs": "../../../../moment/locale/cs.js",
	"./cs.js": "../../../../moment/locale/cs.js",
	"./cv": "../../../../moment/locale/cv.js",
	"./cv.js": "../../../../moment/locale/cv.js",
	"./cy": "../../../../moment/locale/cy.js",
	"./cy.js": "../../../../moment/locale/cy.js",
	"./da": "../../../../moment/locale/da.js",
	"./da.js": "../../../../moment/locale/da.js",
	"./de": "../../../../moment/locale/de.js",
	"./de-at": "../../../../moment/locale/de-at.js",
	"./de-at.js": "../../../../moment/locale/de-at.js",
	"./de-ch": "../../../../moment/locale/de-ch.js",
	"./de-ch.js": "../../../../moment/locale/de-ch.js",
	"./de.js": "../../../../moment/locale/de.js",
	"./dv": "../../../../moment/locale/dv.js",
	"./dv.js": "../../../../moment/locale/dv.js",
	"./el": "../../../../moment/locale/el.js",
	"./el.js": "../../../../moment/locale/el.js",
	"./en-au": "../../../../moment/locale/en-au.js",
	"./en-au.js": "../../../../moment/locale/en-au.js",
	"./en-ca": "../../../../moment/locale/en-ca.js",
	"./en-ca.js": "../../../../moment/locale/en-ca.js",
	"./en-gb": "../../../../moment/locale/en-gb.js",
	"./en-gb.js": "../../../../moment/locale/en-gb.js",
	"./en-ie": "../../../../moment/locale/en-ie.js",
	"./en-ie.js": "../../../../moment/locale/en-ie.js",
	"./en-nz": "../../../../moment/locale/en-nz.js",
	"./en-nz.js": "../../../../moment/locale/en-nz.js",
	"./eo": "../../../../moment/locale/eo.js",
	"./eo.js": "../../../../moment/locale/eo.js",
	"./es": "../../../../moment/locale/es.js",
	"./es-do": "../../../../moment/locale/es-do.js",
	"./es-do.js": "../../../../moment/locale/es-do.js",
	"./es-us": "../../../../moment/locale/es-us.js",
	"./es-us.js": "../../../../moment/locale/es-us.js",
	"./es.js": "../../../../moment/locale/es.js",
	"./et": "../../../../moment/locale/et.js",
	"./et.js": "../../../../moment/locale/et.js",
	"./eu": "../../../../moment/locale/eu.js",
	"./eu.js": "../../../../moment/locale/eu.js",
	"./fa": "../../../../moment/locale/fa.js",
	"./fa.js": "../../../../moment/locale/fa.js",
	"./fi": "../../../../moment/locale/fi.js",
	"./fi.js": "../../../../moment/locale/fi.js",
	"./fo": "../../../../moment/locale/fo.js",
	"./fo.js": "../../../../moment/locale/fo.js",
	"./fr": "../../../../moment/locale/fr.js",
	"./fr-ca": "../../../../moment/locale/fr-ca.js",
	"./fr-ca.js": "../../../../moment/locale/fr-ca.js",
	"./fr-ch": "../../../../moment/locale/fr-ch.js",
	"./fr-ch.js": "../../../../moment/locale/fr-ch.js",
	"./fr.js": "../../../../moment/locale/fr.js",
	"./fy": "../../../../moment/locale/fy.js",
	"./fy.js": "../../../../moment/locale/fy.js",
	"./gd": "../../../../moment/locale/gd.js",
	"./gd.js": "../../../../moment/locale/gd.js",
	"./gl": "../../../../moment/locale/gl.js",
	"./gl.js": "../../../../moment/locale/gl.js",
	"./gom-latn": "../../../../moment/locale/gom-latn.js",
	"./gom-latn.js": "../../../../moment/locale/gom-latn.js",
	"./gu": "../../../../moment/locale/gu.js",
	"./gu.js": "../../../../moment/locale/gu.js",
	"./he": "../../../../moment/locale/he.js",
	"./he.js": "../../../../moment/locale/he.js",
	"./hi": "../../../../moment/locale/hi.js",
	"./hi.js": "../../../../moment/locale/hi.js",
	"./hr": "../../../../moment/locale/hr.js",
	"./hr.js": "../../../../moment/locale/hr.js",
	"./hu": "../../../../moment/locale/hu.js",
	"./hu.js": "../../../../moment/locale/hu.js",
	"./hy-am": "../../../../moment/locale/hy-am.js",
	"./hy-am.js": "../../../../moment/locale/hy-am.js",
	"./id": "../../../../moment/locale/id.js",
	"./id.js": "../../../../moment/locale/id.js",
	"./is": "../../../../moment/locale/is.js",
	"./is.js": "../../../../moment/locale/is.js",
	"./it": "../../../../moment/locale/it.js",
	"./it.js": "../../../../moment/locale/it.js",
	"./ja": "../../../../moment/locale/ja.js",
	"./ja.js": "../../../../moment/locale/ja.js",
	"./jv": "../../../../moment/locale/jv.js",
	"./jv.js": "../../../../moment/locale/jv.js",
	"./ka": "../../../../moment/locale/ka.js",
	"./ka.js": "../../../../moment/locale/ka.js",
	"./kk": "../../../../moment/locale/kk.js",
	"./kk.js": "../../../../moment/locale/kk.js",
	"./km": "../../../../moment/locale/km.js",
	"./km.js": "../../../../moment/locale/km.js",
	"./kn": "../../../../moment/locale/kn.js",
	"./kn.js": "../../../../moment/locale/kn.js",
	"./ko": "../../../../moment/locale/ko.js",
	"./ko.js": "../../../../moment/locale/ko.js",
	"./ky": "../../../../moment/locale/ky.js",
	"./ky.js": "../../../../moment/locale/ky.js",
	"./lb": "../../../../moment/locale/lb.js",
	"./lb.js": "../../../../moment/locale/lb.js",
	"./lo": "../../../../moment/locale/lo.js",
	"./lo.js": "../../../../moment/locale/lo.js",
	"./lt": "../../../../moment/locale/lt.js",
	"./lt.js": "../../../../moment/locale/lt.js",
	"./lv": "../../../../moment/locale/lv.js",
	"./lv.js": "../../../../moment/locale/lv.js",
	"./me": "../../../../moment/locale/me.js",
	"./me.js": "../../../../moment/locale/me.js",
	"./mi": "../../../../moment/locale/mi.js",
	"./mi.js": "../../../../moment/locale/mi.js",
	"./mk": "../../../../moment/locale/mk.js",
	"./mk.js": "../../../../moment/locale/mk.js",
	"./ml": "../../../../moment/locale/ml.js",
	"./ml.js": "../../../../moment/locale/ml.js",
	"./mr": "../../../../moment/locale/mr.js",
	"./mr.js": "../../../../moment/locale/mr.js",
	"./ms": "../../../../moment/locale/ms.js",
	"./ms-my": "../../../../moment/locale/ms-my.js",
	"./ms-my.js": "../../../../moment/locale/ms-my.js",
	"./ms.js": "../../../../moment/locale/ms.js",
	"./my": "../../../../moment/locale/my.js",
	"./my.js": "../../../../moment/locale/my.js",
	"./nb": "../../../../moment/locale/nb.js",
	"./nb.js": "../../../../moment/locale/nb.js",
	"./ne": "../../../../moment/locale/ne.js",
	"./ne.js": "../../../../moment/locale/ne.js",
	"./nl": "../../../../moment/locale/nl.js",
	"./nl-be": "../../../../moment/locale/nl-be.js",
	"./nl-be.js": "../../../../moment/locale/nl-be.js",
	"./nl.js": "../../../../moment/locale/nl.js",
	"./nn": "../../../../moment/locale/nn.js",
	"./nn.js": "../../../../moment/locale/nn.js",
	"./pa-in": "../../../../moment/locale/pa-in.js",
	"./pa-in.js": "../../../../moment/locale/pa-in.js",
	"./pl": "../../../../moment/locale/pl.js",
	"./pl.js": "../../../../moment/locale/pl.js",
	"./pt": "../../../../moment/locale/pt.js",
	"./pt-br": "../../../../moment/locale/pt-br.js",
	"./pt-br.js": "../../../../moment/locale/pt-br.js",
	"./pt.js": "../../../../moment/locale/pt.js",
	"./ro": "../../../../moment/locale/ro.js",
	"./ro.js": "../../../../moment/locale/ro.js",
	"./ru": "../../../../moment/locale/ru.js",
	"./ru.js": "../../../../moment/locale/ru.js",
	"./sd": "../../../../moment/locale/sd.js",
	"./sd.js": "../../../../moment/locale/sd.js",
	"./se": "../../../../moment/locale/se.js",
	"./se.js": "../../../../moment/locale/se.js",
	"./si": "../../../../moment/locale/si.js",
	"./si.js": "../../../../moment/locale/si.js",
	"./sk": "../../../../moment/locale/sk.js",
	"./sk.js": "../../../../moment/locale/sk.js",
	"./sl": "../../../../moment/locale/sl.js",
	"./sl.js": "../../../../moment/locale/sl.js",
	"./sq": "../../../../moment/locale/sq.js",
	"./sq.js": "../../../../moment/locale/sq.js",
	"./sr": "../../../../moment/locale/sr.js",
	"./sr-cyrl": "../../../../moment/locale/sr-cyrl.js",
	"./sr-cyrl.js": "../../../../moment/locale/sr-cyrl.js",
	"./sr.js": "../../../../moment/locale/sr.js",
	"./ss": "../../../../moment/locale/ss.js",
	"./ss.js": "../../../../moment/locale/ss.js",
	"./sv": "../../../../moment/locale/sv.js",
	"./sv.js": "../../../../moment/locale/sv.js",
	"./sw": "../../../../moment/locale/sw.js",
	"./sw.js": "../../../../moment/locale/sw.js",
	"./ta": "../../../../moment/locale/ta.js",
	"./ta.js": "../../../../moment/locale/ta.js",
	"./te": "../../../../moment/locale/te.js",
	"./te.js": "../../../../moment/locale/te.js",
	"./tet": "../../../../moment/locale/tet.js",
	"./tet.js": "../../../../moment/locale/tet.js",
	"./th": "../../../../moment/locale/th.js",
	"./th.js": "../../../../moment/locale/th.js",
	"./tl-ph": "../../../../moment/locale/tl-ph.js",
	"./tl-ph.js": "../../../../moment/locale/tl-ph.js",
	"./tlh": "../../../../moment/locale/tlh.js",
	"./tlh.js": "../../../../moment/locale/tlh.js",
	"./tr": "../../../../moment/locale/tr.js",
	"./tr.js": "../../../../moment/locale/tr.js",
	"./tzl": "../../../../moment/locale/tzl.js",
	"./tzl.js": "../../../../moment/locale/tzl.js",
	"./tzm": "../../../../moment/locale/tzm.js",
	"./tzm-latn": "../../../../moment/locale/tzm-latn.js",
	"./tzm-latn.js": "../../../../moment/locale/tzm-latn.js",
	"./tzm.js": "../../../../moment/locale/tzm.js",
	"./uk": "../../../../moment/locale/uk.js",
	"./uk.js": "../../../../moment/locale/uk.js",
	"./ur": "../../../../moment/locale/ur.js",
	"./ur.js": "../../../../moment/locale/ur.js",
	"./uz": "../../../../moment/locale/uz.js",
	"./uz-latn": "../../../../moment/locale/uz-latn.js",
	"./uz-latn.js": "../../../../moment/locale/uz-latn.js",
	"./uz.js": "../../../../moment/locale/uz.js",
	"./vi": "../../../../moment/locale/vi.js",
	"./vi.js": "../../../../moment/locale/vi.js",
	"./x-pseudo": "../../../../moment/locale/x-pseudo.js",
	"./x-pseudo.js": "../../../../moment/locale/x-pseudo.js",
	"./yo": "../../../../moment/locale/yo.js",
	"./yo.js": "../../../../moment/locale/yo.js",
	"./zh-cn": "../../../../moment/locale/zh-cn.js",
	"./zh-cn.js": "../../../../moment/locale/zh-cn.js",
	"./zh-hk": "../../../../moment/locale/zh-hk.js",
	"./zh-hk.js": "../../../../moment/locale/zh-hk.js",
	"./zh-tw": "../../../../moment/locale/zh-tw.js",
	"./zh-tw.js": "../../../../moment/locale/zh-tw.js"
};
function webpackContext(req) {
	return __webpack_require__(webpackContextResolve(req));
};
function webpackContextResolve(req) {
	var id = map[req];
	if(!(id + 1)) // check for number or string
		throw new Error("Cannot find module '" + req + "'.");
	return id;
};
webpackContext.keys = function webpackContextKeys() {
	return Object.keys(map);
};
webpackContext.resolve = webpackContextResolve;
module.exports = webpackContext;
webpackContext.id = "../../../../moment/locale recursive ^\\.\\/.*$";

/***/ }),

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__("../../../../../src/main.ts");


/***/ })

},[0]);
//# sourceMappingURL=main.bundle.js.map