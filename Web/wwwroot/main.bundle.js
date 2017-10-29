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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__dashboard_dashboard_component__ = __webpack_require__("../../../../../src/app/dashboard/dashboard.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__arbitrage_arbitrage_component__ = __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__arbitrage_detail_detail_component__ = __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};





var routes = [
    {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        component: __WEBPACK_IMPORTED_MODULE_2__dashboard_dashboard_component__["a" /* DashboardComponent */]
    },
    {
        path: 'arbitrage',
        component: __WEBPACK_IMPORTED_MODULE_3__arbitrage_arbitrage_component__["a" /* ArbitrageComponent */]
    },
    {
        path: 'arbitrage/:ticker',
        component: __WEBPACK_IMPORTED_MODULE_4__arbitrage_detail_detail_component__["a" /* DetailComponent */]
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
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/app.component.html":
/***/ (function(module, exports) {

module.exports = "<nav class=\"navbar navbar-expand-lg navbar-light bg-light\">\r\n\t<a class=\"navbar-brand\" href=\"#\">Bitcoin Markets</a>\r\n\t<button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\"#navbarNavAltMarkup\" aria-controls=\"navbarNavAltMarkup\"\r\n\t    aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n      <span class=\"navbar-toggler-icon\"></span>\r\n    </button>\r\n\t<div class=\"collapse navbar-collapse\" id=\"navbarNavAltMarkup\">\r\n\t\t<div class=\"navbar-nav\">\r\n\t\t\t<a routerLink=\"/dashboard\" routerLinkActive=\"active\" class=\"nav-item nav-link\">Dashboard</a>\r\n\t\t\t<a routerLink=\"/arbitrage\" routerLinkActive=\"active\" class=\"nav-item nav-link\">Arbitrage</a>\r\n\t\t</div>\r\n\t</div>\r\n</nav>\r\n<div class=\"container-fluid\">\r\n\t<router-outlet>\r\n\t</router-outlet>\r\n</div>\r\n"

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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__dashboard_dashboard_component__ = __webpack_require__("../../../../../src/app/dashboard/dashboard.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__arbitrage_arbitrage_component__ = __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__arbitrage_detail_detail_component__ = __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.ts");
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
            __WEBPACK_IMPORTED_MODULE_8__dashboard_dashboard_component__["a" /* DashboardComponent */],
            __WEBPACK_IMPORTED_MODULE_9__arbitrage_arbitrage_component__["a" /* ArbitrageComponent */],
            __WEBPACK_IMPORTED_MODULE_10__arbitrage_detail_detail_component__["a" /* DetailComponent */]
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

module.exports = "<div class=\"row mt-4\">\r\n\t<div class=\"col\">\r\n\t\t<h1>Arbitrage</h1>\r\n\t</div>\r\n\t<div class=\"col text-right\">\r\n\t\t<button class=\"btn\" (click)=\"refreshMarkets()\">Refresh</button>\r\n\t</div>\r\n</div>\r\n<hr/>\r\n<div class=\"row mt-4\">\r\n\t<div class=\"col\">\r\n\t\t<select class=\"custom-select\" (change)=\"onBaseExchangeChange($event.target.value)\">\r\n                    <option *ngFor=\"let exchange of exchanges\" [value]=\"exchange\" [selected]=\"context.selectedBaseExchange == exchange\">{{exchange}}</option>\r\n                  </select>\r\n\t</div>\r\n\t<div class=\"col text-right\">\r\n\t\t<select class=\"custom-select\" (change)=\"onArbExchangeChange($event.target.value)\">\r\n                <option *ngFor=\"let exchange of exchanges\" [value]=\"exchange\" [selected]=\"context.selectedArbExchange == exchange\">{{exchange}}</option>\r\n              </select>\r\n\t</div>\r\n</div>\r\n<hr/>\r\n<div *ngIf=\"isLoading\" class=\"load\">\r\n\t<div class=\"loader mx-auto\"></div>\r\n</div>\r\n<table class=\"table\" *ngIf=\"arbitrageMarkets.length > 0\">\r\n\t<thead>\r\n\t\t<tr>\r\n\t\t\t<th (click)=\"changeSort('symbol')\">Market\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'symbol'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t\t<th (click)=\"changeSort('baseVolume')\">{{context.selectedBaseExchange}} Volume\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'baseVolume'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t\t<th (click)=\"changeSort('arbVolume')\">{{context.selectedArbExchange}} Volume\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'arbVolume'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t\t<th (click)=\"changeSort('baseLast')\">{{context.selectedBaseExchange}} Price\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'baseLast'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t\t<th (click)=\"changeSort('arbLast')\">{{context.selectedArbExchange}} Price\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'arbLast'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t\t<th (click)=\"changeSort('spreadAbs')\">Spread\r\n\t\t\t\t<span *ngIf=\"sortProperty == 'spreadAbs'\" class=\"oi\" [class.oi-caret-top]=\"sortAscending\" [class.oi-caret-bottom]=\"!sortAscending\"\r\n\t\t\t\t    title=\"Sort\" aria-hidden=\"true\"></span>\r\n\t\t\t</th>\r\n\t\t</tr>\r\n\t</thead>\r\n\t<tbody>\r\n\t\t<tr *ngFor=\"let mkt of arbitrageMarkets | orderBy:sortProperty:sortAscending\" [class.table-success]=\"mkt.spreadExact >= .02 && mkt.arbVolume > 25 && mkt.baseVolume > 25\">\r\n\t\t\t<th scope=\"row\"><a [routerLink]=\"[ './', mkt.symbol ]\">{{mkt.symbol}}</a></th>\r\n\t\t\t<td><a href=\"{{mkt.baseLink}}\" target=\"_blank\">{{mkt.baseVolume | numeral:'0,0.00'}}</a></td>\r\n\t\t\t<td><a href=\"{{mkt.arbLink}}\" target=\"_blank\">{{mkt.arbVolume | numeral:'0,0.00'}}</a></td>\r\n\t\t\t<td><a href=\"{{mkt.baseLink}}\" target=\"_blank\">{{mkt.baseLast | btcPrice}}</a></td>\r\n\t\t\t<td><a href=\"{{mkt.arbLink}}\" target=\"_blank\">{{mkt.arbLast | btcPrice}}</a></td>\r\n\t\t\t<td>{{mkt.spreadAbs | numeral:'0.00%'}}</td>\r\n\t\t</tr>\r\n\t</tbody>\r\n</table>\r\n"

/***/ }),

/***/ "../../../../../src/app/arbitrage/arbitrage.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ArbitrageComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_arbitrage_market__ = __webpack_require__("../../../../../src/app/models/arbitrage-market.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__ = __webpack_require__("../../../../../src/app/services/arbitrage.service.ts");
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
                arbMkts.push(new __WEBPACK_IMPORTED_MODULE_1__models_arbitrage_market__["a" /* ArbitrageMarket */](_this.baseExchangeMarkets.getValue(market), _this.arbExchangeMarkets.getValue(market)));
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
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-arbitrage',
        template: __webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.html"),
        styles: [__webpack_require__("../../../../../src/app/arbitrage/arbitrage.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__services_exchange_service__["a" /* ExchangeService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_exchange_service__["a" /* ExchangeService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_context_service__["a" /* ContextService */]) === "function" && _c || Object])
], ArbitrageComponent);

var _a, _b, _c;
//# sourceMappingURL=arbitrage.component.js.map

/***/ }),

/***/ "../../../../../src/app/arbitrage/detail/detail.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "input {\r\n\twidth: 150px;\r\n\tmargin: 0 auto;\r\n}\r\n\r\n.arb-pair {\r\n\tpadding: 25px;\r\n}\r\n\r\n.success h1 {\r\n\tfont-size: 4.0rem;\r\n}\r\n\r\n.danger h1 {\r\n\tfont-size: 2.0rem;\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/arbitrage/detail/detail.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"row mt-4\">\r\n    <div class=\"col\">\r\n        <h1 class=\"display-2\">{{symbol}}</h1>\r\n    </div>\r\n    <div class=\"col text-right\">\r\n        <button class=\"btn\" (click)=\"refreshMarkets()\">Refresh</button>\r\n    </div>\r\n</div>\r\n<hr/>\r\n<div class=\"row mt-4\">\r\n    <div class=\"col text-center\">\r\n        <h4>{{baseExchangeMarket.exchange}}</h4>\r\n        <div class=\"form-group\">\r\n            <button class=\"btn\" (click)=\"BuyBase()\">Buy</button>\r\n            <button class=\"btn\" (click)=\"SellBase()\">Sell</button>\r\n        </div>\r\n    </div>\r\n    <div class=\"col text-center\">\r\n        <div class=\"form-group\">\r\n            <label for=\"thresholdInput\">Buy/Sell Threshold</label>\r\n            <input type=\"number\" class=\"form-control\" id=\"thresholdInput\" [(ngModel)]=\"threshold\">\r\n        </div>\r\n    </div>\r\n    <div class=\"col text-center\">\r\n        <h4>{{arbExchangeMarket.exchange}}</h4>\r\n\r\n        <div class=\"form-group\">\r\n            <button class=\"btn\" (click)=\"BuyArb()\">Buy</button>\r\n            <button class=\"btn\" (click)=\"SellArb()\">Sell</button>\r\n        </div>\r\n    </div>\r\n</div>\r\n<hr/>\r\n<div *ngIf=\"isLoading\" class=\"load\">\r\n    <div class=\"loader mx-auto\"></div>\r\n</div>\r\n<div class=\"card\">\r\n    <div class=\"car-body arb-pair\">\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\">\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\"> {{baseExchangeMarket.exchange}} Last Price</h4>\r\n            </div>\r\n            <div class=\"col\"></div>\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\">{{arbExchangeMarket.exchange}} Last Price</h4>\r\n            </div>\r\n        </div>\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\">\r\n            <div class=\"col\">\r\n                <!-- <h1 class=\"text-center\" [class.text-success]=\"baseExchangeMarket.last > arbExchangeMarket.last\" [class.text-danger]=\"baseExchangeMarket.last < arbExchangeMarket.last\">{{baseExchangeMarket.last | btcPrice}}</h1> -->\r\n                <h1 class=\"text-center\">{{baseExchangeMarket.last | btcPrice}}</h1>\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 class=\"text-center\">{{spreadLast | numeral:'0.00%'}}</h1>\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 class=\"text-center\">{{arbExchangeMarket.last | btcPrice}}</h1>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<hr/>\r\n\r\n<!-- BUY at BASE *** SELL at ARB -->\r\n<div class=\"card\">\r\n    <div class=\"buy-base card-body arb-pair\" [class.success]=\"baseExactBuy < arbExactSell\" [class.danger]=\"baseExactBuy > arbExactSell\">\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\">\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\">Buy {{threshold}} {{baseCurrency}}</h4>\r\n            </div>\r\n            <div class=\"col\"></div>\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\">Sell {{threshold}} {{baseCurrency}}</h4>\r\n            </div>\r\n        </div>\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\">\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactBuy > arbExactSell \" class=\"text-center\">{{baseExactBuy | btcPrice}}</h1>\r\n                <h1 *ngIf=\"baseExactBuy < arbExactSell \" class=\"text-center\" [class.text-success]=\"baseExactBuy < arbExactSell\" [class.text-danger]=\"baseExactBuy > arbExactSell\">{{baseExactBuy | btcPrice}}</h1>\r\n\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactBuy < arbExactSell\" class=\"text-center spread\">{{baseBuySpread - (arbExchangeFee + baseExchangeFee) | numeral:'0.00%'}}</h1>\r\n                <h4 *ngIf=\"baseExactBuy < arbExactSell\" class=\"text-center spread\">{{baseBuySpread | numeral:'0.00%'}}</h4>\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactBuy > arbExactSell\" class=\"text-center\">{{arbExactSell | btcPrice}}</h1>\r\n                <h1 *ngIf=\"baseExactBuy < arbExactSell\" class=\"text-center\" [class.text-success]=\"baseExactBuy > arbExactSell\" [class.text-danger]=\"baseExactBuy < arbExactSell\">{{arbExactSell | btcPrice}}</h1>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<hr/>\r\n\r\n<!-- SELL at BASE *** BUY at ARB -->\r\n<div class=\"card\">\r\n    <div class=\"sell-base card-body arb-pair\" [class.success]=\"baseExactSell > arbExactBuy\" [class.danger]=\"baseExactSell < arbExactBuy\">\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\">\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\">Sell {{threshold}} {{baseCurrency}}</h4>\r\n            </div>\r\n            <div class=\"col\"></div>\r\n            <div class=\"col\">\r\n                <h4 class=\"text-center\">Buy {{threshold}} {{baseCurrency}}</h4>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"row\" *ngIf=\"baseExchangeMarket && arbExchangeMarket\" [class.success]=\"baseSellSpread > 0\">\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactSell < arbExactBuy\" class=\"text-center\">{{baseExactSell | btcPrice}}</h1>\r\n                <h1 *ngIf=\"baseExactSell > arbExactBuy\" class=\"text-center\" [class.text-danger]=\"baseExactSell > arbExactBuy\">{{baseExactSell | btcPrice}}</h1>\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactSell > arbExactBuy\" class=\"text-center spread\">{{baseSellSpread - (arbExchangeFee + baseExchangeFee) | numeral:'0.00%'}}</h1>\r\n                <h4 *ngIf=\"baseExactSell > arbExactBuy\" class=\"text-center spread\">{{baseSellSpread | numeral:'0.00%'}}</h4>\r\n            </div>\r\n            <div class=\"col\">\r\n                <h1 *ngIf=\"baseExactSell < arbExactBuy\" class=\"text-center\">{{arbExactBuy | btcPrice}}</h1>\r\n                <h1 *ngIf=\"baseExactSell > arbExactBuy\" class=\"text-center\" [class.text-success]=\"baseExactSell > arbExactBuy\">{{arbExactBuy | btcPrice}}</h1>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<hr/>"

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
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-detail',
        template: __webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.html"),
        styles: [__webpack_require__("../../../../../src/app/arbitrage/detail/detail.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* ActivatedRoute */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["a" /* ActivatedRoute */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* Router */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* Router */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_exchange_service__["a" /* ExchangeService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_arbitrage_service__["a" /* ArbitrageService */]) === "function" && _e || Object])
], DetailComponent);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=detail.component.js.map

/***/ }),

