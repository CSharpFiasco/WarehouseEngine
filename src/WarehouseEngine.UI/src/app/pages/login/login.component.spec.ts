import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { LoginService } from 'src/app/services/login/login.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AuthFacade } from 'src/app/store/auth/auth.facade';
import { provideMockStore } from '@ngrx/store/testing';
import type { WarehouseEngineStore } from 'src/app/store/initial-state';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  const initialState: WarehouseEngineStore = {
    auth: {
      type: 'logged out'
    },
    navigation: {
      isSideNavOpen: false
    }
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        { provide: LoginService, useValue: {} },
        AuthFacade,
        provideMockStore({ initialState }),
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
