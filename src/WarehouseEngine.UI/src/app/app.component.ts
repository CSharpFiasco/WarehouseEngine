import { Component, inject } from '@angular/core';
import { ThemeService } from './services/theme/theme.service';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterOutlet } from '@angular/router';
import { AuthStore } from './store/auth/auth.store';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [TopNavComponent, SidenavComponent, MatProgressSpinnerModule, RouterOutlet]
})
export class AppComponent {
  private readonly authStore = inject(AuthStore);
  protected readonly loginStatus = this.authStore.loginStatus;

  constructor() {
    const themeService = inject(ThemeService);

    themeService.setTheme('indigo-pink');
  }
}
