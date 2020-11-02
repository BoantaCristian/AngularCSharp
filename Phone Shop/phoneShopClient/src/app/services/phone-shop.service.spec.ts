import { TestBed } from '@angular/core/testing';

import { PhoneShopService } from './phone-shop.service';

describe('PhoneShopService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PhoneShopService = TestBed.get(PhoneShopService);
    expect(service).toBeTruthy();
  });
});
