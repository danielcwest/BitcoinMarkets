
import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { ContextService } from './context.service';
import { IExchangeMarket } from '../models/exchange-market';
import { AppContext } from '../models/app-context';
import { CurrencyBalance } from '../models/balance';
import { ExchangeBalances } from '../models/exchange-balances';

import * as Collections from 'typescript-collections';

@Injectable()
export class ExchangeService {

    context: AppContext;

    constructor(private http: Http, private contextService: ContextService) {
        this.contextService.context$.subscribe(context => this.context = context);
    }

	getBalances(): Promise<ExchangeBalances> {
		return this.http.get(`/api/exchange/GetBalances`).toPromise().then(response => {
			let result = response.json() as ExchangeBalances;
            return result;
        });
    }

    private handleError(error: any): Promise<any> {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}


