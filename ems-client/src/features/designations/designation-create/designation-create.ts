import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { DesignationFormComponent } from '../designation-form/designation-form';

import { DesignationService } from '../../../core/services/designation-service';
import { NotificationService } from '../../../core/services/notification-service';

import { CreateDesignation } from '../../../core/models/create-designation';

@Component({
  selector: 'app-designation-create',
  imports: [
    PageHeaderComponent,
    DesignationFormComponent
  ],
  templateUrl: './designation-create.html',
  styleUrl: './designation-create.css'
})
export class DesignationCreateComponent {

  readonly loading = signal(false);

  private readonly designationService = inject(DesignationService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  createDesignation(dto: CreateDesignation): void {
    this.loading.set(true);
    this.designationService
      .create(dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Designation created successfully.'
          );
          this.router.navigate(['/designations']);
        },
        error: error => console.error(error)
      });
  }

  cancel(): void {
    this.router.navigate(['/designations']);
  }
}