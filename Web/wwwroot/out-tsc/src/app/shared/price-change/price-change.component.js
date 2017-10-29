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
var context_service_1 = require("../../services/context.service");
var numeral = require("numeral");
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
        return numeral(val).format('0.00a');
    };
    return PriceChangeComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], PriceChangeComponent.prototype, "priceChange", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], PriceChangeComponent.prototype, "shortHand", void 0);
PriceChangeComponent = __decorate([
    core_1.Component({
        selector: 'app-price-change',
        templateUrl: './price-change.component.html',
        styleUrls: ['./price-change.component.css']
    }),
    __metadata("design:paramtypes", [context_service_1.ContextService])
], PriceChangeComponent);
exports.PriceChangeComponent = PriceChangeComponent;
//# sourceMappingURL=price-change.component.js.map