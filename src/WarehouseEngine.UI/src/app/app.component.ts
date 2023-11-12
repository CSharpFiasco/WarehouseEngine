import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import type { LoginStatus } from './types/login-status.type';
import { ThemeService } from './services/theme/theme.service';
import { CommonModule } from '@angular/common';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoginComponent } from './pages/login/login.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [CommonModule, TopNavComponent, SidenavComponent, LoginComponent, MatProgressSpinnerModule, RouterOutlet],
  standalone: true,
})
export class AppComponent {
  protected loggedIn$: BehaviorSubject<LoginStatus | null> = new BehaviorSubject<LoginStatus | null>(null);

  constructor(themeService: ThemeService) {
    themeService.setTheme('indigo-pink');
  }
}
