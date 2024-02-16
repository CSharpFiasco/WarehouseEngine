import { initialNavigationState, navigationReducer } from './layout/sidenav/sidenav.reducers';
import { initialJwtState, loggingInReducer } from './auth/auth.reducers';
import type { ActionReducerMap } from '@ngrx/store';

export type WarehouseEngineStore = {
  auth: typeof initialJwtState;
  navigation: typeof initialNavigationState;
};

export const reducers: ActionReducerMap<WarehouseEngineStore> = {
  auth: loggingInReducer,
  navigation: navigationReducer,
} as const;