/***/ "../../../../../src/app/dashboard/dashboard.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, ".currency-logo {\r\n\t/* position: absolute;\r\n    left: 10px; */\r\n\tfloat: left;\r\n\twidth: 16px;\r\n\theight: 16px;\r\n}\r\n\r\n.price {\r\n\tfont-weight: bold;\r\n}\r\n\r\n.price-up {\r\n\tcolor: green;\r\n}\r\n\r\n.price-down {\r\n\tcolor: red;\r\n}\r\n\r\n.interval-buttons {\r\n\theight: 100%;\r\n}\r\n\r\n.interval.active {\r\n\tfont-weight: bold;\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/dashboard/dashboard.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"row mt-4\">\r\n\t<div class=\"col\">\r\n\t\t<h1>Market Summary</h1>\r\n\t</div>\r\n\t<div class=\"col\">\r\n\t\t<div class=\"btn-group float-right interval-buttons\" role=\"group\" aria-label=\"Basic example\">\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('1h')\" [class.active]=\"context.interval == '1h'\">1h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('24h')\" [class.active]=\"context.interval == '24h'\">24h</button>\r\n\t\t\t<button type=\"button\" class=\"btn btn-link interval\" (click)=\"setInterval('7d')\" [class.active]=\"context.interval == '7d'\">7d</button>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n\r\n<div class=\"jumbotron jumbotron-fluid\">\r\n\t<div class=\"container\" *ngIf=\"global\">\r\n\t\t<div class=\"row text-center\">\r\n\t\t\t<div class=\"col\">\r\n\t\t\t\t<h1 class=\"display-4\">${{global.market_cap_usd | commaSeparatedNumber}}</h1>\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col\">\r\n\t\t\t\t<h1 class=\"display-4\">\r\n\t\t\t\t\t<app-price-change [priceChange]=\"global\" [shortHand]=\"true\"></app-price-change>\r\n\t\t\t\t</h1>\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n<div class=\"row\">\r\n\t<div class=\"col\" *ngFor=\"let group of assetGroups\">\r\n\t\t<div class=\"card\">\r\n\t\t\t<div class=\"card-body\">\r\n\t\t\t\t<h4 class=\"card-title\">${{group.market_cap_usd | commaSeparatedNumber}}</h4>\r\n\t\t\t\t<p class=\"card-text\" *ngIf=\"context.interval == '1h'\">{{group.percent_change_1h}}%</p>\r\n\t\t\t\t<p class=\"card-text\" *ngIf=\"context.interval == '24h'\">{{group.percent_change_24h}}%</p>\r\n\t\t\t\t<p class=\"card-text\" *ngIf=\"context.interval == '7d'\">{{group.percent_change_7d}}%</p>\r\n\t\t\t</div>\r\n\t\t\t<ul class=\"list-group list-group-flush\">\r\n\t\t\t\t<li *ngFor=\"let item of group.assets\" class=\"list-group-item\">\r\n\t\t\t\t\t<img src=\"https://files.coinmarketcap.com/static/img/coins/16x16/{{item.id}}.png\" class=\"currency-logo\">{{item.name}}\r\n\t\t\t\t\t<span *ngIf=\"context.interval == '1h'\" class=\"price\" [class.price-down]=\"item.percent_change_1h < 0\" [class.price-up]=\"item.percent_change_1h > 0\">{{item.price_usd}}</span>\r\n\t\t\t\t\t<span *ngIf=\"context.interval == '24h'\" class=\"price\" [class.price-down]=\"item.percent_change_24h < 0\" [class.price-up]=\"item.percent_change_24h > 0\">{{item.price_usd}}</span>\r\n\t\t\t\t\t<span *ngIf=\"context.interval == '7d'\" class=\"price\" [class.price-down]=\"item.percent_change_7d < 0\" [class.price-up]=\"item.percent_change_7d > 0\">{{item.price_usd}}</span>\r\n\r\n\t\t\t\t</li>\r\n\t\t\t</ul>\r\n\t\t</div>\r\n\t</div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/dashboard/dashboard.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_config_service__ = __webpack_require__("../../../../../src/app/services/config.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__services_coinmarketcap_service__ = __webpack_require__("../../../../../src/app/services/coinmarketcap.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__services_bittrex_service__ = __webpack_require__("../../../../../src/app/services/bittrex.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__models_asset_group__ = __webpack_require__("../../../../../src/app/models/asset-group.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var DashboardComponent = (function () {
    function DashboardComponent(configService, bittrex, coinMarketCapService, contextService) {
        this.configService = configService;
        this.bittrex = bittrex;
        this.coinMarketCapService = coinMarketCapService;
        this.contextService = contextService;
    }
    DashboardComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
        this.refresh();
    };
    DashboardComponent.prototype.setInterval = function (interval) {
        this.contextService.setInterval(interval);
    };
    DashboardComponent.prototype.refresh = function () {
        var _this = this;
        this.configService.getGroupConfig().then(function (groups) {
            _this.coinMarketCapService.getAssets().then(function (assets) {
                //Aggregate all assets
                _this.global = new __WEBPACK_IMPORTED_MODULE_5__models_asset_group__["a" /* AssetGroup */]('all', assets, '');
                _this.createAssetGroups(assets, groups);
            });
        });
    };
    DashboardComponent.prototype.createAssetGroups = function (assets, groups) {
        var ags = [];
        groups.forEach(function (group) {
            var groupAssets = assets.filter(function (a) { return group.symbols.indexOf(a.symbol) != -1; });
            var ag = new __WEBPACK_IMPORTED_MODULE_5__models_asset_group__["a" /* AssetGroup */](group.name, groupAssets, group.description);
            ags.push(ag);
        });
        this.assetGroups = ags;
    };
    return DashboardComponent;
}());
DashboardComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-dashboard',
        template: __webpack_require__("../../../../../src/app/dashboard/dashboard.component.html"),
        styles: [__webpack_require__("../../../../../src/app/dashboard/dashboard.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__services_config_service__["a" /* ConfigService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_config_service__["a" /* ConfigService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__services_bittrex_service__["a" /* BittrexService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__services_bittrex_service__["a" /* BittrexService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_2__services_coinmarketcap_service__["a" /* CoinMarketCapService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__services_coinmarketcap_service__["a" /* CoinMarketCapService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__services_context_service__["a" /* ContextService */]) === "function" && _d || Object])
], DashboardComponent);

