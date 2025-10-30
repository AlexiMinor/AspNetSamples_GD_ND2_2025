import { TestBed } from '@angular/core/testing';
import { CanDeactivateFn } from '@angular/router';

import { testCanDeactivateGuard } from './test-can-deactivate-guard';

describe('testCanDeactivateGuard', () => {
  const executeGuard: CanDeactivateFn<unknown> = (...guardParameters) => 
      TestBed.runInInjectionContext(() => testCanDeactivateGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
