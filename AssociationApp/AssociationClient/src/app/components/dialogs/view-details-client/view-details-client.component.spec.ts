import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDetailsClientComponent } from './view-details-client.component';

describe('ViewDetailsClientComponent', () => {
  let component: ViewDetailsClientComponent;
  let fixture: ComponentFixture<ViewDetailsClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewDetailsClientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDetailsClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
