import { Component, OnInit } from '@angular/core';

import { ArbitragePair } from '../models/arbitrage-pair';
import { Asset } from '../models/asset';
import { HeroStat } from '../models/hero-stat';
import { IExchangeMarket } from '../models/exchange-market';
import { ContextService } from '../services/context.service';
import { ExchangeService } from '../services/exchange.service';
import { ArbitrageService } from '../services/arbitrage.service';
import { AppContext } from '../models/app-context';
import { CoinMarketCapService } from '../services/coinmarketcap.service';

import * as Collections from 'typescript-collections';

@Component({
    selector: 'app-arbitrage',
    templateUrl: './arbitrage.component.html',
    styleUrls: ['./arbitrage.component.css']
})
export class ArbitrageComponent implements OnInit {

    context: AppContext;

    sortProperty: string = 'symbol';
    sortAscending: boolean = true;
	isLoading: boolean = false;

	arbitragePairs: ArbitragePair[] = [];
	heroStats: Collections.Dictionary<string, HeroStat>;

	commission: number = 0;

	constructor(private arbitrageService: ArbitrageService, private contextService: ContextService, private coincap: CoinMarketCapService) { }

	ngOnInit() {
        this.contextService.context$.subscribe(context => {
			this.context = context;
			this.refresh();
		});
    }

    refresh(): void {
		this.isLoading = true;
		this.arbitrageService.getArbitragePairs().then(pairs => {
			this.arbitragePairs = pairs;
			this.isLoading = false;
		});

		this.arbitrageService.getHeroStats().then(heroStats => {
			this.heroStats = heroStats;

			if (heroStats.containsKey('TOTAL'))
				this.commission = heroStats.getValue('TOTAL').commission;
		});
    }

    processMarkets(): void {
       
	}

	getTradeCount(symbol: string): any {
		if (!this.heroStats || !this.heroStats.size) return '';
		if (this.heroStats.containsKey(symbol)) {
			return this.heroStats.getValue(symbol).tradeCount;
		} else {
			return '';
		}
	}

	getCommission(symbol: string): number {
		if (!this.heroStats || !this.heroStats.size) return 0;
		if (this.heroStats.containsKey(symbol)) {
			return this.heroStats.getValue(symbol).commission;
		} else {
			return 0;
		}
	}

	setInterval(interval: string): void {
		this.contextService.setInterval(interval);
	}

	changeSort(sortProp: string): void {

        if (sortProp && this.sortProperty == sortProp) {
            this.sortAscending = !this.sortAscending;
        } else if(sortProp){
            this.sortAscending = true;
            this.sortProperty = sortProp;
        }
    }
}
