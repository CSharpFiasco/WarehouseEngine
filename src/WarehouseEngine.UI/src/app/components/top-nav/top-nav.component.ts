import { Component } from '@angular/core';
import { NavigationFacade } from 'src/app/store/sidenav/facade';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent {
  constructor(private readonly navigationFacade: NavigationFacade){}

  handleClick = () => {
    this.navigationFacade.toggleSideNav();
  };
}
