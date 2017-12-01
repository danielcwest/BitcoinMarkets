"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var coinmarketcap_service_1 = require("./coinmarketcap.service");
var config_service_1 = require("./config.service");
var context_service_1 = require("./context.service");
var exchange_service_1 = require("./exchange.service");
var arbitrage_service_1 = require("./arbitrage.service");
var order_service_1 = require("./order.service");
var ServicesModule = (function () {
    function ServicesModule() {
    }
    return ServicesModule;
}());
ServicesModule = __decorate([
    core_1.NgModule({
        imports: [
            common_1.CommonModule
        ],
        declarations: [],
        providers: [coinmarketcap_service_1.CoinMarketCapService, config_service_1.ConfigService, context_service_1.ContextService, exchange_service_1.ExchangeService, arbitrage_service_1.ArbitrageService, order_service_1.OrderService]
    })
], ServicesModule);
exports.ServicesModule = ServicesModule;
//# sourceMappingURL=services.module.js.map