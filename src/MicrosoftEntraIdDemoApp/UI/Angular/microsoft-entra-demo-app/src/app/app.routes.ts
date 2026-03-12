import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  {
    path: 'usercheck',
    loadComponent: () => import('./usercheck/usercheck.component').then(m => m.UserCheckComponent),
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];
