import { createAction, props } from '@ngrx/store';
import type { JwtTokenResponse } from 'src/app/types/jwt';

export const setJwtToken = createAction('[Auth] Set JWT Token', props<{ jwtResponse: JwtTokenResponse }>());
export const retrieveJwtToken = createAction(
  '[Auth] Retrieving JWT Token',
  props<{ username: string; password: string }>()
);
export const unsetJwtToken = createAction('[Auth] Unset JWT Token');
