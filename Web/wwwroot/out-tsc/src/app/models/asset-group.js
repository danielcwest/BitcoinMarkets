"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AssetGroup = /** @class */ (function () {
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
exports.AssetGroup = AssetGroup;
//# sourceMappingURL=asset-group.js.map