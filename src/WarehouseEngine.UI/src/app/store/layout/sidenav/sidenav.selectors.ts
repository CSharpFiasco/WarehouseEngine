import { createFeatureSelector, createSelector } from '@ngrx/store';
import type { NavigationState } from './sidenav.reducers';

export const selectNavigationState = createFeatureSelector<NavigationState>('navigation');
export const selectSideNavOpen = createSelector(
  selectNavigationState,
  (state: NavigationState) => state.isSideNavOpen
);