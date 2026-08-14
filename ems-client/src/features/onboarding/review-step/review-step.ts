import { Component, inject, OnInit, signal } from '@angular/core';
import { EmployeeProfile } from '../../../core/models/employee-profile';
import { Asset } from '../../../core/models/asset';
import { finalize, forkJoin } from 'rxjs';
import { EmployeeService } from '../../../core/services/employee-service';
import { DocumentService } from '../../../core/services/document-service';
import { AssetService } from '../../../core/services/asset-service';
import { Document } from '../../../core/models/document';
import { OnboardingService } from '../../../core/services/onboarding-service';
import { NotificationService } from '../../../core/services/notification-service';
import { Router } from '@angular/router';
import { DetailItemComponent } from '../../../shared/components/detail-item/detail-item';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';

@Component({
  selector: 'app-review-step',
  imports: [
    DetailItemComponent,
    MatIconModule,
    DatePipe,
    StatusBadge
  ],
  templateUrl: './review-step.html',
  styleUrl: './review-step.css',
})
export class ReviewStepComponent implements OnInit {
  readonly loading = signal(false);
  readonly profile = signal<EmployeeProfile | null>(null);
  readonly documents = signal<Document[]>([]);
  readonly assets = signal<Asset[]>([]);

  private readonly employeeService = inject(EmployeeService);
  private readonly documentService = inject(DocumentService);
  private readonly assetService = inject(AssetService);
  private readonly onboardingService = inject(OnboardingService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  readonly confirmed = signal(false);

  ngOnInit(): void {
    this.load();
  }


  load(): void {
    this.loading.set(true);
    forkJoin({
      profile: this.employeeService.getMyProfile(),
      documents: this.documentService.getMyDocuments(),
      assets: this.assetService.getMyAssets()
    })
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: response => {
          this.profile.set(response.profile);
          this.documents.set(response.documents);
          this.assets.set(response.assets);
        }
      });
  }

  finish(): void {
    this.loading.set(true);
    this.employeeService
      .completeOnboarding()
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.onboardingService.complete(
            '/onboarding/review'
          );
          this.notificationService.success(
            'Welcome to the company!'
          );
          this.router.navigateByUrl('/dashboard');
        }
      });
  }

    testFinish(): void {
    this.loading.set(true);
    this.employeeService
      .getMyProfile()
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.onboardingService.complete(
            '/onboarding/review'
          );
          this.notificationService.success(
            'Welcome to the company!'
          );
          console.log("onboarding complete")
          this.router.navigate([
            '/dashboard'
          ]);
        }
      });
  }

  getAssetTypeName(type: string): string {
    return type;
  }
}
