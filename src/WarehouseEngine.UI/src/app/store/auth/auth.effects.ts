import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, mergeMap, tap } from 'rxjs';
import { retrieveJwtToken, setJwtToken, unsetJwtToken } from './auth.actions';
import { LoginService } from 'src/app/services/login/login.service';
import type { JwtTokenResponse } from 'src/app/types/jwt';
import type { Action } from '@ngrx/store';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {
  private readonly actions$: Actions = inject(Actions);
  private readonly loginService: LoginService = inject(LoginService);
  private readonly authService: AuthService = inject(AuthService);
  private readonly router: Router = inject(Router);

  public readonly authenticate$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(retrieveJwtToken),
        mergeMap((action) => this.loginService.login$(action.username, action.password)),
        map((x: JwtTokenResponse) => this.#mapJwtResponseToAuthAction(x))
      ),
    { dispatch: true }
  );

  public readonly setJwtToken$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(setJwtToken),
        tap(async (action) => {
          if (action.jwtResponse.type === 'Success') {
            this.authService.setJwtToken(action.jwtResponse.jwt);
            await this.router.navigate(['/']);
          } else if (action.jwtResponse.type === 'Unauthorized' || action.jwtResponse.type === 'Failed') {
            this.authService.unsetJwtToken();
            await this.router.navigate(['/login']);
          }
        })
      ),
    { dispatch: false }
  );

  public readonly unsetJwtToken$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(unsetJwtToken),
        tap(() => {
          this.authService.unsetJwtToken();
          this.router.navigate(['/login']);
        })
      ),
    { dispatch: false }
  );

  readonly #mapJwtResponseToAuthAction = (jwtResponse: JwtTokenResponse): Action => {
    console.log('mapJwtResponseToAuthAction', jwtResponse.type);
    switch (jwtResponse.type) {
      case 'Success':
        return setJwtToken({ jwtResponse });
      case 'Unauthorized':
      case 'Failed':
        return unsetJwtToken();
    }
  };
}
