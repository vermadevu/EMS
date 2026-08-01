import { Component, inject, OnInit, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PagedResult } from '../../employees/models/paged-result';
import { DesignationListItem } from '../../../core/models/designation-list-item';
import { DesignationListState } from '../../../core/models/designation-list-state';
import { finalize, Subject } from 'rxjs';
import { DesignationService } from '../../../core/services/designation-service';
import { DesignationTableComponent } from '../designation-table/designation-table';
import { DesignationToolbarComponent } from '../designation-toolbar/designation-toolbar';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';
import { Router } from '@angular/router';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-designation-list',
  imports: [
    PageHeaderComponent,
    DesignationTableComponent,
    DesignationToolbarComponent,
    PaginationComponent
  ],
  templateUrl: './designation-list.html',
  styleUrl: './designation-list.css',
})
export class DesignationListComponent implements OnInit {
  readonly loading = signal(false);
  readonly page = signal<PagedResult<DesignationListItem> | null>(null);
  readonly designations = signal<DesignationListItem[]>([]);
  private readonly searchSubject = new Subject<string>();

  private readonly router = inject(Router);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly notificationService = inject(NotificationService);
  private readonly designationService = inject(DesignationService);

  readonly state = signal<DesignationListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'name',
    sortDirection: 'asc'
  });

  ngOnInit(): void {
    this.loadDesignations();
  }

  loadDesignations() {
    this.loading.set(true);

    this.designationService
      .getDesignationsPaged(this.state())
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe(result => {
        this.page.set(result);
        this.designations.set(result.items);
      });
  }

  search(value: string): void {
    this.searchSubject.next(value);
  }

  changePage(pageNumber: number) {
    this.state.update(state => ({
      ...state,
      pageNumber
    }));
    this.loadDesignations();
  }

  sort(column: string) {
    const current = this.state();
    const direction =
      current.sortBy === column &&
        current.sortDirection === 'asc'
        ? 'desc'
        : 'asc';
    this.state.update(state => ({
      ...state,
      sortBy: column,
      sortDirection: direction,
      pageNumber: 1
    }));
    this.loadDesignations();
  }

  handleAction(event: {
    action: string;
    designation: DesignationListItem;
  }): void {

    switch (event.action) {

      case 'edit':
        this.router.navigate([
          '/designations/edit',
          event.designation.id
        ]);
        break;

      case 'delete':
        this.confirmationService
          .confirm({
            title: 'Delete Designation',
            message: `Are you sure you want to delete ${event.designation.name}?`,
            confirmText: 'Delete',
            confirmButtonClass: 'btn-error'
          })
          .subscribe(result => {
            if (!result) {
              return;
            }
            this.designationService
              .delete(event.designation.id)
              .subscribe({
                next: () => {
                  this.notificationService.success(
                    'Designation deleted successfully.'
                  );
                  this.loadDesignations();
                },
                error: console.error
              });
          });
        break;
    }
  }
}
