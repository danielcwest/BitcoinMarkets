import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { ExchangeService } from '../../services/exchange.service';
import { ContextService } from '../../services/context.service';

import { AppContext } from '../../models/app-context';
import { IExchangeMarket } from '../../models/exchange-market';
import { OrderBook } from '../../models/order-book';
import { OrderBookEntry } from '../../models/order-book-entry';
import { ArbitrageService } from '../../services/arbitrage.service';

@Component({
    selector: 'app-detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit {

    context: AppContext;
    symbol: string;
    isLoading: boolean;

    baseExchangeMarket: IExchangeMarket;
    arbExchangeMarket: IExchangeMarket;

    baseOrderBook: OrderBook;
    arbOrderBook: OrderBook;

    exchanges: string[] = ['Bittrex', 'Hitbtc', 'Poloniex', 'Liqui', 'Livecoin', 'Tidex', 'Etherdelta', 'Bitz', 'Nova', 'Binance'];

    // Amount in base currency to measure accurate buy/sell price
    threshold = .25;

    baseCurrency: string;
    quoteCurrency: string;
    spreadLast: number;


    // Price at which I can Buy or Sell given the market depth
    baseExactBuy: number;
    arbExactSell: number;
    baseBuySpread: number;

    baseExactSell: number;
    arbExactBuy: number;
    baseSellSpread: number;

    baseExchangeFee: number;
    arbExchangeFee: number;

    constructor(
        private contextService: ContextService,
        private route: ActivatedRoute,
        private router: Router,
        private exchangeService: ExchangeService,
        private arbitrageService: ArbitrageService
    ) { }

    ngOnInit() {
        this.symbol = this.route.snapshot.paramMap.get('ticker');


        this.contextService.context$.subscribe(context => {
            this.context = context;
            this.baseExchangeFee = this.arbitrageService.exchangeFees[this.context.selectedBaseExchange];
            this.arbExchangeFee = this.arbitrageService.exchangeFees[this.context.selectedArbExchange];
            this.refreshMarkets();
        });
    }

    refreshMarkets(): Promise<boolean> {
        this.isLoading = true;
        return new Promise((resolve, reject) => {
            const promises = [];

            promises.push(this.exchangeService.getMarketSummary(this.context.selectedBaseExchange, this.symbol));
            promises.push(this.exchangeService.getMarketSummary(this.context.selectedArbExchange, this.symbol));

            promises.push(this.exchangeService.getOrderBook(this.context.selectedBaseExchange, this.symbol));
            promises.push(this.exchangeService.getOrderBook(this.context.selectedArbExchange, this.symbol));

            Promise.all(promises).then(response => {
                this.baseExchangeMarket = response[0];
                this.arbExchangeMarket = response[1];
                this.baseOrderBook = response[2];
                this.arbOrderBook = response[3];

                this.processMarkets();
                this.isLoading = false;
                resolve(true);
            });
        });
    }

    processMarkets(): void {
        // console.log(this.baseExchangeMarket);
        // console.log(this.arbExchangeMarket);
        // console.log(this.baseOrderBook);
        // console.log(this.arbOrderBook);

        this.spreadLast = Math.abs((this.baseExchangeMarket.last - this.arbExchangeMarket.last) / this.baseExchangeMarket.last);

        this.baseCurrency = this.baseExchangeMarket.baseCurrency;
        this.quoteCurrency = this.baseExchangeMarket.quoteCurrency;

        this.setThresholdPrices();
    }

    setThresholdPrices(): void {

        // TODO: Optimize below
        // Base Exchange
        this.baseOrderBook.asks.some(e => {
            if (e.sumBase >= this.threshold) {
                // console.log(e);
                this.baseExactBuy = e.price;
                return true;
            }
        });

        this.baseOrderBook.bids.some(e => {
            if (e.sumBase >= this.threshold) {
                // console.log(e);
                this.baseExactSell = e.price;
                return true;
            }
        });

        // Arb Exchange
        this.arbOrderBook.asks.some(e => {
            if (e.sumBase >= this.threshold) {
                // console.log(e);
                this.arbExactBuy = e.price;
                return true;
            }
        });

        this.arbOrderBook.bids.some(e => {
            if (e.sumBase >= this.threshold) {
                // console.log(e);
                this.arbExactSell = e.price;
                return true;
            }
        });

        this.baseBuySpread = Math.abs((this.baseExactBuy - this.arbExactSell) / this.baseExactBuy);
        this.baseSellSpread = Math.abs((this.baseExactSell - this.arbExactBuy) / this.baseExactSell);


    }

    onArbExchangeChange(exchange: string): void {
        if (exchange !== this.context.selectedBaseExchange) {
            this.contextService.setArbExchange(exchange);
        } else {
            console.log('ERROR');
        }
    }

    onBaseExchangeChange(exchange: string): void {
        if (exchange !== this.context.selectedArbExchange) {
            this.contextService.setBaseExchange(exchange);
        } else {
            console.log('ERROR');
        }
    }

    BuyBase(): void {

    }

    SellBase(): void {

    }

    BuyArb(): void {

    }

    SellArb(): void {

    }

}
