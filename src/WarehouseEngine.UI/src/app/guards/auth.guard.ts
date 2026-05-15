import { Router, type CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { AuthStore } from '../store/auth/auth.store';

export const isAuthenticatedGuard: CanActivateFn = () => {
  const router = inject(Router);
  const store = inject(AuthStore)

  if (store.isLoggedIn()) {
    return Promise.resolve(true);
  }

  return router.navigate(['/login']);
};
