import { createReducer, on } from '@ngrx/store';
import * as SidenavActions from './sidenav.actions';

export type NavigationState = {
  readonly isSideNavOpen: boolean;
};

export const initialNavigationState: NavigationState = {
  isSideNavOpen: false,
};

export const navigationReducer = createReducer<NavigationState>(
  initialNavigationState,
  on(SidenavActions.toggleSidenav, (state) => ({
    ...state,
    isSideNavOpen: !state.isSideNavOpen,
  }))
);
