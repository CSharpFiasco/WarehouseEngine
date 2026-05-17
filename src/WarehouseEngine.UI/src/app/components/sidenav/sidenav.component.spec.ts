import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavComponent } from './sidenav.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { signal } from '@angular/core';
import { LayoutStore } from '../../store/layout/layout.store';
import { AuthStore } from '../../store/auth/auth.store';
import { routes } from '../../app.routing';

describe('SidenavComponent', () => {
  let component: SidenavComponent;
  let fixture: ComponentFixture<SidenavComponent>;

  const mockLayoutStore = {
    sideNavOpen: signal(false),
    isSideNavOpen: signal(false),
    toggleSideNav: vi.fn(),
    openSideNav: vi.fn(),
    closeSideNav: vi.fn(),
  };

  const mockAuthStore = {
    loginStatus: signal('logged out'),
    isLoggedIn: signal(false),
    isLoggingIn: signal(false),
    login: vi.fn(),
    logout: vi.fn(),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: LayoutStore, useValue: mockLayoutStore },
        { provide: AuthStore, useValue: mockAuthStore },
        provideRouter(routes),
      ],
      imports: [NoopAnimationsModule, SidenavComponent],
    });
    fixture = TestBed.createComponent(SidenavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
