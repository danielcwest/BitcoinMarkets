
import { Asset } from './asset';
import { PriceChange } from './price-change';

export class AssetGroup implements PriceChange {
    name: string;
    description: string;
    volume_24h_usd: number;
    market_cap_usd: number;
    percent_change_1h: number;
    percent_change_24h: number;
    percent_change_7d: number;

    market_cap_usd_1h: number;
    market_cap_usd_24h: number;
    market_cap_usd_7d: number;

    assets: Asset[];

    constructor(name: string, assetGroup: Asset[], description = '') {
        this.name = name;
        this.description = description;
        this.assets = assetGroup;
        this.reset();
    }

    reset(): void {
        this.volume_24h_usd = 0;
        this.market_cap_usd = 0;

        this.market_cap_usd_1h = 0;
        this.market_cap_usd_24h = 0;
        this.market_cap_usd_7d = 0;

        this.percent_change_1h = 0;
        this.percent_change_24h = 0;
        this.percent_change_7d = 0;

        this.assets.forEach(a => {
            this.volume_24h_usd += a['24h_volume_usd'];
            this.market_cap_usd += a.market_cap_usd;
            this.market_cap_usd_1h += a.market_cap_usd_1h;
            this.market_cap_usd_24h += a.market_cap_usd_24h;
            this.market_cap_usd_7d += a.market_cap_usd_7d;
        });

        this.percent_change_1h = ((this.market_cap_usd - this.market_cap_usd_1h) / this.market_cap_usd_1h) * 100;
        this.percent_change_24h = ((this.market_cap_usd - this.market_cap_usd_24h) / this.market_cap_usd_24h) * 100;
        this.percent_change_7d = ((this.market_cap_usd - this.market_cap_usd_7d) / this.market_cap_usd_7d) * 100;

    }
}