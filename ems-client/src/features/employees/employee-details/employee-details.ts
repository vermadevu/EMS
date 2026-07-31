import { Component, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { EmployeeService } from '../../../core/services/employee-service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Employee } from '../models/employee';
import { finalize } from 'rxjs';
import { DetailItemComponent } from '../../../shared/components/detail-item/detail-item';
import { MatIconModule } from '@angular/material/icon';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-employee-details',
  imports: [
    DetailItemComponent,
    PageHeaderComponent,
    RouterLink,
    MatIconModule,
    StatusBadge,
    DatePipe
  ],
  templateUrl: './employee-details.html',
  styleUrl: './employee-details.css',
})
export class EmployeeDetailsComponent {
  private readonly employeeService = inject(EmployeeService);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal(true);
  readonly employee = signal<Employee | null>(null);

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.employeeService.getById(id)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: employee => {
          this.employee.set(employee);
        },
        error: console.error
      });
  }
}
