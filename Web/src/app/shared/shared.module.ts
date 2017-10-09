import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommaSeparatedNumberPipe } from './comma-separated-number.pipe';
import { PriceChangeComponent } from './price-change/price-change.component';
import { NumeralPipe } from './numeral.pipe';
import { BtcPricePipe } from './btc-price.pipe';
import { OrderByPipe } from './order-by.pipe';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [CommaSeparatedNumberPipe, PriceChangeComponent, NumeralPipe, BtcPricePipe, OrderByPipe],
    exports: [CommaSeparatedNumberPipe, PriceChangeComponent, NumeralPipe, BtcPricePipe, OrderByPipe]
})
export class SharedModule { }