var _a, _b, _c, _d;
//# sourceMappingURL=dashboard.component.js.map

/***/ }),

/***/ "../../../../../src/app/models/arbitrage-market.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ArbitrageMarket; });
var ArbitrageMarket = (function () {
    function ArbitrageMarket(base, arb) {
        //Should be the same for each
        this.quoteCurrency = base.quoteCurrency;
        this.baseCurrency = base.baseCurrency;
        this.symbol = base.symbol;
        this.baseLink = base.link;
        this.arbLink = arb.link;
        this.baseVolume = base.volume;
        this.arbVolume = arb.volume;
        this.baseLast = base.last;
        this.arbLast = arb.last;
        this.baseAsk = base.ask;
        this.arbAsk = arb.ask;
        this.baseBid = base.bid;
        this.arbBid = arb.bid;
        this.spread = (base.last - arb.last) / base.last;
        this.spreadAbs = Math.abs(this.spread);
        //If Bases's price is lower, you want to buy on Base so you are looking for the lowest ask
        if (this.spread < 0) {
            this.spreadExact = Math.abs((base.ask - arb.bid) / base.ask);
            this.basePriceExact = base.ask;
            this.arbPriceExact = arb.bid;
        }
        else if (this.spread > 0) {
            this.spreadExact = Math.abs((base.bid - arb.ask) / base.bid);
            this.basePriceExact = base.bid;
            this.arbPriceExact = arb.ask;
        }
        else {
            this.spreadExact = 0;
        }
    }
    return ArbitrageMarket;
}());

