import { Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';


import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';
import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { DesignationListItem } from '../../../core/models/designation-list-item';

@Component({
  selector: 'app-designation-table',
  imports: [
    MatIconModule,
    EmptyStateComponent,
    ActionMenu
  ],
  templateUrl: './designation-table.html',
  styleUrl: './designation-table.css'
})
export class DesignationTableComponent {

  readonly designations = input.required<DesignationListItem[]>();
  readonly loading = input.required<boolean>();
  readonly sortBy = input.required<string>();
  readonly sortDirection = input.required<'asc' | 'desc'>();

  readonly sortChange = output<string>();
  readonly action = output<{
    action: string;
    designation: DesignationListItem;
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

  onActionSelected(action: string, designation: DesignationListItem): void {
    this.action.emit({
      action,
      designation
    });
  }

}