import { TestBed, inject } from '@angular/core/testing';

import { BittrexService } from './bittrex.service';

describe('BittrexService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BittrexService]
    });
  });

  it('should be created', inject([BittrexService], (service: BittrexService) => {
    expect(service).toBeTruthy();
  }));
});