//# sourceMappingURL=arbitrage-market.js.map

/***/ }),

/***/ "../../../../../src/app/models/asset-group.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AssetGroup; });
var AssetGroup = (function () {
    function AssetGroup(name, assetGroup, description) {
        if (description === void 0) { description = ''; }
        this.name = name;
        this.description = description;
        this.assets = assetGroup;
        this.reset();
    }
    AssetGroup.prototype.reset = function () {
        var _this = this;
        this.volume_24h_usd = 0;
        this.market_cap_usd = 0;
        this.market_cap_usd_1h = 0;
        this.market_cap_usd_24h = 0;
        this.market_cap_usd_7d = 0;
        this.percent_change_1h = 0;
        this.percent_change_24h = 0;
        this.percent_change_7d = 0;
        this.assets.forEach(function (a) {
            _this.volume_24h_usd += a['24h_volume_usd'];
            _this.market_cap_usd += a.market_cap_usd;
            _this.market_cap_usd_1h += a.market_cap_usd_1h;
            _this.market_cap_usd_24h += a.market_cap_usd_24h;
            _this.market_cap_usd_7d += a.market_cap_usd_7d;
        });
        this.percent_change_1h = ((this.market_cap_usd - this.market_cap_usd_1h) / this.market_cap_usd_1h) * 100;
        this.percent_change_24h = ((this.market_cap_usd - this.market_cap_usd_24h) / this.market_cap_usd_24h) * 100;
        this.percent_change_7d = ((this.market_cap_usd - this.market_cap_usd_7d) / this.market_cap_usd_7d) * 100;
    };
    return AssetGroup;
}());

