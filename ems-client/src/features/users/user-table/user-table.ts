import { Component, inject, input, output } from '@angular/core';
import { Router } from '@angular/router';

import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';

import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { UserListItem } from '../../../core/models/user-list-item';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [
    EmptyStateComponent,
    ActionMenu
  ],
  templateUrl: './user-table.html',
  styleUrl: './user-table.css'
})
export class UserTableComponent {
  readonly users = input.required<UserListItem[]>();
  readonly loading = input(false);
  readonly action = output<{
    action: string;
    user: UserListItem;
  }>();
  private readonly router = inject(Router);

  userDetails(user: UserListItem) {
    this.router.navigate(['/employees', user.employeeId]);
  }

  onActionSelected(action: string, user: UserListItem) {
    this.action.emit({
      action,
      user
    });
  }

  getActions(user: UserListItem): ActionMenuItem[] {
    return [
      {
        id: 'editRoles',
        label: 'Edit Roles',
        icon: 'admin_panel_settings'
      },
      {
        id: 'resetPassword',
        label: 'Reset Password',
        icon: 'lock_reset'
      },
      ...(user.isActive
        ? [{
            id: 'disable',
            label: 'Disable',
            icon: 'block'
          }]
        : [{
            id: 'enable',
            label: 'Enable',
            icon: 'check_circle'
          }]
      )
    ];
  }

}