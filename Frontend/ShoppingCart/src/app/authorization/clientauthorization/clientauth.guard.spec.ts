import { TestBed } from '@angular/core/testing';

import { ClientauthGuard } from './clientauth.guard';

describe('ClientauthGuard', () => {
  let guard: ClientauthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ClientauthGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
