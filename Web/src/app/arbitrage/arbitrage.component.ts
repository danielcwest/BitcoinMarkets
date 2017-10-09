import { Component, OnInit } from '@angular/core';

import { ArbitrageMarket } from '../models/arbitrage-market';
import { IExchangeMarket } from '../models/exchange-market';
import { ContextService } from '../services/context.service';
import { ExchangeService } from '../services/exchange.service';
import { ArbitrageService } from '../services/arbitrage.service';
import { AppContext } from '../models/app-context';

import * as Collections from 'typescript-collections';

@Component({
    selector: 'app-arbitrage',
    templateUrl: './arbitrage.component.html',
    styleUrls: ['./arbitrage.component.css']
})
export class ArbitrageComponent implements OnInit {

    exchanges: string[] = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];

    context: AppContext;

    arbitrageMarkets: ArbitrageMarket[] = [];

    baseExchangeMarkets: Collections.Dictionary<string, IExchangeMarket>;
    arbExchangeMarkets: Collections.Dictionary<string, IExchangeMarket>;

    sortProperty: string = 'symbol';
    sortAscending: boolean = true;

    isLoading: boolean = false;

    constructor(private arbitrageService: ArbitrageService, private exchangeService: ExchangeService, private contextService: ContextService) { }

    ngOnInit() {
        this.contextService.context$.subscribe(context => {
            this.context = context;
            this.refreshMarkets();
        });
    }

    refreshMarkets(): void {
        this.isLoading = true;
        this.arbitrageService.refreshMarkets().then(response => {
            this.arbitrageMarkets = response;
            this.isLoading = false;
        })

    }

    processMarkets(): void {
        let arbMkts: ArbitrageMarket[] = [];
        this.baseExchangeMarkets.forEach(market => {
            if (this.arbExchangeMarkets.containsKey(market) && this.arbExchangeMarkets.getValue(market).volume > 10) {
                arbMkts.push(new ArbitrageMarket(this.baseExchangeMarkets.getValue(market), this.arbExchangeMarkets.getValue(market)));
            }
        });
        this.arbitrageMarkets = arbMkts;
    }

    changeSort(sortProp: string): void {
        if (this.sortProperty == sortProp) {
            this.sortAscending = !this.sortAscending;
        } else {
            this.sortAscending = true;
            this.sortProperty = sortProp;
        }
    }

    onArbExchangeChange(exchange: string): void {
        if (exchange != this.context.selectedBaseExchange) {
            this.contextService.setArbExchange(exchange);
        } else {
            console.log('ERROR');
        }
    }

    onBaseExchangeChange(exchange: string): void {
        if (exchange != this.context.selectedArbExchange) {
            this.contextService.setBaseExchange(exchange);
        } else {
            console.log('ERROR');
        }
    }
}