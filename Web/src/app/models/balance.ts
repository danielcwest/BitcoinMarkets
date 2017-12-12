
export interface CurrencyBalance {
	currency: string;
	available: number;
	held: number;
}

export interface CurrencyBalances {
	[currency: string]: CurrencyBalance;
}

