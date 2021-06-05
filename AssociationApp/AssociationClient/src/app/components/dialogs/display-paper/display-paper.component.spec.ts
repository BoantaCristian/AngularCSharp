import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplayPaperComponent } from './display-paper.component';

describe('DisplayPaperComponent', () => {
  let component: DisplayPaperComponent;
  let fixture: ComponentFixture<DisplayPaperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisplayPaperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DisplayPaperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
