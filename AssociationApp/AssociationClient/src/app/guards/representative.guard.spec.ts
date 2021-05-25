import { TestBed, async, inject } from '@angular/core/testing';

import { RepresentativeGuard } from './representative.guard';

describe('RepresentativeGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RepresentativeGuard]
    });
  });

  it('should ...', inject([RepresentativeGuard], (guard: RepresentativeGuard) => {
    expect(guard).toBeTruthy();
  }));
});
