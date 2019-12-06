import { TestBed } from '@angular/core/testing';

import { MountainZoneService } from './mountain-zone.service';

describe('MountainZoneService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MountainZoneService = TestBed.get(MountainZoneService);
    expect(service).toBeTruthy();
  });
});
