import { Component, inject } from '@angular/core';
import { ThemeService } from './services/theme/theme.service';
import { CommonModule } from '@angular/common';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoginComponent } from './pages/login/login.component';
import { RouterOutlet } from '@angular/router';
import { AuthFacade } from './store/auth/auth.facade';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [CommonModule, TopNavComponent, SidenavComponent, LoginComponent, MatProgressSpinnerModule, RouterOutlet]
})
export class AppComponent {
  private readonly authFacade: AuthFacade = inject(AuthFacade);
  protected readonly loginStatus$ = this.authFacade.loginStatus$;

  constructor(themeService: ThemeService) {
    themeService.setTheme('indigo-pink');
  }
}
