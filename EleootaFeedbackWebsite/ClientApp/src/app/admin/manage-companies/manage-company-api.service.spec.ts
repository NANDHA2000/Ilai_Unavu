import { TestBed } from '@angular/core/testing';

import { ManageCompanyApiService } from './manage-company-api.service';

describe('ManageCompanyApiService', () => {
  let service: ManageCompanyApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageCompanyApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
