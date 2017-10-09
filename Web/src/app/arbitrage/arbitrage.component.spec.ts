import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArbitrageComponent } from './arbitrage.component';

describe('ArbitrageComponent', () => {
  let component: ArbitrageComponent;
  let fixture: ComponentFixture<ArbitrageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArbitrageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArbitrageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
