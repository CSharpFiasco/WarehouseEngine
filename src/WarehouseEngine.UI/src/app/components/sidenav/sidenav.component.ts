import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { NavigationFacade } from 'src/app/store/sidenav/facade';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
  ],
  standalone: true
})
export class SidenavComponent {
  protected sideNavOpen$ = this.navigationFacade.sideNavOpen$;
  constructor(private readonly navigationFacade: NavigationFacade){}
}
