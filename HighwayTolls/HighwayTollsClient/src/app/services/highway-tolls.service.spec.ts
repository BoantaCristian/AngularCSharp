import { TestBed } from '@angular/core/testing';

import { HighwayTollsService } from './highway-tolls.service';

describe('HighwayTollsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HighwayTollsService = TestBed.get(HighwayTollsService);
    expect(service).toBeTruthy();
  });
});
