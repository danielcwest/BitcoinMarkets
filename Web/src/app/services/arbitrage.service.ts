import { Injectable } from '@angular/core';

import { ArbitrageMarket } from '../models/arbitrage-market';
import { IExchangeMarket } from '../models/exchange-market';
import { ContextService } from './context.service';
import { ExchangeService } from './exchange.service';
import { BittrexService } from './bittrex.service';
import { AppContext } from '../models/app-context';
import { OrderBook } from '../models/order-book';
import { OrderBookEntry } from '../models/order-book-entry';

import * as Collections from 'typescript-collections';


@Injectable()
export class ArbitrageService {

    context: AppContext;

    exchanges: string[] = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];

    //TODO: make dynamic
    exchangeFees: { [exchange: string]: number } = { 'Bittrex': .0025, 'Hitbtc': .0010, 'Poloniex': .0025, 'Binance': .0010 };

    arbitrageMarkets: Collections.Dictionary<string, ArbitrageMarket>;

    exchangeMarketSymbols: Collections.Dictionary<string, Collections.Dictionary<string, boolean>>;

    baseExchangeMarkets: Collections.Dictionary<string, IExchangeMarket>;
    arbExchangeMarkets: Collections.Dictionary<string, IExchangeMarket>;

    baseExchangeMarket: IExchangeMarket;
    arbExchangeMarket: IExchangeMarket;

    baseOrderBook: OrderBook;
    arbOrderBook: OrderBook;

    constructor(private contextService: ContextService, private exchangeService: ExchangeService) {

        this.contextService.context$.subscribe(context => {
            this.context = context;
            this.refreshMarkets();
        });
    }



    refreshMarkets(): Promise<ArbitrageMarket[]> {
        return new Promise((resolve, reject) => {
            let promises = [];

            promises.push(this.exchangeService.getMarketSummaries(this.context.selectedBaseExchange));
            promises.push(this.exchangeService.getMarketSummaries(this.context.selectedArbExchange));
            Promise.all(promises).then(response => {
                let baseExchangeMarkets = response[0];
                let arbExchangeMarkets = response[1];

                let dic = new Collections.Dictionary<string, ArbitrageMarket>();
                baseExchangeMarkets.forEach(market => {
                    if (arbExchangeMarkets.containsKey(market) && arbExchangeMarkets.getValue(market).volume > 10) {
                        dic.setValue(market, new ArbitrageMarket(baseExchangeMarkets.getValue(market), arbExchangeMarkets.getValue(market)));
                    }
                });
                this.arbitrageMarkets = dic;
                resolve(this.arbitrageMarkets.values());
            });
        });
    }

    refreshSymbols(): Promise<boolean> {
        return new Promise((resolve, reject) => {
            let promises = [];

            this.exchanges.forEach(exchange => promises.push(this.exchangeService.getSymbols(exchange)))

            Promise.all(promises).then(response => {
                var i = 0;
                this.exchanges.forEach(exchange => {
                    this.exchangeMarketSymbols.setValue(exchange, response[i++]);
                });
                resolve(true);
            });
        });
    }

    getArbitrageMarket(symbol: string): ArbitrageMarket {
        return this.arbitrageMarkets.getValue(symbol);
    }
}
