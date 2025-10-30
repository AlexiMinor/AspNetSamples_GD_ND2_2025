import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { userLoggedInGuard } from './user-logged-in-guard';

describe('userLoggedInGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
      TestBed.runInInjectionContext(() => userLoggedInGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
