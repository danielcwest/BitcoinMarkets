
import { IExchangeMarket } from './exchange-market';

export class ArbitrageMarket {

    symbol: string;
    quoteCurrency: string;
    baseCurrency: string;

    baseVolume: number;
    arbVolume: number;

    baseLast: number;
    arbLast: number;

    baseBid: number;
    arbBid: number;

    baseAsk: number;
    arbAsk: number;

    spread: number;
    spreadAbs: number;

    spreadExact: number;
    basePriceExact: number;
    arbPriceExact: number;

    baseLink: string;
    arbLink: string;

    constructor(base: IExchangeMarket, arb: IExchangeMarket) {

        //Should be the same for each
        this.quoteCurrency = base.quoteCurrency;
        this.baseCurrency = base.baseCurrency;
        this.symbol = base.symbol;

        this.baseLink = base.link;
        this.arbLink = arb.link;

        this.baseVolume = base.volume;
        this.arbVolume = arb.volume;

        this.baseLast = base.last;
        this.arbLast = arb.last;
        this.baseAsk = base.ask;
        this.arbAsk = arb.ask;
        this.baseBid = base.bid;
        this.arbBid = arb.bid;

        this.spread = (base.last - arb.last) / base.last;
        this.spreadAbs = Math.abs(this.spread);

        //If Bases's price is lower, you want to buy on Base so you are looking for the lowest ask
        if (this.spread < 0) {
            this.spreadExact = Math.abs((base.ask - arb.bid) / base.ask);
            this.basePriceExact = base.ask;
            this.arbPriceExact = arb.bid;
        }
        //if Bittrex has a higher price, use Bittrex's highest bid and Arb's lowest ask
        else if (this.spread > 0) {
            this.spreadExact = Math.abs((base.bid - arb.ask) / base.bid);
            this.basePriceExact = base.bid;
            this.arbPriceExact = arb.ask;
        } else {
            this.spreadExact = 0;
        }

    }

}