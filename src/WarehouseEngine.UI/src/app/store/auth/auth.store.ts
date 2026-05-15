import { computed, inject, InjectionToken } from '@angular/core';
import { Router } from '@angular/router';
import { signalStore, withHooks, withComputed, withMethods, withState, patchState } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, switchMap, tap } from 'rxjs';
import { LoginService } from '../../services/login/login.service';
import { AuthService } from '../../services/auth.service';
import type { JwtTokenResponse } from '../../types/jwt';

export type JwtState = { type: 'logged in'; readonly jwt: string } | { type: 'logged out' } | { type: 'logging in' };
export type JwtStateType = JwtState['type'];

type AuthState = {
  readonly authState: JwtState;
};

const initialState: AuthState = {
  authState: { type: 'logged out' },
};

function getJwtStateByJwtResponse(jwtResponse: JwtTokenResponse): JwtState {
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
    default: {
      throw new Error(`Unhandled JwtTokenResponse type: ${jwtResponse['type']}`);
    }
  }
}

export const AuthStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withHooks({
    onInit(store, authService = inject(AuthService)) {
      const token = authService.getJwtToken();
      if (token) {
        patchState(store, {
          authState: {
            type: 'logged in',
            jwt: token,
          }
        });
      }
    }
  }),
  withComputed((store) => ({
    loginStatus: computed(() => store.authState().type),
    isLoggedIn: computed(() => store.authState().type === 'logged in'),
    isLoggingIn: computed(() => store.authState().type === 'logging in'),
  })),
  withMethods((store, loginService = inject(LoginService), authService = inject(AuthService), router = inject(Router)) => ({
    login: rxMethod<{ username: string; password: string }>(
      pipe(
        tap(() => patchState(store, { authState: { type: 'logging in' } })),
        switchMap(({ username, password }) => loginService.login$(username, password)),
        tap((response: JwtTokenResponse) => {
          const newState = getJwtStateByJwtResponse(response);

          if (response.type === 'Success') {
            authService.setJwtToken(response.jwt);
            patchState(store, { authState: newState });
            router.navigate(['/']);
          } else {
            authService.unsetJwtToken();
            patchState(store, { authState: newState });
            router.navigate(['/login']);
          }
        })
      )
    ),
    logout(): void {
      authService.unsetJwtToken();
      patchState(store, { authState: { type: 'logged out' } });
      router.navigate(['/login']);
    },
  }),
  )
);
