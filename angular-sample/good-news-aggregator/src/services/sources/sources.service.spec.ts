/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SourcesService } from './sources.service';

describe('Service: Sources', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SourcesService]
    });
  });

  it('should ...', inject([SourcesService], (service: SourcesService) => {
    expect(service).toBeTruthy();
  }));
});
