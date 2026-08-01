import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  debounceTime,
  distinctUntilChanged,
  finalize,
  Subject
} from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';


import { AssetService } from '../../../core/services/asset-service';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

import { AssetListItem } from '../../../core/models/asset-list-item';
import { PagedResult } from '../../employees/models/paged-result';
import { AssetListState } from '../../../core/models/asset-list-state';
import { AssetTableComponent } from '../asset-table/asset-table';
import { AssetToolbarComponent } from '../asset-toolbar/asset-toolbar';
import { AssignAssetDialogComponent } from '../assign-asset-dialog/assign-asset-dialog';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-asset-list',
  imports: [
    PageHeaderComponent,
    AssetToolbarComponent,
    AssetTableComponent,
    PaginationComponent
  ],
  templateUrl: './asset-list.html',
  styleUrl: './asset-list.css'
})
export class AssetListComponent {

  readonly loading = signal(false);

  readonly page = signal<PagedResult<AssetListItem> | null>(null);

  readonly assets = signal<AssetListItem[]>([]);

  private readonly searchSubject = new Subject<string>();

  private readonly assetService = inject(AssetService);

  private readonly router = inject(Router);

  private readonly confirmationService = inject(ConfirmationService);

  private readonly notificationService = inject(NotificationService);
  private readonly dialog = inject(MatDialog);

  readonly state = signal<AssetListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'assetName',
    sortDirection: 'asc'
  });

  constructor() {

    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {

        this.state.update(state => ({
          ...state,
          search,
          pageNumber: 1
        }));

        this.loadAssets();

      });

  }

  ngOnInit(): void {
    this.loadAssets();
  }

  loadAssets(): void {

    this.loading.set(true);

    this.assetService
      .getAssets(this.state())
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({

        next: result => {

          this.page.set(result);

          this.assets.set(result.items);

        },

        error: console.error

      });

  }

  search(value: string): void {
    this.searchSubject.next(value);
  }

  changePage(pageNumber: number): void {

    this.state.update(state => ({
      ...state,
      pageNumber
    }));

    this.loadAssets();

  }

  changeAssetType(assetType?: number): void {

    this.state.update(state => ({
      ...state,
      assetType,
      pageNumber: 1
    }));

    this.loadAssets();

  }

  changeStatus(status?: number): void {

    this.state.update(state => ({
      ...state,
      status,
      pageNumber: 1
    }));

    this.loadAssets();

  }

  sort(column: string): void {

    const current = this.state();

    const direction =
      current.sortBy === column &&
        current.sortDirection === 'asc'
        ? 'desc'
        : 'asc';

    this.state.update(state => ({
      ...state,
      sortBy: column,
      sortDirection: direction
    }));

    this.loadAssets();

  }

  handleAction(event: {
    action: string;
    asset: AssetListItem;
  }): void {

    switch (event.action) {

      case 'view':
        this.router.navigate(['/assets', event.asset.id]);
        break;

      case 'edit':
        this.router.navigate(['/assets/edit', event.asset.id]);
        break;

      case 'delete':
        this.confirmationService.confirm({
          title: 'Delete Asset',
          message: `Are you sure you want to delete ${event.asset.assetName}?`,
          confirmText: 'Delete',
          confirmButtonClass: 'btn-error'
        })
          .subscribe(confirmed => {
            if (!confirmed) return;
            this.assetService.delete(event.asset.id)
              .subscribe({
                next: () => {
                  this.notificationService.success(
                    'Asset deleted successfully.'
                  );
                  this.loadAssets();
                },
                error: console.error
              });
          });

        break;

      case 'assign':
        this.openAssignDialog(event.asset);
        break;

      case 'return':
        this.returnAsset(event.asset);
        break;

    }

  }

  private openAssignDialog(asset: AssetListItem): void {

    this.dialog.open(AssignAssetDialogComponent, {
      width: '500px',
      disableClose: true,
      autoFocus: false,
      restoreFocus: false,
      panelClass: 'confirm-dialog-panel',
      backdropClass: 'confirm-dialog-backdrop',
      data: {
        assetId: asset.id,
        assetName: asset.assetName
      }
    })
      .afterClosed()
      .subscribe(result => {
        if (!result) {
          return;
        }
        this.notificationService.success(
          'Asset assigned successfully.'
        );
        this.loadAssets();
      });
  }

  private returnAsset(asset: AssetListItem): void {
    this.confirmationService
      .confirm({
        title: 'Return Asset',
        message: `Return ${asset.assetName}?`,
        icon: 'assignment_return',
        confirmText: 'Return',
        confirmButtonClass: 'btn-warning'
      })
      .subscribe(result => {
        if (!result) {
          return;
        }
        this.assetService
          .return(asset.id)
          .subscribe({
            next: () => {
              this.notificationService.success(
                'Asset returned successfully.'
              );
              this.loadAssets();
            },
            error: console.error
          });
      });
  }

}