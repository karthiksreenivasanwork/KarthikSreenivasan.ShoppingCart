import { TestBed } from '@angular/core/testing';

import { ComponentcommunicationService } from './componentcommunication.service';

describe('ComponentcommunicationService', () => {
  let service: ComponentcommunicationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ComponentcommunicationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
