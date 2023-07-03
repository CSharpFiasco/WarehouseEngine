import { createAction, props } from '@ngrx/store';
import { Credentials } from 'src/app/types/credentials';

export const login = createAction('[Auth] Login', props<{ credentials: Credentials }>());
export const loginSuccess = createAction('[Auth] Login Success');
export const loginFailure = createAction('[Auth] Login Failure', props<{ error: unknown }>());
