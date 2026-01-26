import { initialNavigationState, navigationReducer } from './layout/sidenav/sidenav.reducers';
import { initialJwtState, loggingInReducer } from './auth/auth.reducers';
import type { ActionReducerMap } from '@ngrx/store';

export type WarehouseEngineStore = {
  readonly auth: typeof initialJwtState;
  readonly navigation: typeof initialNavigationState;
};

export const reducers = {
  auth: loggingInReducer,
  navigation: navigationReducer,
} as const satisfies ActionReducerMap<WarehouseEngineStore>;
