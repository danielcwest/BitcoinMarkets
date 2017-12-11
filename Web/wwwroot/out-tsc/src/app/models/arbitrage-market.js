"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ArbitrageMarket = /** @class */ (function () {
    function ArbitrageMarket(base, arb) {
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
        else if (this.spread > 0) {
            this.spreadExact = Math.abs((base.bid - arb.ask) / base.bid);
            this.basePriceExact = base.bid;
            this.arbPriceExact = arb.ask;
        }
        else {
            this.spreadExact = 0;
        }
    }
    return ArbitrageMarket;
}());
exports.ArbitrageMarket = ArbitrageMarket;
//# sourceMappingURL=arbitrage-market.js.map