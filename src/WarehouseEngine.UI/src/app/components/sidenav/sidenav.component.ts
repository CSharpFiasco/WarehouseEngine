import { Component } from '@angular/core';
import { NavigationFacade } from 'src/app/store/sidenav/facade';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent {
  protected sideNavOpen$ = this.navigationFacade.sideNavOpen$;
  constructor(private readonly navigationFacade: NavigationFacade){}
}
