import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule } from '@angular/router';
import { LayoutFacade } from 'src/app/store/layout/facade';
import { AuthFacade } from 'src/app/store/auth/auth.facade';

@Component({
    selector: 'app-sidenav',
    templateUrl: './sidenav.component.html',
    styleUrls: ['./sidenav.component.scss'],
    imports: [CommonModule, MatSidenavModule, MatListModule, RouterModule]
})
export class SidenavComponent {
  private readonly navigationFacade: LayoutFacade = inject(LayoutFacade);
  private readonly authFacade: AuthFacade = inject(AuthFacade);

  protected sideNavOpen$ = this.navigationFacade.sideNavOpen$;

  protected logoutOnKeyup(event: KeyboardEvent): void {
    if (event.key === 'Escape') {
      this.logout();
    }
  }

  protected logout(): void {
    this.authFacade.unsetJwtToken();
  }
}
