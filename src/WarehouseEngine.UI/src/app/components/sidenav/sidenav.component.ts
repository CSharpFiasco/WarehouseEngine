import { Component, inject, type Signal } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule } from '@angular/router';
import { LayoutStore } from '../../store/layout/layout.store';
import { AuthStore } from '../../store/auth/auth.store';

@Component({
    selector: 'app-sidenav',
    templateUrl: './sidenav.component.html',
    styleUrls: ['./sidenav.component.scss'],
    imports: [MatSidenavModule, MatListModule, RouterModule]
})
export class SidenavComponent {
  private readonly layoutStore = inject(LayoutStore);
  private readonly authStore = inject(AuthStore);

  protected readonly isSideNavOpen: Signal<boolean> = this.layoutStore.isSideNavOpen;

  protected logoutOnKeyup(event: KeyboardEvent): void {
    if (event.key === 'Escape') {
      this.logout();
    }
  }

  protected logout(): void {
    this.authStore.logout();
  }
}
