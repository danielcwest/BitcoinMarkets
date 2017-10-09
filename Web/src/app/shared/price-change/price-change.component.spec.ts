import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PriceChangeComponent } from './price-change.component';

describe('PriceChangeComponent', () => {
  let component: PriceChangeComponent;
  let fixture: ComponentFixture<PriceChangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PriceChangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PriceChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
