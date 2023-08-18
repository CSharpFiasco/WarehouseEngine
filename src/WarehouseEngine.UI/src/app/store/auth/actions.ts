import { createAction, props } from '@ngrx/store';
import { JwtTokenResponse } from 'src/app/types/jwt';

export const setJwtToken = createAction('[Auth] Set JWT Token', props<{ jwt: string }>());
