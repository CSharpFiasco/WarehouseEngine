import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { toggleSidenav } from './sidenav/sidenav.actions';
import { selectSideNavOpen } from './sidenav/sidenav.selectors';

@Injectable()
export class LayoutFacade {
  readonly sideNavOpen$ = this.store.pipe(select(selectSideNavOpen));

  constructor(private readonly store: Store) {}

  toggleSideNav(): void {
    this.store.dispatch(toggleSidenav());
  }
}