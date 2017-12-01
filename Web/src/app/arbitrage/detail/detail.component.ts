import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { ExchangeService } from '../../services/exchange.service';
import { ContextService } from '../../services/context.service';

import { Order } from '../../models/order';
import { Asset } from '../../models/asset';
import { AppContext } from '../../models/app-context';
import { ArbitragePair } from '../../models/arbitrage-pair';
import { OrderBook } from '../../models/order-book';
import { OrderBookEntry } from '../../models/order-book-entry';
import { ArbitrageService } from '../../services/arbitrage.service';
import { OrderService } from '../../services/order.service';
import { CoinMarketCapService } from '../../services/coinmarketcap.service';

@Component({
    selector: 'app-detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit {

	pair: ArbitragePair;

	orders: Order[] = [];

    context: AppContext;
	isLoading: boolean;

    constructor(
        private contextService: ContextService,
        private route: ActivatedRoute,
        private router: Router,
        private arbitrageService: ArbitrageService,
		private orderService: OrderService,
		private coincap: CoinMarketCapService
    ) { }

	ngOnInit() {
		let pairId = Number(this.route.snapshot.paramMap.get('pairId'));

		this.contextService.context$.subscribe(context => {
			this.context = context;
			this.refreshPair();
		});
    }

	refreshPair(): void {
		this.isLoading = true;
		let pairId = Number(this.route.snapshot.paramMap.get('pairId'));
		this.arbitrageService.getArbitragePair(pairId).then(pair => {
			this.pair = pair;
			this.isLoading = false;
		});


	}

	save(): void {
		this.isLoading = true;
		this.arbitrageService.saveArbitragePair(this.pair).then(result => {
			this.refreshPair(); 
		});
	}

}
