import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DOCUMENT_TYPES, DocumentType } from '../../../core/models/document-type';
import { DocumentService } from '../../../core/services/document-service';
import { NotificationService } from '../../../core/services/notification-service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UploadDocumentDialogData } from '../../../core/models/upload-document-dialog-data';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-upload-document-dialog',
  imports: [
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './upload-document-dialog.html',
  styleUrl: './upload-document-dialog.css',
})
export class UploadDocumentDialogComponent {
  private readonly fb = inject(FormBuilder);

  private readonly documentService = inject(DocumentService);

  private readonly notification = inject(NotificationService);

  readonly documentTypes = DOCUMENT_TYPES;

  readonly dialogRef =
    inject(MatDialogRef<UploadDocumentDialogComponent>);

  readonly data =
    inject<UploadDocumentDialogData>(MAT_DIALOG_DATA);

  selectedFile: File | null = null;

  readonly form = this.fb.nonNullable.group({

    documentType: [
      DocumentType.Resume,
      Validators.required
    ]
  });

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) {
      return;
    }
    this.selectedFile = input.files[0];
  }

  submit(): void {
    if (!this.selectedFile) {
      this.notification.error(
        'Please select a file.'
      );
      return;
    }

    const formData = new FormData();

    formData.append(
      'employeeId',
      this.data.employeeId.toString()
    );

    formData.append(
      'documentType',
      this.form.controls.documentType.value.toString()
    );

    formData.append(
      'file',
      this.selectedFile
    );

    this.documentService
      .upload(formData)
      .subscribe({
        next: document => {
          this.notification.success(
            'Document uploaded successfully.'
          );
          this.dialogRef.close(document);
        }
      });
  }
}

