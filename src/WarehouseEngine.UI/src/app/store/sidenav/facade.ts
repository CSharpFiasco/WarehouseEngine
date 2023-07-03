import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { toggleSideNav } from './actions';
import { selectSideNavOpen } from './selectors';

@Injectable()
export class NavigationFacade {
  readonly sideNavOpen$ = this.store.pipe(select(selectSideNavOpen));

  constructor(private readonly store: Store) {}

  toggleSideNav(): void {
    this.store.dispatch(toggleSideNav());
  }
}