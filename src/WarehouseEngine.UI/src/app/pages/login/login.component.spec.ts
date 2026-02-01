import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { LoginService } from 'src/app/services/login/login.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AuthStore } from 'src/app/store/auth/auth.store';
import { provideRouter } from '@angular/router';
import { signal } from '@angular/core';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  const mockAuthStore = {
    loginStatus: signal('logged out'),
    isLoggedIn: signal(false),
    isLoggingIn: signal(false),
    login: jasmine.createSpy('login'),
    logout: jasmine.createSpy('logout'),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        { provide: LoginService, useValue: {} },
        { provide: AuthStore, useValue: mockAuthStore },
        provideRouter([]),
      ],
      imports: [NoopAnimationsModule, LoginComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
