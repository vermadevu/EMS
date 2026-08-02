import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { Asset } from '../../../core/models/asset';

import { AssetService } from '../../../core/services/asset-service';
import { NotificationService } from '../../../core/services/notification-service';
import { OnboardingService } from '../../../core/services/onboarding-service';
import { MatIconModule } from '@angular/material/icon';
import { DetailItemComponent } from '../../../shared/components/detail-item/detail-item';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';

@Component({
  selector: 'app-assets-step',
  imports: [
    RouterLink,
    MatIconModule,
    DetailItemComponent,
    StatusBadge
  ],
  templateUrl: './assets-step.html',
  styleUrl: './assets-step.css'
})
export class AssetsStepComponent implements OnInit {
  readonly loading = signal(false);
  readonly assets = signal<Asset[]>([]);
  private readonly assetService = inject(AssetService);
  private readonly router = inject(Router);
  private readonly onboardingService = inject(OnboardingService);
  private readonly notificationService = inject(NotificationService);

  ngOnInit(): void {
    this.loadAssets();
  }

  loadAssets(): void {
    this.loading.set(true);
    this.assetService
      .getMyAssets()
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: assets => {
          this.assets.set(assets);
        }
      });
  }

  continue(): void {
    this.onboardingService.complete(
      '/onboarding/assets'
    );
    this.router.navigate([
      '/onboarding/policies'
    ]);
  }
}