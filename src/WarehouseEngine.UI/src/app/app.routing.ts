import { LoginComponent } from './pages/login/login.component';
import { isAuthenticatedGuard } from './guards/auth.guard';
import type { Route } from '@angular/router';

export const routes: Route[] = [
  {
    path: '',
    canActivate: [isAuthenticatedGuard],
    loadComponent: () => import('./pages/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'warehouses',
    canActivate: [isAuthenticatedGuard],
    loadComponent: () =>
      import('./pages/warehouse/warehouse.component').then((m) => m.WarehouseComponent),
  },
  { path: 'login', component: LoginComponent },
];
