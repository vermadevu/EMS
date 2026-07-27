import { Routes } from '@angular/router';

import { guestGuard } from '../core/guards/guest-guard';
import { authGuard } from '../core/guards/auth-guard';

import { Login } from '../features/auth/login/login';
import { Shell } from '../core/layouts/shell/shell/shell';

import { DashboardComponent } from '../features/dashboard/dashboard';

import { EmployeeListComponent } from '../features/employees/employee-list/employee-list';

import { DepartmentListComponent } from '../features/departments/department-list/department-list';

import { DesignationListComponent } from '../features/designations/designation-list/designation-list';

import { AssetListComponent } from '../features/assets/asset-list/asset-list';

import { DocumentListComponent } from '../features/documents/document-list/document-list';

import { UserListComponent } from '../features/users/user-list/user-list';
import { RolePermissionsComponent } from '../features/users/role-permissions/role-permissions';
import { UserPermissionsComponent } from '../features/users/user-permissions/user-permissions';

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
        component: DashboardComponent
      },

      {
        path: 'employees',
        component: EmployeeListComponent
      },

      {
        path: 'departments',
        component: DepartmentListComponent
      },

      {
        path: 'designations',
        component: DesignationListComponent
      },

      {
        path: 'assets',
        component: AssetListComponent
      },

      {
        path: 'documents',
        component: DocumentListComponent
      },

      {
        path: 'users',
        component: UserListComponent
      },

      {
        path: 'role-permissions',
        component: RolePermissionsComponent
      },

      {
        path: 'user-permissions',
        component: UserPermissionsComponent
      }

    ]
  },

  {
    path: '**',
    redirectTo: ''
  }

];