//# sourceMappingURL=asset-group.js.map

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

/***/ "../../../../../src/app/models/price-change.ts":
/***/ (function(module, exports) {

//# sourceMappingURL=price-change.js.map

/***/ }),

/***/ "../../../../../src/app/services/arbitrage.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ArbitrageService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_arbitrage_market__ = __webpack_require__("../../../../../src/app/models/arbitrage-market.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
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
                var dic = new __WEBPACK_IMPORTED_MODULE_4_typescript_collections__["Dictionary"]();
                baseExchangeMarkets.forEach(function (market) {
                    if (arbExchangeMarkets.containsKey(market)) {
                        dic.setValue(market, new __WEBPACK_IMPORTED_MODULE_1__models_arbitrage_market__["a" /* ArbitrageMarket */](baseExchangeMarkets.getValue(market), arbExchangeMarkets.getValue(market)));
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
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__exchange_service__["a" /* ExchangeService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__exchange_service__["a" /* ExchangeService */]) === "function" && _b || Object])
], ArbitrageService);

var _a, _b;
//# sourceMappingURL=arbitrage.service.js.map

/***/ }),

/***/ "../../../../../src/app/services/bittrex.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return BittrexService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_typescript_collections__ = __webpack_require__("../../../../typescript-collections/dist/lib/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_typescript_collections___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_typescript_collections__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var BittrexService = (function () {
    function BittrexService(http, contextService) {
        var _this = this;
        this.http = http;
        this.contextService = contextService;
        this.contextService.context$.subscribe(function (context) { return _this.context = context; });
    }
    BittrexService.prototype.getMarketSummary = function () {
        return this.http.get("/api/bittrex/marketsummaries/").toPromise().then(function (response) {
            var result = response.json();
            var dic = new __WEBPACK_IMPORTED_MODULE_3_typescript_collections__["Dictionary"]();
            result.forEach(function (market) { return dic.setValue(market.symbol, market); });
            return dic;
        });
    };
    BittrexService.prototype.handleError = function (error) {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    return BittrexService;
}());
BittrexService = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object])
], BittrexService);

