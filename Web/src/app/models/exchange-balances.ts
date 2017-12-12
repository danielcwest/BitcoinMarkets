
import { CurrencyBalances } from './balance';

export interface ExchangeBalances {
	[exchange: string]: CurrencyBalances;
}
