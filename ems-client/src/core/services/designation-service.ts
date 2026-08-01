import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Designation } from '../models/designation';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { CreateDesignation } from '../models/create-designation';
import { UpdateDesignation } from '../models/update-desgination';
import { PagedResult } from '../../features/employees/models/paged-result';
import { DesignationListItem } from '../models/designation-list-item';
import { DesignationListState } from '../models/designation-list-state';

@Service()
export class DesignationService {
    private readonly http = inject(HttpClient);

    getDesignations() {
        return this.http.get<Designation[]>(`${environment.apiUrl}${API_ENDPOINTS.designations}/all`);
    }
    delete(id: number) {
        return this.http.delete<void>(`${environment.apiUrl}${API_ENDPOINTS.designations}/${id}`);
    }

    create(request: CreateDesignation) {
        return this.http.post<Designation>(`${environment.apiUrl}${API_ENDPOINTS.designations}`, request);
    }

    update(id: number, request: UpdateDesignation) {
        return this.http.put<void>(`${environment.apiUrl}${API_ENDPOINTS.designations}/${id}`, request);
    }

    getById(id: number) {
        return this.http.get<Designation>(`${environment.apiUrl}${API_ENDPOINTS.designations}/${id}`);

    }

    getDesignationsPaged(query: DesignationListState) {
        let params = new HttpParams();

        if (query.pageNumber !== 1)
            params = params.set('pageNumber', query.pageNumber);

        if (query.pageSize !== 10)
            params = params.set('pageSize', query.pageSize);

        if (query.search)
            params = params.set('search', query.search);

        if (query.sortBy !== 'name')
            params = params.set('sortBy', query.sortBy);

        if (query.sortDirection !== 'asc')
            params = params.set('sortDirection', query.sortDirection);

        return this.http.get<PagedResult<DesignationListItem>>(
            `${environment.apiUrl}${API_ENDPOINTS.designations}`,
            { params }
        );
    }
}
