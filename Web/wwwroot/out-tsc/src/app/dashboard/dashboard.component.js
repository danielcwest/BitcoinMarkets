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
var bittrex_service_1 = require("../services/bittrex.service");
var asset_group_1 = require("../models/asset-group");
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
                _this.global = new asset_group_1.AssetGroup('all', assets, '');
                _this.createAssetGroups(assets, groups);
            });
        });
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
    __metadata("design:paramtypes", [config_service_1.ConfigService, bittrex_service_1.BittrexService, coinmarketcap_service_1.CoinMarketCapService, context_service_1.ContextService])
], DashboardComponent);
exports.DashboardComponent = DashboardComponent;
//# sourceMappingURL=dashboard.component.js.map