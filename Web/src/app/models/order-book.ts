
import { OrderBookEntry } from './order-book-entry';

export interface OrderBook {
    symbol: string;
    bids: OrderBookEntry[];
    asks: OrderBookEntry[];
}