import { inject, Service } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogData } from '../../shared/models/confirm-dialog-data';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';

@Service()
export class ConfirmationService {
    private readonly dialog = inject(MatDialog);

    confirm(data: ConfirmDialogData): Observable<boolean> {

        return this.dialog.open(ConfirmDialogComponent, {
            width: '480px',
            maxWidth: '95vw',
            disableClose: true,
            autoFocus: false,
            restoreFocus: false,
            panelClass: 'confirm-dialog-panel',
            backdropClass: 'confirm-dialog-backdrop',
            data
        }).afterClosed();
    }
}
