
import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { ContextService } from './context.service';
import { IExchangeMarket } from '../models/exchange-market';
import { AppContext } from '../models/app-context';
import { OrderBook } from '../models/order-book';

import * as Collections from 'typescript-collections';

@Injectable()
export class ExchangeService {

    context: AppContext;

    constructor(private http: Http, private contextService: ContextService) {
        this.contextService.context$.subscribe(context => this.context = context);
    }

    getMarketSummaries(exchange: string): Promise<Collections.Dictionary<string, IExchangeMarket>> {
        return this.http.get(`/api/${exchange}/marketsummaries/`).toPromise().then(response => {
            let result = response.json() as IExchangeMarket[];
            let dic = new Collections.Dictionary<string, IExchangeMarket>();
            result.forEach(market => dic.setValue(market.symbol, market));
            return dic;
        });
    }

    getMarketSummary(exchange: string, symbol: string): Promise<IExchangeMarket> {
        return this.http.get(`/api/${exchange}/marketsummary?symbol=${symbol}`).toPromise().then(response => {
            let result = response.json() as IExchangeMarket;
            return result;
        });
    }

    getOrderBook(exchange: string, symbol: string): Promise<OrderBook> {
        return this.http.get(`/api/${exchange}/orderbook?symbol=${symbol}`).toPromise().then(response => {
            let result = response.json() as OrderBook;
            return result;
        });
    }

    getSymbols(exchange: string): Promise<string[]> {
        return this.http.get(`/api/${exchange}/symbols`).toPromise().then(response => {
            let result = response.json() as string[];
            return result;
        });
    }

    private handleError(error: any): Promise<any> {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}


