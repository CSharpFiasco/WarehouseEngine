import { Router, type CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  console.log('authGuard', authService.getJwtToken());

  return authService.getJwtToken() !== null ? true : router.navigate(['/login']);
};
