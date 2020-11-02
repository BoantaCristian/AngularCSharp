import { TestBed } from '@angular/core/testing';

import { MedicalOfficeService } from './medical-office.service';

describe('MedicalOfficeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MedicalOfficeService = TestBed.get(MedicalOfficeService);
    expect(service).toBeTruthy();
  });
});
