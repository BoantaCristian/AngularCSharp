import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishDailogOrderComponent } from './finish-dailog-order.component';

describe('FinishDailogOrderComponent', () => {
  let component: FinishDailogOrderComponent;
  let fixture: ComponentFixture<FinishDailogOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FinishDailogOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FinishDailogOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
