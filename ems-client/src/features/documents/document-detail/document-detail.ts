import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee-service';
import { DocumentService } from '../../../core/services/document-service';
import { Employee } from '../../employees/models/employee';
import { forkJoin } from 'rxjs';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { MatIconModule } from '@angular/material/icon';
import { DatePipe } from '@angular/common';
import { Document } from '../../../core/models/document';
import { MatDialog } from '@angular/material/dialog';
import { UploadDocumentDialogComponent } from '../upload-document-dialog/upload-document-dialog';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-document-detail',
  imports: [
    PageHeaderComponent,
    MatIconModule,
    DatePipe
  ],
  templateUrl: './document-detail.html',
  styleUrl: './document-detail.css',
})
export class DocumentDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly employeeService = inject(EmployeeService);
  private readonly documentService = inject(DocumentService);
  private readonly confirmationService = inject(ConfirmationService);

  private readonly notificationService = inject(NotificationService);

  readonly employee = signal<Employee | null>(null);
  readonly documents = signal<Document[]>([]);
  readonly loading = signal(true);

  private readonly dialog = inject(MatDialog);

  constructor() {
    this.loadDocuments();
  }

  uploadDocument(): void {

    const dialogRef = this.dialog.open(
      UploadDocumentDialogComponent,
      {
        width: '500px',
        panelClass: 'confirm-dialog-panel',
        backdropClass: 'confirm-dialog-backdrop',
        data: {
          employeeId: this.employee()!.id
        }
      });

    dialogRef
      .afterClosed()
      .subscribe(result => {
        if (!result) {
          return;
        }
        this.loadDocuments();
      });
  }

  loadDocuments() {
    const employeeId = Number(
      this.route.snapshot.paramMap.get('employeeId')
    );

    forkJoin({
      employee: this.employeeService.getById(employeeId),
      documents: this.documentService.getByEmployee(employeeId)
    }).subscribe({
      next: ({ employee, documents }) => {
        this.employee.set(employee);
        this.documents.set(documents);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  //   loadDocuments() {
  //   const employeeId = Number(
  //     this.route.snapshot.paramMap.get('employeeId')
  //   );

  //   this.documentService
  //     .getByEmployee(employeeId)
  //     .subscribe({
  //       next: documents => {
  //         console.log(documents);
  //         this.documents.set(documents);
  //       }
  //     });
  // }


  viewDocument(document: Document): void {
    window.open(document.url, '_blank');
  }

  deleteDocument(document: Document): void {
    this.confirmationService.confirm({
      title: 'Delete Document',
      message: `Are you sure you want to delete "${document.originalFileName}"?`,
      icon: 'delete',
      confirmText: 'Delete',
      confirmButtonClass: 'btn-error'
    }).subscribe(confirmed => {
      if (!confirmed) {
        return;
      }
      this.documentService
        .delete(document.id)
        .subscribe({
          next: () => {
            this.notificationService.success(
              'Document deleted successfully.'
            );

            this.loadDocuments();
          }
        });
    });
  }
}

