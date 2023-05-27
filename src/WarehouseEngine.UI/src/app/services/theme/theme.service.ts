import { Injectable } from "@angular/core";
import { StyleManagerService } from "../style-manager/style-manager.service";
import { DefaultTheme } from "src/app/types/default-theme";

@Injectable()
export class ThemeService {
  private theme: DefaultTheme = 'deeppurple-amber';

  constructor(
    private styleManager: StyleManagerService
  ) {}

  // getThemeOptions(): Observable<Array<Option>> {
  //   return this.http.get<Array<Option>>("assets/options.json");
  // }

  setTheme(themeToSet: DefaultTheme) {
    this.styleManager.setStyle(
      "theme",
      `${themeToSet}.css`
    );
  }
}
