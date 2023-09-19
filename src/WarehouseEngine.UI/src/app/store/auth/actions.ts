import { createAction, props } from '@ngrx/store';

export const setJwtToken = createAction('[Auth] Set JWT Token', props<{ jwt: string }>());
