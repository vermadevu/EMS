import { Component, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { EmployeeService } from '../../../core/services/employee-service';
import { ActivatedRoute } from '@angular/router';
import { Employee } from '../models/employee';
import { finalize } from 'rxjs';
import { DetailItemComponent } from '../../../shared/components/detail-item/detail-item';
import { MatIconModule } from '@angular/material/icon';
import { StatusBadge } from '../../../shared/components/status-badge/status-badge';
import { DatePipe } from '@angular/common';
import { AssetService } from '../../../core/services/asset-service';
import { DocumentService } from '../../../core/services/document-service';
import { Asset } from '../../../core/models/asset';
import { Document } from '../../../core/models/document';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-employee-details',
  imports: [
    DetailItemComponent,
    PageHeaderComponent,
    MatIconModule,
    StatusBadge,
    DatePipe
  ],
  templateUrl: './employee-details.html',
  styleUrl: './employee-details.css',
})
export class EmployeeDetailsComponent {

  private readonly confirmationService = inject(ConfirmationService);
  private readonly employeeService = inject(EmployeeService);
  private readonly notificationService = inject(NotificationService);  private readonly assetService = inject(AssetService);
  private readonly documentService = inject(DocumentService);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal(true);
  readonly employee = signal<Employee | null>(null);

  readonly assets = signal<Asset[]>([]);
  readonly documents = signal<Document[]>([]);

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.employeeService.getById(id)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: employee => {
          this.employee.set(employee);
          this.loadDocuments(employee);
          this.loadAssets(employee);
        },
        error: console.error
      });
  }

  loadDocuments(employee: Employee) {
    this.documentService
      .getByEmployee(employee.id)
      .subscribe({
        next: documents => this.documents.set(documents)
      });
  }

  loadAssets(employee: Employee) {
    this.assetService
      .getByEmployee(employee.id)
      .subscribe({
        next: assets => this.assets.set(assets)
      });
  }



    approve(employee: Employee) {
      this.confirmationService.confirm({
        title: 'Approve Employee',
        message: `Approve onboarding for ${employee.fullName}?`,
        icon: 'task_alt',
        confirmText: 'Approve',
        confirmButtonClass: 'btn-success'
      })
        .subscribe(confirmed => {
          if (!confirmed) {
            return;
          }
  
          this.employeeService.activate(employee.id).subscribe({
            next: () => {
              this.notificationService.success(
                'Employee approved successfully.'
              );
            },
            error: console.error
          });
        });
    }
}
