import { Component, inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NotificationService } from '../../../core/services/notification-service';

export interface UserCreatedDialogData {
  employeeName: string;
  username: string;
  temporaryPassword: string;
}

@Component({
  selector: 'app-user-created-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './user-created-dialog.html',
  styleUrl: './user-created-dialog.css'
})
export class UserCreatedDialogComponent {

  readonly data = inject<UserCreatedDialogData>(MAT_DIALOG_DATA);
  private readonly dialogRef = inject(MatDialogRef<UserCreatedDialogComponent>);
  private readonly notificationService = inject(NotificationService);


  copyPassword(): void {

    navigator.clipboard.writeText(
      this.data.temporaryPassword
    );

    this.notificationService.success(
      'Password copied to clipboard.'
    );
  }

  close(): void {
    this.dialogRef.close();
  }

}