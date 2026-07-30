import { Component, input, output } from '@angular/core';
import { EmployeeListItem } from '../../../shared/models/employee-list-item';
import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-employee-table',
  imports: [
    EmptyStateComponent,
    StatusBadge,
    ActionMenu,
    DatePipe,
    MatIconModule
  ],
  templateUrl: './employee-table.html',
  styleUrl: './employee-table.css',
})
export class EmployeeTableComponent {
  readonly employees = input.required<EmployeeListItem[]>();
  readonly loading = input(false);
  readonly view = output<number>();
  readonly edit = output<number>();
  readonly delete = output<number>();
  readonly activate = output<number>();
  readonly completeOnboarding = output<number>();
  readonly sortBy = input.required<string>();
  readonly sortDirection = input.required<'asc' | 'desc'>();
  readonly sortChange = output<string>();

  getActions(employee: EmployeeListItem): ActionMenuItem[] {
    return [
      {
        id: 'view',
        label: 'View',
        icon: 'visibility'
      },

      {
        id: 'edit',
        label: 'Edit',
        icon: 'edit'
      },

      ...(employee.status === 'Pending'
        ? [{
          id: 'completeOnboarding',
          label: 'Complete Onboarding',
          icon: 'check_circle'
        }]
        : []),

      ...(employee.status !== 'Active'
        ? [{
          id: 'activate',
          label: 'Activate',
          icon: 'task_alt'
        }]
        : []),
      {
        id: 'delete',
        label: 'Delete',
        icon: 'delete',
        color: 'error'
      }
    ];
  }


  onAction(employeeId: number, action: string): void {
    switch (action) {

      case 'view':
        this.view.emit(employeeId);
        break;

      case 'edit':
        this.edit.emit(employeeId);
        break;

      case 'delete':
        this.delete.emit(employeeId);
        break;

      case 'activate':
        this.activate.emit(employeeId);
        break;

      case 'completeOnboarding':
        this.completeOnboarding.emit(employeeId);
        break;
    }
  }
  isSorted(column: string): boolean {
    return this.sortBy() === column;
  }

  getSortIcon(column: string): string {

    if (!this.isSorted(column)) {
      return 'unfold_more';
    }

    return this.sortDirection() === 'asc'
      ? 'keyboard_arrow_up'
      : 'keyboard_arrow_down';

  }
}
