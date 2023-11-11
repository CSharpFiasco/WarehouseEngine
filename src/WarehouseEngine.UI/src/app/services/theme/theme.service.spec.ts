import { TestBed } from '@angular/core/testing';

import { ThemeService } from './theme.service';
import { StyleManagerService } from '../style-manager/style-manager.service';

describe('ThemeService', () => {
  let service: ThemeService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ThemeService, StyleManagerService],
    });
    TestBed.inject(StyleManagerService);
    service = TestBed.inject(ThemeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
