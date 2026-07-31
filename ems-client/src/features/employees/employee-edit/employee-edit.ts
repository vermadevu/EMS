import { Component, inject, OnInit, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee-service';
import { Employee } from '../models/employee';
import { finalize } from 'rxjs';
import { EmployeeFormComponent } from '../components/employee-form/employee-form';

@Component({
  selector: 'app-employee-edit',
  imports: [
    PageHeaderComponent,
    EmployeeFormComponent
  ],
  templateUrl: './employee-edit.html',
  styleUrl: './employee-edit.css',
})
export class EmployeeEditComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly employeeService = inject(EmployeeService);
  private readonly router = inject(Router);

  readonly employee = signal<Employee | null>(null);
  readonly loading = signal(true);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.employeeService
      .getById(id)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: employee => {
          this.employee.set(employee);
        },
        error: error => {
          console.error(error);
          this.router.navigate(['/employees']);
        }
      });
  }
}
