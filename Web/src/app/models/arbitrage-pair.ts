export interface ArbitragePair {
	id: number;
	baseExchange: string;
	counterExchange: string;
	symbol: string;
	status: string;
	type: string;
	baseSymbol: string;
	counterSymbol: string;
	baseCurrency: string;
	marketCurrency: string;
	lastRunUtc: Date;
	tradeThreshold: number;
	spreadThreshold: number;
	withdrawalThreshold: number;
	baseExchangeFee: number;
	counterExchangeFee: number;
	baseBaseWithdrawalFee: number;
	baseMarketWithdrawalFee: number;
	counterBaseWithdrawalFee: number;
	counterMarketWithdrawalFee: number;
	askSpread: number;
	bidSpread: number;
	marketSpread: number;
	decimalPlaces: number;
	askMultiplier: number;
	bidMultiplier: number;
	increment: number;
	tickSize: number;
}


