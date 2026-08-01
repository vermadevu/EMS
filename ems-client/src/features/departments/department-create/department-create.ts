import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { DepartmentFormComponent } from '../department-form/department-form';

import { DepartmentService } from '../../../core/services/department-service';
import { NotificationService } from '../../../core/services/notification-service';

import { CreateDepartment } from '../../../core/models/create-department';

@Component({
  selector: 'app-department-create',
  imports: [
    PageHeaderComponent,
    DepartmentFormComponent
  ],
  templateUrl: './department-create.html',
  styleUrl: './department-create.css'
})
export class DepartmentCreateComponent {

  readonly loading = signal(false);

  private readonly departmentService = inject(DepartmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);

  createDepartment(dto: CreateDepartment): void {
    this.loading.set(true);
    this.departmentService
      .create(dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Department created successfully.'
          );
          this.router.navigate(['/departments']);
        },
        error: error => console.error(error)
      });
  }

  cancel(): void {
    this.router.navigate(['/departments']);
  }
}