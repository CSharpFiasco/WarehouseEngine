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
    toggleSideNav: jasmine.createSpy('toggleSideNav'),
    openSideNav: jasmine.createSpy('openSideNav'),
    closeSideNav: jasmine.createSpy('closeSideNav'),
  };

  const mockAuthStore = {
    loginStatus: signal('logged out'),
    isLoggedIn: signal(false),
    isLoggingIn: signal(false),
    login: jasmine.createSpy('login'),
    logout: jasmine.createSpy('logout'),
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
