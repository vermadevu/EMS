import { Component, inject, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { AssetListItem } from '../../../core/models/asset-list-item';

import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { ActionMenuItem } from '../../../shared/models/action-menu-item';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { ActionMenu } from '../../../shared/components/action-menu/action-menu';
import { Router } from '@angular/router';

@Component({
  selector: 'app-asset-table',
  imports: [
    MatIconModule,
    EmptyStateComponent,
    StatusBadge,
    ActionMenu
  ],
  templateUrl: './asset-table.html',
  styleUrl: './asset-table.css'
})
export class AssetTableComponent {

  readonly assets = input.required<AssetListItem[]>();

  readonly loading = input.required<boolean>();

  readonly sortBy = input.required<string>();

  readonly sortDirection = input.required<'asc' | 'desc'>();

  readonly sortChange = output<string>();

  private readonly router = inject(Router);

  readonly action = output<{
    action: string;
    asset: AssetListItem;
  }>();

  assetDetails(asset : AssetListItem){
    return this.router.navigate(['assets',asset.id]);
  }

  getSortIcon(column: string): string {

    if (this.sortBy() !== column) {
      return 'unfold_more';
    }

    return this.sortDirection() === 'asc'
      ? 'arrow_upward'
      : 'arrow_downward';

  }

  getActions(asset: AssetListItem): ActionMenuItem[] {

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
      ...(asset.status === 'Available'
        ? [{
          id: 'assign',
          label: 'Assign',
          icon: 'assignment'
        }]
        : [{
          id: 'return',
          label: 'Return',
          icon: 'assignment_return'
        }]),
      {
        id: 'delete',
        label: 'Delete',
        icon: 'delete',
        color: 'error',
        disabled: asset.status === 'Assigned'
      }
    ];
  }

  onActionSelected(action: string, asset: AssetListItem): void {
    this.action.emit({
      action,
      asset
    });
  }

  getAssetTypeName(type: string): string {
    return type;
  }

  getStatusName(status: string): string {
    return status;
  }
}