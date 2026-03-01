import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopNavComponent } from './top-nav.component';
import { signal } from '@angular/core';
import { LayoutStore } from '../../store/layout/layout.store';
import { ThemeService } from '../../services/theme/theme.service';

describe('TopNavComponent', () => {
  let component: TopNavComponent;
  let fixture: ComponentFixture<TopNavComponent>;

  const mockLayoutStore = {
    sideNavOpen: signal(false),
    isSideNavOpen: signal(false),
    toggleSideNav: jasmine.createSpy('toggleSideNav'),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: LayoutStore, useValue: mockLayoutStore },
        { provide: ThemeService, useValue: {} },
      ],
      imports: [TopNavComponent],
    });
    fixture = TestBed.createComponent(TopNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
