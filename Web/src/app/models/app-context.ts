
import * as Collections from 'typescript-collections';
import { HeroStat } from '../models/hero-stat';

export class AppContext {
    interval: string; //1h, 24h, 7d
	assetCount: number;

	constructor() {
		this.interval = '1h';
	}
}
