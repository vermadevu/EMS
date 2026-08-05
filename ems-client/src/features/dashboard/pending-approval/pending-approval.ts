import { Component, effect, inject, input, signal } from '@angular/core';
import { EmployeeListItem } from '../../../shared/models/employee-list-item';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { EmployeeService } from '../../../core/services/employee-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-pending-approval',
  imports: [
    MatIconModule
  ],
  templateUrl: './pending-approval.html',
  styleUrl: './pending-approval.css',
})
export class PendingApproval {
  readonly title = input.required<string>();
  readonly employees = input.required<EmployeeListItem[]>();
  readonly employeeList = signal<EmployeeListItem[]>([]);
  readonly route = input.required<string>();
  private readonly router = inject(Router);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly employeeService = inject(EmployeeService);
  private readonly notificationService = inject(NotificationService);

  constructor() {
    effect(() => {
      this.employeeList.set(this.employees());
    });
  }

  approve(employee: EmployeeListItem) {
    this.confirmationService.confirm({
      title: 'Approve Employee',
      message: `Approve onboarding for ${employee.fullName}?`,
      icon: 'task_alt',
      confirmText: 'Approve',
      confirmButtonClass: 'btn-success'
    })
      .subscribe(confirmed => {
        if (!confirmed) {
          return;
        }

        this.employeeService.activate(employee.id).subscribe({
          next: () => {
            this.notificationService.success(
              'Employee approved successfully.'
            );

            this.employeeList.update(list =>
              list.filter(x => x.id !== employee.id)
            );
          },
          error: console.error
        });
      });
  }

  go() {
    this.router.navigateByUrl(this.route());
  }

  view(employee: EmployeeListItem) {
    this.router.navigate([
      '/employees',
      employee.id
    ]);
  }
}
