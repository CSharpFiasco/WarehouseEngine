import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ThemeService } from 'src/app/services/theme/theme.service';
import { LayoutFacade } from 'src/app/store/layout/facade';
import type { Theme } from 'src/app/types/default-theme';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss'],
  imports: [CommonModule, MatIconModule, MatToolbarModule, MatMenuModule, MatButtonModule],
  standalone: true,
})
export class TopNavComponent {
  protected readonly currentTheme$ = this.themeService.currentTheme$;
  constructor(
    private readonly navigationFacade: LayoutFacade,
    private readonly themeService: ThemeService
  ) {}

  protected toggleSideNav = () => {
    this.navigationFacade.toggleSideNav();
  };

  protected updateTheme(theme: Theme) {
    this.themeService.setTheme(theme);
  }
}