var _a, _b;
//# sourceMappingURL=bittrex.service.js.map

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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__);
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
        this.intervals = ['1h', '24h', '7d'];
        this.contextSource = new __WEBPACK_IMPORTED_MODULE_1_rxjs_Rx__["BehaviorSubject"]({ interval: '24h', assetCount: 250, selectedBaseExchange: 'Bittrex', selectedArbExchange: 'Hitbtc' });
        this.context$ = this.contextSource.asObservable();
        this.context = { interval: '24h', assetCount: 250, selectedBaseExchange: 'Bittrex', selectedArbExchange: 'Hitbtc' };
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
    ContextService.prototype.setBaseExchange = function (exchange) {
        this.context.selectedBaseExchange = exchange;
        this.contextSource.next(this.context);
    };
    ContextService.prototype.setArbExchange = function (exchange) {
        this.context.selectedArbExchange = exchange;
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_typescript_collections__ = __webpack_require__("../../../../typescript-collections/dist/lib/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_typescript_collections___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_typescript_collections__);
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
    ExchangeService.prototype.getMarketSummaries = function (exchange) {
        return this.http.get("/api/" + exchange + "/marketsummaries/").toPromise().then(function (response) {
            var result = response.json();
            var dic = new __WEBPACK_IMPORTED_MODULE_3_typescript_collections__["Dictionary"]();
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
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["C" /* Injectable */])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__context_service__["a" /* ContextService */]) === "function" && _b || Object])
], ExchangeService);

