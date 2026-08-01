import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AssetService } from '../../../core/services/asset-service';
import { Asset } from '../../../core/models/asset';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { AssetStatus } from '../../../core/models/asset-status';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';
import { MatDialog } from '@angular/material/dialog';
import { AssignAssetDialogComponent } from '../assign-asset-dialog/assign-asset-dialog';

@Component({
  selector: 'app-asset-detail',
  imports: [
    PageHeaderComponent,
    StatusBadge
  ],
  templateUrl: './asset-detail.html',
  styleUrl: './asset-detail.css'
})
export class AssetDetailComponent {

  readonly asset = signal<Asset | null>(null);

  private readonly route = inject(ActivatedRoute);

  readonly router = inject(Router);

  private readonly assetService = inject(AssetService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly notificationService = inject(NotificationService);
  readonly AssetStatus = AssetStatus;
  private readonly dialog = inject(MatDialog);

  private loadAsset(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.assetService
      .getAsset(id)
      .subscribe({
        next: asset => this.asset.set(asset),
        error: console.error
      });

  }

  ngOnInit(): void {
    this.loadAsset();
  }

  returnAsset(): void {
    const asset = this.asset();

    if (!asset) {
      return;
    }
    this.confirmationService.confirm({
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
              this.loadAsset();
            },
            error: console.error
          });
      });
  }

  assignAsset(): void {
    const asset = this.asset();

    if (!asset) {
      return;
    }

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

        if (result) {
          this.loadAsset();
        }

      });

  }
}