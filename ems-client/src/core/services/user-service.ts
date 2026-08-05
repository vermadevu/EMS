import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { AvailableEmployee } from '../models/available-employee';
import { CreateUserRequest } from '../models/create-user-request';
import { CreateUserResponse } from '../models/create-user-response';
import { PagedResult } from '../../features/employees/models/paged-result';
import { UserQueryParams } from '../models/user-query-params';
import { UserListItem } from '../models/user-list-item';

@Service()
export class UserService {
    readonly http = inject(HttpClient);

    readonly baseUrl = environment.apiUrl + API_ENDPOINTS.users;

    getAvailableEmployees() {
        return this.http.get<AvailableEmployee[]>(
            `${this.baseUrl}/available-employees`
        );
    }

    getRoles() {
        return this.http.get<any[]>(
            `${this.baseUrl}/roles`
        );
    }

    create(user: CreateUserRequest) {
        return this.http.post<CreateUserResponse>(
            `${this.baseUrl}`, user
        )
    }

    getUsers(query: UserQueryParams) {

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

        if (query.role) {
            params = params.set('role', query.role);
        }

        if (query.isActive !== undefined && query.isActive !== null) {
            params = params.set('isActive', query.isActive);
        }

        return this.http.get<PagedResult<UserListItem>>(
            this.baseUrl,
            { params }
        );
    }

    activate(id: string) {
        return this.http.patch(
            `${this.baseUrl}/${id}/activate`,
            {}
        );
    }

    deactivate(id: string) {
        return this.http.patch(
            `${this.baseUrl}/${id}/deactivate`,
            {}
        );
    }
}
