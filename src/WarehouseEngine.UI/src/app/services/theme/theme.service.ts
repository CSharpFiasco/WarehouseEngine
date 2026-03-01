import { Injectable, inject } from '@angular/core';
import { StyleManagerService } from '../style-manager/style-manager.service';
import { BehaviorSubject } from 'rxjs';
import type { Theme } from '../../types/default-theme';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly styleManager: StyleManagerService = inject(StyleManagerService);

  private readonly defaultTheme: Theme = 'indigo-pink';
  private readonly currentThemeSubject = new BehaviorSubject<Theme>(this.defaultTheme);
  public readonly currentTheme$ = this.currentThemeSubject.asObservable();

  setTheme(themeToSet: Theme) {
    this.currentThemeSubject.next(themeToSet);
    this.styleManager.setStyle('theme', `${themeToSet}.css`);
  }
}