var _a, _b;
//# sourceMappingURL=exchange.service.js.map

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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__bittrex_service__ = __webpack_require__("../../../../../src/app/services/bittrex.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__exchange_service__ = __webpack_require__("../../../../../src/app/services/exchange.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__arbitrage_service__ = __webpack_require__("../../../../../src/app/services/arbitrage.service.ts");
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
        providers: [__WEBPACK_IMPORTED_MODULE_2__coinmarketcap_service__["a" /* CoinMarketCapService */], __WEBPACK_IMPORTED_MODULE_3__config_service__["a" /* ConfigService */], __WEBPACK_IMPORTED_MODULE_4__context_service__["a" /* ContextService */], __WEBPACK_IMPORTED_MODULE_5__bittrex_service__["a" /* BittrexService */], __WEBPACK_IMPORTED_MODULE_6__exchange_service__["a" /* ExchangeService */], __WEBPACK_IMPORTED_MODULE_7__arbitrage_service__["a" /* ArbitrageService */]]
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

/***/ "../../../../../src/app/shared/price-change/price-change.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, ".price-up {\r\n\tcolor: green;\r\n}\r\n\r\n.price-down {\r\n\tcolor: red;\r\n}\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/shared/price-change/price-change.component.html":
/***/ (function(module, exports) {

module.exports = "<span *ngIf=\"isPercent\" class=\"price-change\" [class.price-down]=\"!isPos\" [class.price-up]=\"isPos\" (click)=\"isPercent = false\">\r\n    {{sign}}{{percentChange | commaSeparatedNumber}}%\r\n</span>\r\n<span *ngIf=\"!isPercent\" class=\"price-change\" [class.price-down]=\"!isPos\" [class.price-up]=\"isPos\" (click)=\"isPercent = true\">\r\n        {{sign}}${{netChange}}\r\n    </span>\r\n"

/***/ }),

/***/ "../../../../../src/app/shared/price-change/price-change.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PriceChangeComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_context_service__ = __webpack_require__("../../../../../src/app/services/context.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_price_change__ = __webpack_require__("../../../../../src/app/models/price-change.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_price_change___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2__models_price_change__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_numeral__ = __webpack_require__("../../../../numeral/numeral.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_numeral___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_numeral__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var PriceChangeComponent = (function () {
    function PriceChangeComponent(contextService) {
        this.contextService = contextService;
        this.shortHand = false;
        this.isPercent = true;
    }
    PriceChangeComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            if (context.interval == '1h') {
                _this.isPos = _this.priceChange.percent_change_1h > 0;
                _this.sign = _this.isPos ? '+' : '-';
                _this.percentChange = Math.abs(_this.priceChange.percent_change_1h);
                _this.netChange = Math.abs(_this.priceChange.market_cap_usd - _this.priceChange.market_cap_usd_1h);
            }
            else if (context.interval == '24h') {
                _this.isPos = _this.priceChange.percent_change_24h > 0;
                _this.sign = _this.isPos ? '+' : '-';
                _this.percentChange = Math.abs(_this.priceChange.percent_change_24h);
                _this.netChange = Math.abs(_this.priceChange.market_cap_usd - _this.priceChange.market_cap_usd_24h);
                console.log();
            }
            else if (context.interval == '7d') {
                _this.isPos = _this.priceChange.percent_change_7d > 0;
                _this.sign = _this.isPos ? '+' : '-';
                _this.percentChange = Math.abs(_this.priceChange.percent_change_7d);
                _this.netChange = Math.abs(_this.priceChange.market_cap_usd - _this.priceChange.market_cap_usd_7d);
            }
        });
    };
    PriceChangeComponent.prototype.formatShorthand = function (val) {
        return __WEBPACK_IMPORTED_MODULE_3_numeral__(val).format('0.00a');
    };
    return PriceChangeComponent;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["F" /* Input */])(),
    __metadata("design:type", typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__models_price_change__["PriceChange"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__models_price_change__["PriceChange"]) === "function" && _a || Object)
], PriceChangeComponent.prototype, "priceChange", void 0);
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["F" /* Input */])(),
    __metadata("design:type", Boolean)
], PriceChangeComponent.prototype, "shortHand", void 0);
PriceChangeComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["o" /* Component */])({
        selector: 'app-price-change',
        template: __webpack_require__("../../../../../src/app/shared/price-change/price-change.component.html"),
        styles: [__webpack_require__("../../../../../src/app/shared/price-change/price-change.component.css")]
    }),
    __metadata("design:paramtypes", [typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__services_context_service__["a" /* ContextService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__services_context_service__["a" /* ContextService */]) === "function" && _b || Object])
], PriceChangeComponent);

