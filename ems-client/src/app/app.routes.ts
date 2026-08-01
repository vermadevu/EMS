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
import { EmployeeCreateComponent } from '../features/employees/employee-create/employee-create';
import { EmployeeEditComponent } from '../features/employees/employee-edit/employee-edit';
import { EmployeeDetailsComponent } from '../features/employees/employee-details/employee-details';
import { DocumentDetailComponent } from '../features/documents/document-detail/document-detail';
import { AssetCreateComponent } from '../features/assets/asset-create/asset-create';
import { AssetEditComponent } from '../features/assets/asset-edit/asset-edit';
import { AssetDetailComponent } from '../features/assets/asset-detail/asset-detail';
import { DepartmentCreateComponent } from '../features/departments/department-create/department-create';
import { DepartmentEditComponent } from '../features/departments/department-edit/department-edit';
import { DesignationCreateComponent } from '../features/designations/designation-create/designation-create';
import { DesignationEditComponent } from '../features/designations/designation-edit/designation-edit';

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
        path: 'employees',
        component: EmployeeListComponent
      },
      {
        path: 'employees/create',
        component: EmployeeCreateComponent
      },
      {
        path: 'employees/edit/:id',
        component: EmployeeEditComponent
      },
      {
        path: 'employees/:id',
        component: EmployeeDetailsComponent
      },

      {
        path: 'departments',
        children: [
          {
            path: '',
            component: DepartmentListComponent
          },
          {
            path: 'create',
            component: DepartmentCreateComponent
          },
          {
            path: 'edit/:id',
            component: DepartmentEditComponent
          },
        ]
      },

      {
        path: 'designations',
        children: [
          {
            path: '',
            component: DesignationListComponent
          },
          {
            path: 'create',
            component: DesignationCreateComponent
          },
          {
            path: 'edit/:id',
            component: DesignationEditComponent
          }
        ]
      },

      {
        path: 'assets',
        children: [
          {
            path: '',
            component: AssetListComponent
          },
          {
            path: 'create',
            component: AssetCreateComponent
          },
          {
            path: 'edit/:id',
            component: AssetEditComponent
          },
          {
            path: ':id',
            component: AssetDetailComponent
          }
        ]
      },

      {
        path: 'documents',
        component: DocumentListComponent
      },

      {
        path: 'documents/:employeeId',
        component: DocumentDetailComponent
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