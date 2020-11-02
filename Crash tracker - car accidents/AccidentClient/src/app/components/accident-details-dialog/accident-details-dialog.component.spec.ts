import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccidentDetailsDialogComponent } from './accident-details-dialog.component';

describe('AccidentDetailsDialogComponent', () => {
  let component: AccidentDetailsDialogComponent;
  let fixture: ComponentFixture<AccidentDetailsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccidentDetailsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccidentDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
