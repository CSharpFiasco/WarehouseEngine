import { createFeatureSelector, createSelector } from '@ngrx/store';
import type { NavigationState } from './sidenav.reducers';
import type { WarehouseEngineStore } from '../../initial-state';

const storeKey: keyof WarehouseEngineStore = 'navigation';

export const selectNavigationState = createFeatureSelector<NavigationState>(storeKey);
export const selectSideNavOpen = createSelector(selectNavigationState, (state: NavigationState) => state.isSideNavOpen);
