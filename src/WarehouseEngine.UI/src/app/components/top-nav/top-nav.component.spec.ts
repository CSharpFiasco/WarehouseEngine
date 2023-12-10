import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopNavComponent } from './top-nav.component';
import { LayoutFacade } from 'src/app/store/layout/facade';
import { ThemeService } from 'src/app/services/theme/theme.service';

describe('TopNavComponent', () => {
  let component: TopNavComponent;
  let fixture: ComponentFixture<TopNavComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: LayoutFacade, useValue: {} },
        { provide: ThemeService, useValue: {} },
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
