import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule } from '@angular/router';
import { LayoutFacade } from 'src/app/store/layout/facade';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
  imports: [CommonModule, MatSidenavModule, MatListModule, RouterModule],
  standalone: true,
})
export class SidenavComponent {
  private readonly navigationFacade: LayoutFacade = inject(LayoutFacade);

  protected sideNavOpen$ = this.navigationFacade.sideNavOpen$;
}
