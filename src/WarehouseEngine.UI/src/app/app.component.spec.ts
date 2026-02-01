import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { ThemeService } from './services/theme/theme.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Component, signal } from '@angular/core';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { LoginComponent } from './pages/login/login.component';
import { provideRouter } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { provideLocationMocks } from '@angular/common/testing';
import { routes } from './app.routing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthStore } from './store/auth/auth.store';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

@Component({
  selector: 'app-top-nav, mat-spinner',
  standalone: true,
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
  let themeServiceSpy: jasmine.SpyObj<ThemeService>;

  const mockAuthStore = {
    loginStatus: signal('logged out' as const),
    isLoggedIn: signal(false),
    isLoggingIn: signal(false),
    authState: signal({ type: 'logged out' as const }),
    login: jasmine.createSpy('login'),
    logout: jasmine.createSpy('logout'),
  };

  let fixtureNativeElement: HTMLElement;
  let harness: RouterTestingHarness;

  beforeEach(async () => {
    themeServiceSpy = jasmine.createSpyObj<ThemeService>('ThemeService', ['setTheme']);

    sessionStorage.clear();

    await TestBed.configureTestingModule({
    imports: [NoopAnimationsModule, AppComponent],
    providers: [
        { provide: ThemeService, useValue: themeServiceSpy },
        { provide: AuthStore, useValue: mockAuthStore },
        provideRouter(routes),
        provideLocationMocks(),
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting()
    ]
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

    fixture = TestBed.createComponent(AppComponent);
    harness = await RouterTestingHarness.create();
    fixtureNativeElement = fixture.nativeElement;

    app = fixture.componentInstance;

    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(app).toBeTruthy();
  });

  describe('Given we are not logged in', () => {
    beforeEach(async () => {
      await harness.navigateByUrl('/');
    });

    it('should render login by default', () => {
      const loginEl = fixtureNativeElement.querySelector('app-login');
      expect(loginEl).not.toBeNull();
    });
  });
});
