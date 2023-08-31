import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { ThemeService } from './services/theme/theme.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Component } from '@angular/core';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { LoginComponent } from './pages/login/login.component';
import { BehaviorSubject } from 'rxjs';
import { LoginStatus } from './types/login-status.type';

@Component({
  selector: 'app-top-nav, mat-spinner',
  standalone: true,
})
class StubComponent {

}
@Component({
  selector: 'app-sidenav',
  template: '<ng-content></ng-content>',
  standalone: true,
})
class SideNavStubComponent {

}

@Component({
  selector: 'app-login',
  template: '<span>The login component exists!</span>',
  standalone: true,
})
class LoginMockComponent {

}

describe('AppComponent', () => {
  let app: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let themeServiceSpy: Partial<ThemeService>;

  beforeEach(async () => {
    themeServiceSpy = {
      setTheme: jest.fn(),
    };
    await TestBed.configureTestingModule({
      providers: [
        { provide: ThemeService, useValue: themeServiceSpy }
      ],
      imports: [
        NoopAnimationsModule,
        AppComponent
      ],
    })
    .overrideComponent(AppComponent, {
      add: {
        imports: [
          StubComponent,
          SideNavStubComponent,
          LoginMockComponent
        ]

      },
      remove: {
        imports: [
          TopNavComponent,
          SidenavComponent,
          LoginComponent
        ]
      }
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    app = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(app).toBeTruthy();
  });

  describe('Given we are not logged in', () => {
    it('should render login by default', () => {
      const compiled = fixture.nativeElement as HTMLElement;
      expect(compiled.querySelector('span')?.textContent).toContain('The login component exists!');
    });
  });
});
