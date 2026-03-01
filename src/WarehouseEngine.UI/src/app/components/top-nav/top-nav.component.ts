import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { LayoutStore } from '../../store/layout/layout.store';
import { ThemeService } from '../../services/theme/theme.service';
import type { Theme } from '../../types/default-theme';

@Component({
    selector: 'app-top-nav',
    templateUrl: './top-nav.component.html',
    styleUrls: ['./top-nav.component.scss'],
    imports: [CommonModule, MatIconModule, MatToolbarModule, MatMenuModule, MatButtonModule]
})
export class TopNavComponent {
  private readonly layoutStore = inject(LayoutStore);
  private readonly themeService: ThemeService = inject(ThemeService);

  protected readonly currentTheme$ = this.themeService.currentTheme$;

  protected toggleSideNav = () => {
    this.layoutStore.toggleSideNav();
  };

  protected updateTheme(theme: Theme) {
    this.themeService.setTheme(theme);
  }
}
