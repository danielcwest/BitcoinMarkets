import { Component, OnInit } from '@angular/core';

import { ConfigService } from '../services/config.service';
import { CoinMarketCapService } from '../services/coinmarketcap.service';
import { ContextService } from '../services/context.service';
import { ArbitrageService } from '../services/arbitrage.service';

import { HeroStat } from '../models/hero-stat';
import { Asset } from '../models/asset';
import { AssetGroup } from '../models/asset-group';
import { ConfigGroup } from '../models/config-group';
import { TickerGlobal } from '../models/ticker-global';
import { AppContext } from '../models/app-context';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    context: AppContext;

    assetGroups: AssetGroup[];
    global: AssetGroup;
	isLoading: boolean = false;

	constructor(private configService: ConfigService, private coincap: CoinMarketCapService, private contextService: ContextService, private arbitrageService: ArbitrageService) { }

    ngOnInit() {
		this.contextService.context$.subscribe(context => {
			this.context = context;
			this.refresh();
		});
    }

    setInterval(interval: string): void {
        this.contextService.setInterval(interval);
    }

	refresh(): void {
		
    }

    createAssetGroups(assets: Asset[], groups: ConfigGroup[]): void {
        let ags: AssetGroup[] = [];
        groups.forEach(group => {
            let groupAssets = assets.filter(a => group.symbols.indexOf(a.symbol) != -1);
            let ag = new AssetGroup(group.name, groupAssets, group.description);
            ags.push(ag);
        });
        this.assetGroups = ags;
    }
}
