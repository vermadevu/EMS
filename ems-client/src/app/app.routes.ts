import { Routes } from '@angular/router';

import { Login } from '../features/auth/login/login';
import { guestGuard } from '../core/guards/guest-guard';
import { authGuard } from '../core/guards/auth-guard';
import { Shell } from '../core/layouts/shell/shell/shell';
import { Dashboard } from '../features/dashboard/dashboard';

export const routes: Routes = [

  {
    path: 'login',
    component: Login,
    canActivate: [guestGuard]
  },

  {
    path: '',
    component: Shell,
    canActivate: [authGuard],
    children: [

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },

      {
        path: 'dashboard',
        component: Dashboard
      }

    ]
  },

  {
    path: '**',
    redirectTo: '/'
  }

];