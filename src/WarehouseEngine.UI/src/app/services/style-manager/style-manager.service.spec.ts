import { TestBed } from '@angular/core/testing';

import { StyleManagerService } from './style-manager.service';
import { beforeEach, describe, expect, it } from 'vitest';

describe('StyleManagerService', () => {
  let service: StyleManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StyleManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
