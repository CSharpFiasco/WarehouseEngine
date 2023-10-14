import { createReducer, on } from '@ngrx/store';
import * as SidenavActions from './sidenav.actions';

export interface NavigationState {
  isSideNavOpen: boolean;
}

export const initialState: NavigationState = {
  isSideNavOpen: false,
};

export const navigationReducer = createReducer(
  initialState,
  on(SidenavActions.toggleSidenav, (state) => ({
    ...state,
    isSideNavOpen: !state.isSideNavOpen,
  }))
);