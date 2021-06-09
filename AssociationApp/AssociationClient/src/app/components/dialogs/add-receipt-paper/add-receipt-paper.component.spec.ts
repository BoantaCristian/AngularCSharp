import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddReceiptPaperComponent } from './add-receipt-paper.component';

describe('AddReceiptPaperComponent', () => {
  let component: AddReceiptPaperComponent;
  let fixture: ComponentFixture<AddReceiptPaperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddReceiptPaperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddReceiptPaperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
