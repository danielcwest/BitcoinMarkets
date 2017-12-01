import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommaSeparatedNumberPipe } from './comma-separated-number.pipe';
import { NumeralPipe } from './numeral.pipe';
import { BtcPricePipe } from './btc-price.pipe';
import { OrderByPipe } from './order-by.pipe';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [CommaSeparatedNumberPipe, NumeralPipe, BtcPricePipe, OrderByPipe],
    exports: [CommaSeparatedNumberPipe, NumeralPipe, BtcPricePipe, OrderByPipe]
})
export class SharedModule { }
