import { Injectable, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { retrieveJwtToken, unsetJwtToken } from './auth.actions';
import type { WarehouseEngineStore } from '../initial-state';
import { selectLoginStatus } from './auth.selectors';

@Injectable()
export class AuthFacade {
  private readonly store: Store<WarehouseEngineStore> = inject(Store);
  public readonly loginStatus$ = this.store.select(selectLoginStatus);

  public login(username: string, password: string): void {
    this.store.dispatch(retrieveJwtToken({ username, password }));
  }

  public unsetJwtToken(): void {
    this.store.dispatch(unsetJwtToken());
  }
}
