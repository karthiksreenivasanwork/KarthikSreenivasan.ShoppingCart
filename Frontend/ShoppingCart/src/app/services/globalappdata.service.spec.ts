import { TestBed } from '@angular/core/testing';

import { GlobalappdataService } from './globalappdata.service';

describe('GlobalappdataService', () => {
  let service: GlobalappdataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlobalappdataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
