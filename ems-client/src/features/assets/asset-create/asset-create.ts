import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { AssetFormComponent } from '../asset-form/asset-form';

import { AssetService } from '../../../core/services/asset-service';
import { NotificationService } from '../../../core/services/notification-service';

import { CreateAsset } from '../../../core/models/create-asset';

@Component({
  selector: 'app-asset-create',
  imports: [
    PageHeaderComponent,
    AssetFormComponent
  ],
  templateUrl: './asset-create.html',
  styleUrl: './asset-create.css'
})
export class AssetCreateComponent {

  readonly loading = signal(false);

  private readonly assetService = inject(AssetService);
  private readonly notificationService = inject(NotificationService);
  readonly router = inject(Router);

  create(dto: CreateAsset): void {
    this.loading.set(true);
    this.assetService.create(dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Asset created successfully.'
          );
          this.router.navigate(['/assets']);
        },
        error: error => console.error(error)
      });
  }

  cancel(): void {
    this.router.navigate(['/assets']);
  }

}