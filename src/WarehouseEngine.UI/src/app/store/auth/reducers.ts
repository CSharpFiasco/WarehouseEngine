import { createReducer, on } from "@ngrx/store";
import { StrictUnion } from "src/app/types/strict-union";
import * as AuthActions from './actions';

type Error = {
    kind: 'error';
    message?: string;
}
type Loading = { kind: 'loading' }
type LoggedIn = { kind: 'loggedIn' }
type Idle = { kind: 'idle' }
type AuthState = {
    // status: StrictUnion<Idle | Loading | LoggedIn | Error>
    status: Idle | Loading | LoggedIn | Error
}

const initialAuthState: AuthState = {
    status: {
        kind: 'error',
        message: 'test'
    }
}

export const authReducer = createReducer(
    initialAuthState,
    on(AuthActions.login, (state) => ({ ...state, status: {
        kind: 'loading'
    } })),
  );