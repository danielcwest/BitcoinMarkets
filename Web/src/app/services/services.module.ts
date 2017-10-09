import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoinMarketCapService } from './coinmarketcap.service';
import { ConfigService } from './config.service';
import { ContextService } from './context.service';
import { BittrexService } from './bittrex.service';
import { ExchangeService } from './exchange.service';
import { ArbitrageService } from './arbitrage.service';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [],
    providers: [CoinMarketCapService, ConfigService, ContextService, BittrexService, ExchangeService, ArbitrageService]
})
export class ServicesModule { }
