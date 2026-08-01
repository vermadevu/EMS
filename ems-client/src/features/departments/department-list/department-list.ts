import { Component, inject, OnInit, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PagedResult } from '../../employees/models/paged-result';
import { DepartmentListItem } from '../../../core/models/department-list-item';
import { DepartmentListState } from '../../../core/models/department-list-state';
import { finalize, Subject } from 'rxjs';
import { DepartmentService } from '../../../core/services/department-service';
import { DepartmentTableComponent } from '../department-table/department-table';
import { DepartmentToolbarComponent } from '../department-toolbar/department-toolbar';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';
import { Router } from '@angular/router';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

@Component({
  selector: 'app-department-list',
  imports: [
    PageHeaderComponent,
    DepartmentTableComponent,
    DepartmentToolbarComponent,
    PaginationComponent
  ],
  templateUrl: './department-list.html',
  styleUrl: './department-list.css',
})
export class DepartmentListComponent implements OnInit {
  readonly loading = signal(false);
  readonly page = signal<PagedResult<DepartmentListItem> | null>(null);
  readonly departments = signal<DepartmentListItem[]>([]);
  private readonly searchSubject = new Subject<string>();

  private readonly router = inject(Router);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly notificationService = inject(NotificationService);
  private readonly departmentService = inject(DepartmentService);

  readonly state = signal<DepartmentListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'name',
    sortDirection: 'asc'
  });

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments() {
    this.loading.set(true);

    this.departmentService
      .getDepartmentsPaged(this.state())
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe(result => {
        this.page.set(result);
        this.departments.set(result.items);
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
    this.loadDepartments();
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
    this.loadDepartments();
  }

  handleAction(event: {
    action: string;
    department: DepartmentListItem;
  }): void {

    switch (event.action) {

      case 'edit':
        this.router.navigate([
          '/departments/edit',
          event.department.id
        ]);
        break;

      case 'delete':
        this.confirmationService
          .confirm({
            title: 'Delete Department',
            message: `Are you sure you want to delete ${event.department.name}?`,
            confirmText: 'Delete',
            confirmButtonClass: 'btn-error'
          })
          .subscribe(result => {
            if (!result) {
              return;
            }
            this.departmentService
              .delete(event.department.id)
              .subscribe({
                next: () => {
                  this.notificationService.success(
                    'Department deleted successfully.'
                  );
                  this.loadDepartments();
                },
                error: console.error
              });
          });
        break;
    }
  }
}
