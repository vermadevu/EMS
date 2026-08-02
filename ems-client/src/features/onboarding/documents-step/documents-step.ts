import {
  Component,
  inject,
  OnInit,
  signal
} from '@angular/core';

import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { DocumentService } from '../../../core/services/document-service';
import { NotificationService } from '../../../core/services/notification-service';

import { Document } from '../../../core/models/document';
import { DocumentType } from '../../../core/models/document-type';
import { RequiredDocument } from '../../../core/models/required-document';
import { MatIconModule } from '@angular/material/icon';
import { OnboardingService } from '../../../core/services/onboarding-service';

@Component({
  selector: 'app-documents-step',
  imports: [
    MatIconModule
  ],
  templateUrl: './documents-step.html',
  styleUrl: './documents-step.css'
})
export class DocumentsStepComponent implements OnInit {
  readonly loading = signal(false);
  readonly documents = signal<Document[]>([]);
  private readonly documentService = inject(DocumentService);
  private readonly notificationService = inject(NotificationService);
  private readonly onboardingService = inject(OnboardingService);
  private readonly router = inject(Router);


  readonly requiredDocuments: RequiredDocument[] = [

    {
      type: DocumentType.Resume,
      label: 'Resume'
    },
    {
      type: DocumentType.Aadhaar,
      label: 'Aadhaar Card'
    },
    {
      type: DocumentType.PAN,
      label: 'PAN Card'
    },
    {
      type: DocumentType.Degree,
      label: 'Degree'
    }
  ];

  readonly selectedFiles = signal<Record<number, File>>({});

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments(): void {
    this.loading.set(true);
    this.documentService
      .getMyDocuments()
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: documents => {
          this.documents.set(documents);
        }
      });
  }

  continue(): void {
    const completed = this.requiredDocuments.every(x =>
      this.hasDocument(x.type)
    );

    if (!completed) {
      this.notificationService.error(
        'Please upload all required documents.'
      );
      return;
    }
    this.onboardingService.complete('/onboarding/documents');

    this.router.navigate([
      '/onboarding/assets'
    ]);
  }

  onFileSelected(event: Event, type: DocumentType): void {
    const file = (event.target as HTMLInputElement)
      .files?.[0];

    if (!file) {
      return;
    }
    this.selectedFiles.update(files => ({
      ...files,
      [type]: file
    }));
  }

  getSelectedFile(type: DocumentType): File | undefined {
    return this.selectedFiles()[type];
  }

  upload(type: DocumentType): void {
    const file = this.getSelectedFile(type);

    if (!file) {
      this.notificationService.error(
        'Please select a file.'
      );
      return;
    }
    const formData = new FormData();

    formData.append('file', file);
    console.log(type.toString());
    formData.append('documentType', type.toString());
    this.loading.set(true);
    this.documentService
      .uploadMyDocument(formData)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Document uploaded successfully.'
          );
          this.selectedFiles.update(files => {
            const updated = { ...files };
            delete updated[type];
            return updated;
          });
          this.loadDocuments();
        }
      });
  }

  hasDocument(type: DocumentType): boolean {
    return this.documents()
      .some(x => x.documentType === DocumentType[type]);
  }

  getDocument(type: DocumentType): Document | undefined {
    return this.documents()
      .find(x => x.documentType === DocumentType[type]);
  }

  deleteDocument(id: number): void {
    this.documentService
      .deleteMyDocument(id)
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Document deleted successfully.'
          );
          this.loadDocuments();
        }
      });
  }
}