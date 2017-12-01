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
var config_service_1 = require("../services/config.service");
var coinmarketcap_service_1 = require("../services/coinmarketcap.service");
var context_service_1 = require("../services/context.service");
var arbitrage_service_1 = require("../services/arbitrage.service");
var asset_group_1 = require("../models/asset-group");
var DashboardComponent = (function () {
    function DashboardComponent(configService, coincap, contextService, arbitrageService) {
        this.configService = configService;
        this.coincap = coincap;
        this.contextService = contextService;
        this.arbitrageService = arbitrageService;
        this.isLoading = false;
    }
    DashboardComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.contextService.context$.subscribe(function (context) {
            _this.context = context;
            _this.refresh();
        });
    };
    DashboardComponent.prototype.setInterval = function (interval) {
        this.contextService.setInterval(interval);
    };
    DashboardComponent.prototype.refresh = function () {
    };
    DashboardComponent.prototype.createAssetGroups = function (assets, groups) {
        var ags = [];
        groups.forEach(function (group) {
            var groupAssets = assets.filter(function (a) { return group.symbols.indexOf(a.symbol) != -1; });
            var ag = new asset_group_1.AssetGroup(group.name, groupAssets, group.description);
            ags.push(ag);
        });
        this.assetGroups = ags;
    };
    return DashboardComponent;
}());
DashboardComponent = __decorate([
    core_1.Component({
        selector: 'app-dashboard',
        templateUrl: './dashboard.component.html',
        styleUrls: ['./dashboard.component.css']
    }),
    __metadata("design:paramtypes", [config_service_1.ConfigService, coinmarketcap_service_1.CoinMarketCapService, context_service_1.ContextService, arbitrage_service_1.ArbitrageService])
], DashboardComponent);
exports.DashboardComponent = DashboardComponent;
//# sourceMappingURL=dashboard.component.js.map