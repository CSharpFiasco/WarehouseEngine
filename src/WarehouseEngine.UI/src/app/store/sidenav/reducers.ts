import { createReducer, on } from '@ngrx/store';
import { toggleSideNav } from './actions';

export interface NavigationState {
  sideNavOpen: boolean;
}

export const initialState: NavigationState = {
  sideNavOpen: false,
};

export const navigationReducer = createReducer(
  initialState,
  on(toggleSideNav, (state) => ({
    ...state,
    sideNavOpen: !state.sideNavOpen,
  }))
);
