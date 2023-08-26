import { Injectable } from "@angular/core";
import { StyleManagerService } from "../style-manager/style-manager.service";
import { Theme } from "src/app/types/default-theme";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class ThemeService {
  private readonly defaultTheme: Theme = 'deeppurple-amber';
  public readonly currentTheme$ = new BehaviorSubject<Theme>(this.defaultTheme);

  constructor(
    private readonly styleManager: StyleManagerService
  ) {}

  setTheme(themeToSet: Theme) {
    this.currentTheme$.next(themeToSet);
    this.styleManager.setStyle(
      "theme",
      `${themeToSet}.css`
    );
  }
}
