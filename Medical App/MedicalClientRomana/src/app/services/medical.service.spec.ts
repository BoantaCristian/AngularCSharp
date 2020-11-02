import { TestBed } from '@angular/core/testing';

import { MedicalService } from './medical.service';

describe('MedicalService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MedicalService = TestBed.get(MedicalService);
    expect(service).toBeTruthy();
  });
});
