import { Injectable } from '@angular/core';
import { StyleManagerService } from '../style-manager/style-manager.service';
import type { Theme } from 'src/app/types/default-theme';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly defaultTheme: Theme = 'indigo-pink';
  private readonly currentThemeSubject = new BehaviorSubject<Theme>(this.defaultTheme);
  public readonly currentTheme$ = this.currentThemeSubject.asObservable();

  constructor(private readonly styleManager: StyleManagerService) {}

  setTheme(themeToSet: Theme) {
    this.currentThemeSubject.next(themeToSet);
    this.styleManager.setStyle('theme', `${themeToSet}.css`);
  }
}
