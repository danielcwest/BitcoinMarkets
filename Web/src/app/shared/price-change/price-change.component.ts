import { Component, OnInit, Input } from '@angular/core';

import { ContextService } from '../../services/context.service';
import { AppContext } from '../../models/app-context';
import { PriceChange } from '../../models/price-change';

import * as numeral from 'numeral';

@Component({
    selector: 'app-price-change',
    templateUrl: './price-change.component.html',
    styleUrls: ['./price-change.component.css']
})
export class PriceChangeComponent implements OnInit {

    @Input() priceChange: PriceChange;
    @Input() shortHand: boolean = false;

    context: AppContext;

    isPos: boolean;
    isPercent: boolean = true;
    percentChange: number;
    netChange: number;
    sign: string;

    constructor(private contextService: ContextService) { }

    ngOnInit() {
        this.contextService.context$.subscribe(context => {
            this.context = context;

            if (context.interval == '1h') {
                this.isPos = this.priceChange.percent_change_1h > 0;
                this.sign = this.isPos ? '+' : '-';
                this.percentChange = Math.abs(this.priceChange.percent_change_1h);
                this.netChange = Math.abs(this.priceChange.market_cap_usd - this.priceChange.market_cap_usd_1h);
            } else if (context.interval == '24h') {
                this.isPos = this.priceChange.percent_change_24h > 0;
                this.sign = this.isPos ? '+' : '-';
                this.percentChange = Math.abs(this.priceChange.percent_change_24h);
                this.netChange = Math.abs(this.priceChange.market_cap_usd - this.priceChange.market_cap_usd_24h);
                console.log();

            } else if (context.interval == '7d') {
                this.isPos = this.priceChange.percent_change_7d > 0;
                this.sign = this.isPos ? '+' : '-';
                this.percentChange = Math.abs(this.priceChange.percent_change_7d);
                this.netChange = Math.abs(this.priceChange.market_cap_usd - this.priceChange.market_cap_usd_7d);
            }
        });
    }

    formatShorthand(val: number): string {
        return numeral(val).format('0.00a')
    }
}
