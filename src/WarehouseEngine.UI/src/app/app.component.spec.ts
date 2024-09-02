import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { ThemeService } from './services/theme/theme.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Component } from '@angular/core';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { LoginComponent } from './pages/login/login.component';
import { provideRouter } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { provideLocationMocks } from '@angular/common/testing';
import { routes } from './app.routing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import type { WarehouseEngineStore } from './store/initial-state';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { AuthFacade } from './store/auth/auth.facade';
import { ReplaySubject } from 'rxjs';
import type { Action } from '@ngrx/store';
import * as AuthActions from './store/auth/auth.actions';
import { AuthEffects } from './store/auth/auth.effects';
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

  const initialState: WarehouseEngineStore = {
    auth: {
      type: 'logged out'
    },
    navigation: {
      isSideNavOpen: false
    }
  };
  let actions$: ReplaySubject<Action>;
  let effects: AuthEffects;

  let fixtureNativeElement: HTMLElement;
  let harness: RouterTestingHarness;

  beforeEach(async () => {
    themeServiceSpy = jasmine.createSpyObj<ThemeService>('ThemeService', ['setTheme']);
    actions$ = new ReplaySubject<Action>(1);

    sessionStorage.clear();

    await TestBed.configureTestingModule({
    imports: [NoopAnimationsModule, AppComponent],
    providers: [
        { provide: ThemeService, useValue: themeServiceSpy },
        provideRouter(routes),
        provideLocationMocks(),
        provideMockStore({ initialState }),
        provideMockActions(() => actions$),
        AuthEffects,
        AuthFacade,
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

    TestBed.inject(MockStore);
    effects = TestBed.inject(AuthEffects);

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

  describe('Given we are logged in', () => {
    beforeEach(() => {
      actions$.next(AuthActions.setJwtToken({
        jwtResponse: {
          type: 'Success',
          jwt: 'jwt'
        }
      }));

      fixture.detectChanges();
    });

    it('should render home', (done) => {
      effects.setJwtToken$.subscribe(async () => {
        fixture.detectChanges();
        await harness.navigateByUrl('/');
        fixture.detectChanges();

        await fixture.whenStable();

        const loginEl = fixtureNativeElement.querySelector('app-home');
        expect(loginEl).not.toBeNull();
        done();
      });
    });
  });
});
