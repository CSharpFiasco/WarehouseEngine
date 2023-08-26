import { Component } from '@angular/core';
import { ThemeService } from 'src/app/services/theme/theme.service';
import { NavigationFacade } from 'src/app/store/sidenav/facade';
import { Theme } from 'src/app/types/default-theme';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent {
  protected readonly currentTheme$ = this.themeService.currentTheme$;
  constructor(private readonly navigationFacade: NavigationFacade,
    private readonly themeService: ThemeService
    ){}

  protected handleClick = () => {
    this.navigationFacade.toggleSideNav();
  };

  updateTheme(theme: Theme){
    this.themeService.setTheme(theme);
  }
}
