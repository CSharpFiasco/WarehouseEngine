import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';
import type { JwtTokenResponse } from 'src/app/types/jwt';

// #region Auth
export type JwtState = { type: 'logged in'; readonly jwt: string } | { type: 'logged out' } | { type: 'logging in' };
export type JwtStateType = JwtState['type'];

export const initialJwtState: JwtState = {
  type: 'logged out',
} as const;

const throwIfUnexpectedAction = (action: never): never => {
  throw new Error(`Unexpected action: ${action}`);
};

const throwIfUnexpectedJwtResponse = (jwtResponse: never): never => {
  throw new Error(`Unexpected jwtResponse: ${jwtResponse}`);
};

const getJwtStateByJwtResponse = (jwtResponse: JwtTokenResponse): JwtState => {
  switch (jwtResponse.type) {
    case 'Success':
      return {
        type: 'logged in',
        jwt: jwtResponse.jwt,
      };
    case 'Unauthorized':
    case 'Failed':
      return {
        type: 'logged out',
      };
    default:
      return throwIfUnexpectedJwtResponse(jwtResponse);
  }
};

export const loggingInReducer = createReducer<JwtState>(
  initialJwtState,
  on(AuthActions.setJwtToken, AuthActions.unsetJwtToken, AuthActions.retrieveJwtToken, (state, action) => {
    switch (action.type) {
      case AuthActions.retrieveJwtToken.type:
        return {
          type: 'logging in',
        };
      case AuthActions.setJwtToken.type:
        return getJwtStateByJwtResponse(action.jwtResponse);
      case AuthActions.unsetJwtToken.type:
        return {
          type: 'logged out',
        };
      default:
        return throwIfUnexpectedAction(action);
    }
  })
);
