import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { AssetFormComponent } from '../asset-form/asset-form';

import { AssetService } from '../../../core/services/asset-service';
import { NotificationService } from '../../../core/services/notification-service';

import { Asset } from '../../../core/models/asset';
import { UpdateAsset } from '../../../core/models/update-asset';

@Component({
  selector: 'app-asset-edit',
  imports: [
    PageHeaderComponent,
    AssetFormComponent
  ],
  templateUrl: './asset-edit.html',
  styleUrl: './asset-edit.css'
})
export class AssetEditComponent {

  readonly loading = signal(false);

  readonly asset = signal<Asset | null>(null);

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly assetService = inject(AssetService);
  private readonly notificationService = inject(NotificationService);

  ngOnInit(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.assetService.getAsset(id).subscribe({

      next: asset => this.asset.set(asset),

      error: console.error

    });

  }

  update(dto: UpdateAsset): void {

    const id = this.asset()!.id;

    this.loading.set(true);

    this.assetService
      .update(id, dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({

        next: () => {

          this.notificationService.success(
            'Asset updated successfully.'
          );

          this.router.navigate(['/assets']);

        },

        error: console.error

      });

  }

  cancel(): void {
    this.router.navigate(['/assets']);
  }

}