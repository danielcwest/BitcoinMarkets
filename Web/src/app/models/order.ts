

export interface Order {
	id: number;
	baseExchange: string;
	counterExchange: string;
	baseSymbol: string;
	counterSymbol: string;
	baseCurrency: string;
	marketCurrency: string;
	baseOrderUuid: string;
	counterOrderUuid: string;
	type: string;
	tradeThreshold: number;
	withdrawalThreshold: number;
	counterExchangeFee: number;
	counterBaseWithdrawalFee: number;
	counterMarketWithdrawalFee: number;
	baseQuantityFilled: number;
	counterQuantityFilled: number;
	askSpread: number;
	bidSpread: number;
	processId: number;
	commission: number;
}
