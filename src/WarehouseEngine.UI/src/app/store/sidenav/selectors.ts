import { createFeatureSelector, createSelector } from '@ngrx/store';
import { NavigationState } from './reducers';

export const selectNavigationState = createFeatureSelector<NavigationState>('navigation');
export const selectSideNavOpen = createSelector(
  selectNavigationState,
  (state: NavigationState) => state.sideNavOpen
);