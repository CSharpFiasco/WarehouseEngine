import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavComponent } from './sidenav.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideMockStore } from '@ngrx/store/testing';
import type { WarehouseEngineStore } from 'src/app/store/initial-state';
import { LayoutFacade } from 'src/app/store/layout/facade';
import { AuthFacade } from 'src/app/store/auth/auth.facade';
import { provideRouter } from '@angular/router';
import { routes } from 'src/app/app.routing';

describe('SidenavComponent', () => {
  let component: SidenavComponent;
  let fixture: ComponentFixture<SidenavComponent>;

  const initialState: WarehouseEngineStore = {
    auth: {
      type: 'logged out'
    },
    navigation: {
      isSideNavOpen: false
    }
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LayoutFacade,
        AuthFacade,
        provideRouter(routes),
        provideMockStore({ initialState }),
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
