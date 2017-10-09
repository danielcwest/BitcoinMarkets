import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { ContextService } from './context.service';
import { TickerItem } from '../models/ticker-item';
import { TickerGlobal } from '../models/ticker-global';
import { Asset } from '../models/asset';
import { AppContext } from '../models/app-context';

@Injectable()
export class CoinMarketCapService {

    context: AppContext;

    constructor(private http: Http, private contextService: ContextService) {
        this.contextService.context$.subscribe(context => this.context = context);
    }

    getAssets(): Promise<Asset[]> {
        return this.http.get(`https://api.coinmarketcap.com/v1/ticker/?limit=250`).toPromise().then(response => {
            let result = response.json() as TickerItem[];
            let assets = result.map(i => new Asset(i));
            return assets;
        }).catch(this.handleError);
    }

    getGlobal(): Promise<TickerGlobal> {
        return this.http.get(`https://api.coinmarketcap.com/v1/global/`).toPromise().then(response => {
            let result = response.json() as TickerGlobal;
            return result;
        }).catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.log(error);
        //console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}