var _a, _b;
//# sourceMappingURL=price-change.component.js.map

/***/ }),

/***/ "../../../../../src/app/shared/shared.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SharedModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__ = __webpack_require__("../../../../../src/app/shared/comma-separated-number.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__price_change_price_change_component__ = __webpack_require__("../../../../../src/app/shared/price-change/price-change.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__numeral_pipe__ = __webpack_require__("../../../../../src/app/shared/numeral.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__btc_price_pipe__ = __webpack_require__("../../../../../src/app/shared/btc-price.pipe.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__order_by_pipe__ = __webpack_require__("../../../../../src/app/shared/order-by.pipe.ts");
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
        declarations: [__WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__["a" /* CommaSeparatedNumberPipe */], __WEBPACK_IMPORTED_MODULE_3__price_change_price_change_component__["a" /* PriceChangeComponent */], __WEBPACK_IMPORTED_MODULE_4__numeral_pipe__["a" /* NumeralPipe */], __WEBPACK_IMPORTED_MODULE_5__btc_price_pipe__["a" /* BtcPricePipe */], __WEBPACK_IMPORTED_MODULE_6__order_by_pipe__["a" /* OrderByPipe */]],
        exports: [__WEBPACK_IMPORTED_MODULE_2__comma_separated_number_pipe__["a" /* CommaSeparatedNumberPipe */], __WEBPACK_IMPORTED_MODULE_3__price_change_price_change_component__["a" /* PriceChangeComponent */], __WEBPACK_IMPORTED_MODULE_4__numeral_pipe__["a" /* NumeralPipe */], __WEBPACK_IMPORTED_MODULE_5__btc_price_pipe__["a" /* BtcPricePipe */], __WEBPACK_IMPORTED_MODULE_6__order_by_pipe__["a" /* OrderByPipe */]]
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/add/operator/toPromise.js");
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