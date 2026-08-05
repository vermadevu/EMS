import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../core/services/user-service';
import { AvailableEmployee } from '../../../core/models/available-employee';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { forkJoin } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { NotificationService } from '../../../core/services/notification-service';
import { MatDialog } from '@angular/material/dialog';
import { UserCreatedDialogComponent } from '../../../shared/components/user-created-dialog/user-created-dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule
  ],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css'
})
export class UserFormComponent implements OnInit {
  readonly employeeSearch = signal('');
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);
  readonly employees = signal<AvailableEmployee[]>([]);
  readonly loading = signal(false);
  readonly selectedEmployee = signal<AvailableEmployee | null>(null);
  private readonly notificationService = inject(NotificationService);
  readonly dialog = inject(MatDialog);
  private readonly router = inject(Router);


  readonly roles = signal<string[]>([]);
  readonly form = this.fb.nonNullable.group({
    employeeId: [0, Validators.min(1)],
    displayName: [{ value: '', disabled: true }],
    username: [{ value: '', disabled: true }],
    email: [{ value: '', disabled: true }],
    roles: [['Employee']]
  });

  readonly filteredEmployees = computed(() => {
    const search = this.employeeSearch()
      .trim()
      .toLowerCase();
    if (!search) {
      return this.employees();
    }
    return this.employees().filter(employee =>
      employee.fullName.toLowerCase().includes(search) ||
      employee.employeeCode.toLowerCase().includes(search)
    );
  });

  ngOnInit(): void {

    forkJoin({
      employees: this.userService.getAvailableEmployees(),
      roles: this.userService.getRoles()
    })
      .subscribe({
        next: ({ employees, roles }) => {
          this.employees.set(employees);
          this.roles.set(roles);
        }
      });
  }


  selectEmployee(employee: AvailableEmployee): void {
    this.form.patchValue({
      employeeId: employee.employeeId,
      displayName: employee.fullName,
      username: employee.email,
      email: employee.email
    });
    this.employeeSearch.set(
      `${employee.fullName} (${employee.employeeCode})`
    );
  }

  toggleRole(role: string, event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    const roles = [...this.form.controls.roles.value];
    if (checked) {
      if (!roles.includes(role)) {
        roles.push(role);
      }
    } else {
      const index = roles.indexOf(role);
      if (index >= 0) {
        roles.splice(index, 1);
      }
    }
    this.form.controls.roles.setValue(roles);
  }


  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.userService.create(this.form.getRawValue()).subscribe({
      next: response => {
        this.notificationService
          .success('User created successfully.');

        this.dialog.open(UserCreatedDialogComponent, {
          width: '500px',
          disableClose: true,
          data: {
            employeeName: this.form.controls.displayName.value,
            username: response.username,
            temporaryPassword: response.temporaryPassword
          }
        })
          .afterClosed()
          .subscribe(() => {
            this.router.navigate(['/users']);
          });
      },
      error: console.error
    });
  }
}