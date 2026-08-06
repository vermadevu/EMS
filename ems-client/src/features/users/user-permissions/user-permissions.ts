import { Component, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { Subject } from 'rxjs';
import { EmployeeListState } from '../../employees/models/employee-list-state';

@Component({
  selector: 'app-user-permissions',
  imports: [
    PageHeaderComponent
  ],
  templateUrl: './user-permissions.html',
  styleUrl: './user-permissions.css',
})
export class UserPermissionsComponent {

  private readonly searchSubject = new Subject<string>();

  readonly state = signal<EmployeeListState>({
    pageNumber: 1,
    pageSize: 10,
    search: '',
    sortBy: 'fullName',
    sortDirection: 'asc'
  });


  search(value: string): void {
    this.searchSubject.next(value);
  }
}
