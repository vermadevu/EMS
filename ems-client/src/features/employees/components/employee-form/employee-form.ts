import { Component, computed, effect, inject, input, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { Department } from '../../../../core/models/department';
import { Designation } from '../../../../core/models/designation';
import { DepartmentService } from '../../../../core/services/department-service';
import { DesignationService } from '../../../../core/services/designation-service';
import { EmployeeService } from '../../../../core/services/employee-service';
import { Employee } from '../../models/employee';
import { finalize, forkJoin } from 'rxjs';
import { MatSpinner } from '@angular/material/progress-spinner';
import { CreateEmployeeRequest } from '../../models/create-employee-request';
import { UpdateEmployeeRequest } from '../../models/update-employee-request';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    MatIconModule,
    ReactiveFormsModule,
    MatSpinner
  ],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css',
})
export class EmployeeFormComponent implements OnInit {

  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly mode = input<'create' | 'edit'>('create');
  readonly isEditMode = computed(() => this.mode() === 'edit');
  readonly employee = input<Employee | null>(null);

  readonly loading = signal(false);

  readonly lookupData = signal({
    departments: [] as Department[],
    designations: [] as Designation[],
    managers: [] as Employee[]
  });

  private readonly departmentService = inject(DepartmentService);
  private readonly designationService = inject(DesignationService);
  private readonly employeeService = inject(EmployeeService);

  constructor() {

    effect(() => {

      const employee = this.employee();

      if (!employee) return;

      this.form.patchValue({
        firstName: employee.firstName,
        lastName: employee.lastName,
        email: employee.email,
        phone: employee.phone,
        joiningDate: employee.joiningDate,
        departmentId: employee.departmentId,
        designationId: employee.designationId,
        managerId: employee.managerId
      });
    });
  }

  ngOnInit(): void {
    this.loadLookupData();
  }

  readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]{10,20}$/)]],
    joiningDate: ['', Validators.required],
    departmentId: [0, Validators.min(1)],
    designationId: [0, Validators.min(1)],
    managerId: [null as number | null]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    const request = this.form.getRawValue();
    if(!this.isEditMode()){
      this.createEmployee(request);
    }
    else{
      this.updateEmployee(request);
    }
  }

  cancel(): void {
    this.router.navigate(['/employees']);
  }

  private loadLookupData(): void {

    this.loading.set(true);

    forkJoin({
      departments: this.departmentService.getDepartments(),
      designations: this.designationService.getDesignations(),
      managers: this.employeeService.getManagers()

    })
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: ({
          departments,
          designations,
          managers
        }) => {
          this.lookupData.set({
            departments,
            designations,
            managers
          }
          )
        },
        error: error => {
          console.error(error);
          // TODO:
          // NotificationService
        }
      });
  }

  private createEmployee(request: CreateEmployeeRequest): void {
    this.loading.set(true);
    this.employeeService.create(request)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.router.navigate(['/employees']);
        },
        error: error => {
          console.error(error);
        }
      });
  }

  private updateEmployee(request: UpdateEmployeeRequest): void {

    const employeeId = this.employee()?.id;

    if (!employeeId) {
      return;
    }

    this.loading.set(true);
    this.employeeService.update(employeeId, request)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.router.navigate(['/employees']);
        },
        error: error => {
          console.error(error);
        }
      });
  }
}
