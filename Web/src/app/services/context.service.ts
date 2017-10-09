import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';

import { AppContext } from '../models/app-context';

@Injectable()
export class ContextService {


    //dashboard/market summary
    intervals: string[] = ['1h', '24h', '7d'];

    private contextSource = new BehaviorSubject<AppContext>({ interval: '24h', assetCount: 250, selectedBaseExchange: 'Bittrex', selectedArbExchange: 'Hitbtc' });
    context$ = this.contextSource.asObservable();
    context: AppContext = { interval: '24h', assetCount: 250, selectedBaseExchange: 'Bittrex', selectedArbExchange: 'Hitbtc' };

    constructor() { }

    setInterval(interval: string): void {
        if (this.intervals.indexOf(interval) != -1) {
            this.context.interval = interval;
            this.contextSource.next(this.context);
        }
    }

    setAssetCount(count: number): void {
        if (count && count > 0) {
            this.context.assetCount = count;
            this.contextSource.next(this.context);
        }
    }

    setBaseExchange(exchange: string): void {
        this.context.selectedBaseExchange = exchange;
        this.contextSource.next(this.context);
    }

    setArbExchange(exchange: string): void {
        this.context.selectedArbExchange = exchange;
        this.contextSource.next(this.context);
    }

}
