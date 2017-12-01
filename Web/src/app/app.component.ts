import { Component, OnInit } from '@angular/core';

import { ConfigService } from './services/config.service';
import { ContextService } from './services/context.service';
import { AppContext } from './models/app-context';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    context: AppContext;

    constructor(private configService: ConfigService, private contextService: ContextService) { }

    ngOnInit() {
        this.contextService.context$.subscribe(context => this.context = context);
	}

	refreshPairs(): void {
		this.contextService.notify();
	}

	setInterval(interval: string): void {
		this.contextService.setInterval(interval);
	}
}
