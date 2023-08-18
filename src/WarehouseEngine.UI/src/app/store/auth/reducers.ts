import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './actions';

type JwtState = { jwt: string | null };

const initialJwtState: JwtState = {
  jwt: null
};

export const loggingInReducer = createReducer(
  initialJwtState,
  on(
    AuthActions.setJwtToken,
    (state: JwtState, { jwt }: { jwt: string }): JwtState => ({
      ...state,
      jwt: 'jwt'
    })
  )
);
