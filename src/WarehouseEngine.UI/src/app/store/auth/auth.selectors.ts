import { createFeatureSelector, createSelector } from '@ngrx/store';
import type { JwtState } from './auth.reducers';
import type { WarehouseEngineStore } from '../initial-state';

const storeKey: keyof WarehouseEngineStore = 'auth';

export const selectAuthState = createFeatureSelector<JwtState>(storeKey);
export const selectLoginStatus = createSelector(selectAuthState, (state) => state.type);