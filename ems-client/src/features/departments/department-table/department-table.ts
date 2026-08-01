import { Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';


import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';
import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { DepartmentListItem } from '../../../core/models/department-list-item';

@Component({
  selector: 'app-department-table',
  imports: [
    MatIconModule,
    EmptyStateComponent,
    ActionMenu
  ],
  templateUrl: './department-table.html',
  styleUrl: './department-table.css'
})
export class DepartmentTableComponent {

  readonly departments = input.required<DepartmentListItem[]>();
  readonly loading = input.required<boolean>();
  readonly sortBy = input.required<string>();
  readonly sortDirection = input.required<'asc' | 'desc'>();

  readonly sortChange = output<string>();
  readonly action = output<{
    action: string;
    department: DepartmentListItem;
  }>();

  getSortIcon(column: string): string {

    if (this.sortBy() !== column) {
      return 'unfold_more';
    }

    return this.sortDirection() === 'asc'
      ? 'arrow_upward'
      : 'arrow_downward';
  }

  getActions(): ActionMenuItem[] {
    return [
      {
        id: 'edit',
        label: 'Edit',
        icon: 'edit'
      },
      {
        id: 'delete',
        label: 'Delete',
        icon: 'delete',
        color: 'error'
      }
    ];
  }

  onActionSelected(action: string, department: DepartmentListItem): void {
    this.action.emit({
      action,
      department
    });
  }

}