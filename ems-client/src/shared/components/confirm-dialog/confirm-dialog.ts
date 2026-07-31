import { Component, inject } from '@angular/core';
import { ConfirmDialogData } from '../../models/confirm-dialog-data';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { NgClass } from '@angular/common';


@Component({
  selector: 'app-confirm-dialog',
  imports: [
    MatDialogModule,
    MatIconModule,
    NgClass
  ],
  templateUrl: './confirm-dialog.html',
  styleUrl: './confirm-dialog.css',
})
export class ConfirmDialogComponent {
  readonly dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);
  readonly data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);

  confirm(): void {
    this.dialogRef.close(true);
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
