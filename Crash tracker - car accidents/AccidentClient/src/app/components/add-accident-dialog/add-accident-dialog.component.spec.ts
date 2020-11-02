import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAccidentDialogComponent } from './add-accident-dialog.component';

describe('AddAccidentDialogComponent', () => {
  let component: AddAccidentDialogComponent;
  let fixture: ComponentFixture<AddAccidentDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAccidentDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAccidentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
