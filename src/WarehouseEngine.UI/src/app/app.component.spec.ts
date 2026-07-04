import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { ThemeService } from './services/theme/theme.service';
import { Component } from '@angular/core';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { LoginComponent } from './pages/login/login.component';
import { provideRouter, Router } from '@angular/router';
import { provideLocationMocks } from '@angular/common/testing';
import { routes } from './app.routing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from './services/auth.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

@Component({
  selector: 'app-top-nav, mat-spinner',
  standalone: true,
  template: ''
})
class StubComponent { }
@Component({
  selector: 'app-sidenav',
  template: '<ng-content></ng-content>',
  standalone: true,
})
class SideNavStubComponent { }

@Component({
  selector: 'app-login',
  template: '<span>The login component exists!</span>',
  standalone: true,
})
class LoginMockComponent { }

describe('AppComponent', () => {
  let app: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let themeServiceSpy: { setTheme: ReturnType<typeof vi.fn> };

  let fixtureNativeElement: HTMLElement;

  beforeEach(async () => {
    themeServiceSpy = { setTheme: vi.fn() };

    sessionStorage.clear();

    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        { provide: ThemeService, useValue: themeServiceSpy },
        provideRouter(routes),
        provideLocationMocks(),
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
      ],
    })
      .overrideComponent(AppComponent, {
        add: {
          imports: [StubComponent, SideNavStubComponent, LoginMockComponent],
        },
        remove: {
          imports: [TopNavComponent, SidenavComponent, LoginComponent],
        },
      })
      .compileComponents();
  });

  function createApp(): void {
    fixture = TestBed.createComponent(AppComponent);
    fixtureNativeElement = fixture.nativeElement;
    app = fixture.componentInstance;
    fixture.detectChanges();
  }

  it('should create the app', () => {
    createApp();
    expect(app).toBeTruthy();
  });

  describe('Given we are not logged in', () => {
    beforeEach(async () => {
      createApp();
      const router = TestBed.inject(Router);
      await router.navigateByUrl('/');
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
    });

    it('should render login by default', () => {
      const loginEl = fixtureNativeElement.querySelector('app-login');
      expect(loginEl).not.toBeNull();
    });
  });

  describe('Given we are logged in', () => {
    beforeEach(async () => {
      TestBed.inject(AuthService).setJwtToken('test-jwt-token');
      createApp();
      const router = TestBed.inject(Router);
      await router.navigateByUrl('/');
      fixture.detectChanges();
      await fixture.whenStable();
      fixture.detectChanges();
    });

    it('should render the home component', () => {
      const homeEl = fixtureNativeElement.querySelector('app-home');
      expect(homeEl).not.toBeNull();
    });
  });
});
