import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { Department } from '../models/department';
import { DepartmentListState } from '../models/department-list-state';
import { DepartmentListItem } from '../models/department-list-item';
import { PagedResult } from '../../features/employees/models/paged-result';
import { Observable } from 'rxjs';
import { CreateDepartment } from '../models/create-department';
import { UpdateDepartment } from '../models/update-department';

@Service()
export class DepartmentService {
    private readonly http = inject(HttpClient);

    getDepartments() {
        return this.http.get<Department[]>(`${environment.apiUrl}${API_ENDPOINTS.departments}/all`);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${environment.apiUrl}${API_ENDPOINTS.departments}/${id}`);
    }

    create(request: CreateDepartment) {
        return this.http.post<Department>(`${environment.apiUrl}${API_ENDPOINTS.departments}`, request);
    }

    update(id: number, request: UpdateDepartment) {
        return this.http.put<void>(`${environment.apiUrl}${API_ENDPOINTS.departments}/${id}`, request);
    }

    getById(id: number) {
        return this.http.get<Department>(`${environment.apiUrl}${API_ENDPOINTS.departments}/${id}`);

    }

    getDepartmentsPaged(query: DepartmentListState) {

        let params = new HttpParams();

        if (query.pageNumber !== 1) {
            params = params.set('pageNumber', query.pageNumber);
        }

        if (query.pageSize !== 10) {
            params = params.set('pageSize', query.pageSize);
        }

        if (query.search) {
            params = params.set('search', query.search);
        }

        if (query.sortBy !== 'name') {
            params = params.set('sortBy', query.sortBy);
        }

        if (query.sortDirection !== 'asc') {
            params = params.set('sortDirection', query.sortDirection);
        }

        return this.http.get<PagedResult<DepartmentListItem>>(
            `${environment.apiUrl}${API_ENDPOINTS.departments}`,
            { params }
        );
    }
}
