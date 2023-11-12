import { LoginComponent } from './pages/login/login.component';
import { authGuard } from './guards/auth.guard';
import type { Route } from '@angular/router';

export const routes: Route[] = [
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/home/home.component').then((m) => m.HomeComponent),
  },
  { path: 'login', component: LoginComponent },
];
