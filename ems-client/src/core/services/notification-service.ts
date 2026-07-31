import { inject, Service } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';


@Service()
export class NotificationService {
     private readonly snackBar = inject(MatSnackBar);

  success(message: string): void {

    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      panelClass: ['snackbar-success']
    });

  }

  error(message: string): void {

    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['snackbar-error']
    });

  }

  info(message: string): void {

    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });

  }
}
