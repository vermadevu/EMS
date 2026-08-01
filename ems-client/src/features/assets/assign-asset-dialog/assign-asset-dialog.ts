import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { EmployeeService } from '../../../core/services/employee-service';
import { AssetService } from '../../../core/services/asset-service';
import { NotificationService } from '../../../core/services/notification-service';

import { EmployeeListItem } from '../../../shared/models/employee-list-item';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-assign-asset-dialog',
  imports: [
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './assign-asset-dialog.html',
  styleUrl: './assign-asset-dialog.css'
})
export class AssignAssetDialogComponent {

  private readonly fb = inject(FormBuilder);

  private readonly employeeService = inject(EmployeeService);
  private readonly assetService = inject(AssetService);
  private readonly notificationService = inject(NotificationService);

  readonly dialogRef = inject(MatDialogRef<AssignAssetDialogComponent>);
  readonly data = inject(MAT_DIALOG_DATA);

  employees: EmployeeListItem[] = [];

  readonly form = this.fb.nonNullable.group({
    employeeId: [0, Validators.required]
  });

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees() {
    return this.employeeService
      .getAll()
      .subscribe({
        next: employees =>{
          console.log(employees)
           this.employees = employees
          }
      });

  }

  assign(): void {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.assetService
      .assign(
        this.data.assetId,
        this.form.value.employeeId!
      )
      .subscribe({

        next: () => {

          this.notificationService.success(
            'Asset assigned successfully.'
          );

          this.dialogRef.close(true);

        }

      });

  }

  close(): void {
    this.dialogRef.close(false);
  }

}