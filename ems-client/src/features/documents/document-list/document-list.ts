import { Component, inject, input, OnInit, signal } from '@angular/core';
import { DocumentService } from '../../../core/services/document-service';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { EmployeeDocumentSummary } from '../../../core/models/employee-document-summary';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { DocumentTableComponent } from '../document-table/document-table';
import { SearchBox } from '../../../shared/components/search-box/search-box';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';
import { PagedResult } from '../../employees/models/paged-result';
import { debounceTime, distinctUntilChanged, finalize, Subject } from 'rxjs';
import { DocumentListState } from '../../../core/models/document-list-state';

@Component({
  selector: 'app-document-list',
  imports: [
    MatIconModule,
    PageHeaderComponent,
    DocumentTableComponent,
    PaginationComponent,
  ],
  templateUrl: './document-list.html',
  styleUrl: './document-list.css',
})
export class DocumentListComponent implements OnInit {
  private readonly documentService = inject(DocumentService);
  private readonly router = inject(Router);

  readonly loading = signal(false);

  readonly employees = signal<EmployeeDocumentSummary[]>([]);

  readonly page = signal<PagedResult<EmployeeDocumentSummary> | null>(null);

  private readonly searchSubject = new Subject<string>();

  readonly state = signal<DocumentListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'fullName',
    sortDirection: 'asc'
  });

  constructor() {

    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {

        this.state.update(state => ({
          ...state,
          search,
          pageNumber: 1
        }));

        this.loadEmployees();

      });

  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {

    this.loading.set(true);

    this.documentService
      .getEmployeeSummary(this.state())
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: result => {
          this.page.set(result);
          this.employees.set(result.items);
        },
        error: console.error
      });
  }

  search(value: string): void {

    this.searchSubject.next(value);

  }

  changePage(pageNumber: number): void {

    this.state.update(state => ({
      ...state,
      pageNumber
    }));

    this.loadEmployees();

  }

  sort(column: string): void {

    const current = this.state();

    const direction =
      current.sortBy === column &&
        current.sortDirection === 'asc'
        ? 'desc'
        : 'asc';

    this.state.update(state => ({
      ...state,
      sortBy: column,
      sortDirection: direction
    }));

    this.loadEmployees();

  }

  open(employeeId: number): void {

    this.router.navigate([
      '/documents',
      employeeId
    ]);

  }
}