import { Component, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { EmployeeToolbarComponent } from '../employee-toolbar/employee-toolbar';
import { EmployeeTableComponent } from '../employee-table/employee-table';
import { EmployeeListState } from '../models/employee-list-state';
import { EmployeeService } from '../../../core/services/employee-service';
import { EmployeeListItem } from '../../../shared/models/employee-list-item';
import { PagedResult } from '../models/paged-result';
import { debounceTime, distinctUntilChanged, finalize, Subject } from 'rxjs';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';
import { DepartmentService } from '../../../core/services/department-service';
import { Department } from '../../../core/models/department';
import { StatusOption } from '../../../core/models/status-option';
import { DesignationService } from '../../../core/services/designation-service';

@Component({
  selector: 'app-employee-list',
  imports: [
    PageHeaderComponent,
    EmployeeToolbarComponent,
    EmployeeTableComponent,
    PaginationComponent
  ],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeListComponent {

  readonly loading = signal(false);
  readonly page = signal<PagedResult<EmployeeListItem> | null>(null);
  private readonly searchSubject = new Subject<string>();

  private readonly employeeService = inject(EmployeeService);
  readonly employees = signal<EmployeeListItem[]>([]);


  private readonly designationService = inject(DesignationService);
  readonly designations = signal<Department[]>([]);

  private readonly departmentService = inject(DepartmentService);
  readonly departments = signal<Department[]>([]);

  readonly statuses = signal<StatusOption[]>([]);

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
    this.loadDepartments();
    this.loadEmployees();
    this.loadStatuses();
    this.loadDesignations();
  }

  readonly state = signal<EmployeeListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'joiningDate',
    sortDirection: 'desc'
  });


  loadEmployees(): void {
    this.loading.set(true);
    this.employeeService
      .getEmployees(this.state())
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

  private loadDepartments(): void {
    this.departmentService
      .getDepartments()
      .subscribe({
        next: departments =>
          this.departments.set(departments)
      });
  }

  private loadDesignations(): void {
    this.designationService
      .getDesignations()
      .subscribe({
        next: designations =>
          this.designations.set(designations)
      });
  }

  private loadStatuses(): void {
    this.employeeService
      .getStatuses()
      .subscribe({
        next: statuses => this.statuses.set(statuses)
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


  changeDepartment(departmentId?: number): void {
    this.updateState({ departmentId });
  }

  changeStatus(status?: string): void {
    this.updateState({ status });
  }

  changeDesignation(designationId?: number): void {
    this.updateState({ designationId });
  }


  private updateState(changes: Partial<EmployeeListState>): void {
    this.state.update(state => ({
      ...state,
      ...changes,
      pageNumber: 1
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
  this.updateState({
    sortBy: column,
    sortDirection: direction
  });
}

}