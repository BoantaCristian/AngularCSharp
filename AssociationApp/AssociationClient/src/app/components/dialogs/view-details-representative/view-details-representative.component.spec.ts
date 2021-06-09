import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDetailsRepresentativeComponent } from './view-details-representative.component';

describe('ViewDetailsRepresentativeComponent', () => {
  let component: ViewDetailsRepresentativeComponent;
  let fixture: ComponentFixture<ViewDetailsRepresentativeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewDetailsRepresentativeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDetailsRepresentativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
