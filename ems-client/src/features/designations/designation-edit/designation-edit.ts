import { Component, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { DesignationService } from '../../../core/services/designation-service';
import { NotificationService } from '../../../core/services/notification-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Designation } from '../../../core/models/designation';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { DesignationFormComponent } from '../designation-form/designation-form';
import { UpdateDesignation } from '../../../core/models/update-desgination';

@Component({
  selector: 'app-designation-edit',
  imports: [
    PageHeaderComponent,
    DesignationFormComponent
  ],
  templateUrl: './designation-edit.html',
  styleUrl: './designation-edit.css',
})
export class DesignationEditComponent {
  readonly loading = signal(false);
  readonly designation = signal<Designation | null>(null);

  private readonly designationService = inject(DesignationService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly designationId = Number(
    this.route.snapshot.paramMap.get('id')
  );

  ngOnInit(): void {
    this.loadDesignation();
  }

  private loadDesignation(): void {

    this.loading.set(true);

    this.designationService
      .getById(this.designationId)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({

        next: designation => this.designation.set(designation),

        error: console.error

      });

  }

  updateDesignation(dto: UpdateDesignation): void {
    this.loading.set(true);
    this.designationService
      .update(this.designationId, dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Designation updated successfully.'
          );
          this.router.navigate(['/designations']);
        },
        error: console.error
      });
  }

  cancel(): void {
    this.router.navigate(['/designations']);
  }
}
