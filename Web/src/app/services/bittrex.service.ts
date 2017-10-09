import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { ContextService } from './context.service';
import { IExchangeMarket } from '../models/exchange-market';
import { AppContext } from '../models/app-context';

import * as Collections from 'typescript-collections';

@Injectable()
export class BittrexService {

    context: AppContext;

    constructor(private http: Http, private contextService: ContextService) {
        this.contextService.context$.subscribe(context => this.context = context);
    }

    getMarketSummary(): Promise<Collections.Dictionary<string, IExchangeMarket>> {
        return this.http.get(`/api/bittrex/marketsummaries/`).toPromise().then(response => {
            let result = response.json() as IExchangeMarket[];
            let dic = new Collections.Dictionary<string, IExchangeMarket>();
            result.forEach(market => dic.setValue(market.symbol, market));
            return dic;
        });
    }

    private handleError(error: any): Promise<any> {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}


