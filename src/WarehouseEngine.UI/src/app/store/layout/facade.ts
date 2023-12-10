import { Injectable, inject } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { toggleSidenav } from './sidenav/sidenav.actions';
import { selectSideNavOpen } from './sidenav/sidenav.selectors';

@Injectable()
export class LayoutFacade {
  private readonly store: Store = inject(Store);

  public readonly sideNavOpen$ = this.store.pipe(select(selectSideNavOpen));

  public toggleSideNav(): void {
    this.store.dispatch(toggleSidenav());
  }
}
