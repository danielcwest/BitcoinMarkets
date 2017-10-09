import { BtcPricePipe } from './btc-price.pipe';

describe('BtcPricePipe', () => {
  it('create an instance', () => {
    const pipe = new BtcPricePipe();
    expect(pipe).toBeTruthy();
  });
});
