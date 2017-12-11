"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var moment = require("moment");
var Asset = /** @class */ (function () {
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
        this.last_updated = moment.unix(Number(item.last_updated)).toDate();
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
exports.Asset = Asset;
//# sourceMappingURL=asset.js.map