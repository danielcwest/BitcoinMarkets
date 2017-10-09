
export interface IExchangeMarket {
    exchange: string;
    symbol: string;
    volume: number;
    timestamp: Date;
    last: number;
    bid: number;
    ask: number;
    quoteCurrency: string;
    baseCurrency: string;
    link: string;
}
