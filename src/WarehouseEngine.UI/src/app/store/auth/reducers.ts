import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './actions';

type JwtState = { readonly jwt: string | null };

const initialJwtState: JwtState = {
  jwt: null,
};

export const loggingInReducer = createReducer(
  initialJwtState,
  on(
    AuthActions.setJwtToken,
    (state: JwtState, { jwt }: { readonly jwt: string }): JwtState => ({
      ...state,
      jwt: jwt,
    })
  )
);
