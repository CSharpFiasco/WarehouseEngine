import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopNavComponent } from './top-nav.component';
import { LayoutFacade } from 'src/app/store/layout/facade';
import { LoginService } from 'src/app/services/login/login.service';
import { ThemeService } from 'src/app/services/theme/theme.service';

class FacadeStub {}

class ThemeServiceStub {}

describe('TopNavComponent', () => {
  let component: TopNavComponent;
  let fixture: ComponentFixture<TopNavComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: LayoutFacade, useClass: FacadeStub },
        { provide: ThemeService, useClass: ThemeServiceStub },
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
