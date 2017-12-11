import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';
import { HeroStat } from '../models/hero-stat';

import { AppContext } from '../models/app-context';

import * as Collections from 'typescript-collections';

@Injectable()
export class ContextService {


    //dashboard/market summary
    intervals: string[] = ['1h', '4h', '12h', '24h', '7d'];

	private contextSource = new BehaviorSubject<AppContext>(new AppContext());
    context$ = this.contextSource.asObservable();
	context: AppContext = new AppContext();

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

	notify(): void {
		this.contextSource.next(this.context);
	}
}
