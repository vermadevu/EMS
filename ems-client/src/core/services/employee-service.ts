import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { EmployeeListState } from '../../features/employees/models/employee-list-state';
import { PagedResult } from '../../features/employees/models/paged-result';
import { EmployeeListItem } from '../../shared/models/employee-list-item';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { environment } from '../../environments/environment';

@Service()
export class EmployeeService {

  private readonly http = inject(HttpClient);

  getEmployees(query: EmployeeListState) {
    let params = new HttpParams();


    if (query.pageNumber !== 1) {
      params = params.set('pageNumber', query.pageNumber);
    }

    if (query.pageSize !== 10) {
      params = params.set('pageSize', query.pageSize);
    }

    if (query.sortBy !== 'joiningDate') {
      params = params.set('sortBy', query.sortBy);
    }

    if (query.sortDirection !== 'desc') {
      params = params.set('sortDirection', query.sortDirection);
    }

    if (query.search) {
      params = params.set('search', query.search);
    }

    if (query.departmentId) {
      params = params.set('departmentId', query.departmentId);
    }

    if (query.designationId) {
      params = params.set('designationId', query.designationId);
    }

    if (query.status) {
      params = params.set('status', query.status);
    }

    return this.http.get<PagedResult<EmployeeListItem>>(
      `${environment.apiUrl}${API_ENDPOINTS.employees}`,
      { params }
    );

  }

}