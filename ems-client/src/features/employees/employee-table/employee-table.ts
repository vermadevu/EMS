import { Component, inject, input, output } from '@angular/core';
import { EmployeeListItem } from '../../../shared/models/employee-list-item';
import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

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

  private readonly router = inject(Router);

  readonly action = output<{
    action: string;
    employee: EmployeeListItem;
  }>();

  onActionSelected(action: string, employee: EmployeeListItem): void {
    this.action.emit({
      action,
      employee
    });
  }

  viewEmployee(id: number): void {
    this.router.navigate(['/employees', id]);
  }

  editEmployee(id: number): void {
    this.router.navigate(['/employees/edit', id]);
  }

  deleteEmployee(id: number) {
    console.log('Delete', id);
  }

  activateEmployee(id: number) {
    console.log('Activate', id);
  }

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
