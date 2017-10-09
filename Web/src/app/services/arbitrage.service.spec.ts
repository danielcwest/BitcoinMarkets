import { TestBed, inject } from '@angular/core/testing';

import { ArbitrageService } from './arbitrage.service';

describe('ArbitrageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ArbitrageService]
    });
  });

  it('should be created', inject([ArbitrageService], (service: ArbitrageService) => {
    expect(service).toBeTruthy();
  }));
});
