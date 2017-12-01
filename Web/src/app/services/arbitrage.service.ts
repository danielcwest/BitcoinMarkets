import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';

import { ArbitragePair } from '../models/arbitrage-pair';
import { Asset } from '../models/asset';
import { IExchangeMarket } from '../models/exchange-market';
import { ContextService } from './context.service';
import { CoinMarketCapService } from './coinmarketcap.service';
import { AppContext } from '../models/app-context';
import { OrderBook } from '../models/order-book';
import { OrderBookEntry } from '../models/order-book-entry';
import { HeroStat } from '../models/hero-stat';

import * as Collections from 'typescript-collections';


@Injectable()
export class ArbitrageService {

    context: AppContext;

	arbitragePairs: Collections.Dictionary<number, ArbitragePair>;

	constructor(private http: Http, private contextService: ContextService, private coincap: CoinMarketCapService) {

        this.contextService.context$.subscribe(context => {
            this.context = context;
        });
    }

	getArbitragePairs(): Promise<ArbitragePair[]> {
		return this.http.get(`/api/arbitrage/get`).toPromise().then(response => {
			let result = response.json() as ArbitragePair[];

			const dic = new Collections.Dictionary<number, ArbitragePair>();
			result.forEach(pair => {
					dic.setValue(pair.id, pair);				
			});
			this.arbitragePairs = dic;

			return result;
		}).catch(this.handleError);
	}

	getArbitragePair(pairId: number): Promise<ArbitragePair> {
		return this.http.get(`/api/arbitrage/getpair?id=${pairId}`).toPromise().then(response => {
			let result = response.json() as ArbitragePair;

			this.arbitragePairs.setValue(result.id, result);

			return result;
		}).catch(this.handleError);	}

	saveArbitragePair(pair: ArbitragePair): Promise<boolean> {
		return this.http.post(`/api/arbitrage/save`, pair).toPromise().then(result => true);
	}

	getStats(): Promise<Collections.Dictionary<string, HeroStat>> {
		return this.http.get(`/api/arbitrage/getherostats?interval=${this.context.interval}`).toPromise().then(response => {
			let result = response.json() as HeroStat[];
			const dic = new Collections.Dictionary<string, HeroStat>();
			result.forEach(stat => {
				dic.setValue(stat.symbol, stat);
			});
			return dic;
		}).catch(this.handleError);
	}

	getHeroStats(): Promise<Collections.Dictionary<string, HeroStat>> {

		var baseTickers = new Collections.Dictionary<string, Asset>();
		return this.coincap.getBaseTickers().then(tickers => {
			tickers.forEach(t => {
				if (t.symbol == 'BTC' || t.symbol == 'ETH')
					baseTickers.setValue(t.symbol, t);
			});

			var total = 0;
			var trades = 0;
			return this.getStats().then(stats => {
				stats.forEach(s => {
					let stat = stats.getValue(s);

					if (stat.symbol.endsWith('BTC'))
						stat.commission = stat.commission * baseTickers.getValue('BTC').price_usd;
					else if (stat.symbol.endsWith('ETH'))
						stat.commission = stat.commission * baseTickers.getValue('ETH').price_usd;

					total += stat.commission;
					trades += stat.tradeCount;
				});

				var totalStat = <HeroStat>{ symbol: 'TOTAL', commission: total, tradeCount: trades };
				stats.setValue(totalStat.symbol, totalStat);
				return stats;
			});
		});
	}

	private handleError(error: any): Promise<any> {
		console.log(error);
		//console.error('An error occurred', error);
		return Promise.reject(error.message || error);
	}
}
