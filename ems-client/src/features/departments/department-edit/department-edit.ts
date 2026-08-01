import { Component, inject, signal } from '@angular/core';
import { finalize } from 'rxjs';
import { UpdateDepartment } from '../../../core/models/update-department';
import { DepartmentService } from '../../../core/services/department-service';
import { NotificationService } from '../../../core/services/notification-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Department } from '../../../core/models/department';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { DepartmentFormComponent } from '../department-form/department-form';

@Component({
  selector: 'app-department-edit',
  imports: [
    PageHeaderComponent,
    DepartmentFormComponent
  ],
  templateUrl: './department-edit.html',
  styleUrl: './department-edit.css',
})
export class DepartmentEditComponent {
  readonly loading = signal(false);
  readonly department = signal<Department | null>(null);

  private readonly departmentService = inject(DepartmentService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly departmentId = Number(
    this.route.snapshot.paramMap.get('id')
  );

  ngOnInit(): void {
    this.loadDepartment();
  }

  private loadDepartment(): void {

    this.loading.set(true);

    this.departmentService
      .getById(this.departmentId)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({

        next: department => this.department.set(department),

        error: console.error

      });

  }

  updateDepartment(dto: UpdateDepartment): void {
    this.loading.set(true);
    this.departmentService
      .update(this.departmentId, dto)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Department updated successfully.'
          );
          this.router.navigate(['/departments']);
        },
        error: console.error
      });
  }

  cancel(): void {
    this.router.navigate(['/departments']);
  }
}
