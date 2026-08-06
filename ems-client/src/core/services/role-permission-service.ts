import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { HttpClient } from '@angular/common/http';
import { RoleListItem } from '../models/role-list-item';
import { RolePermissionDetails } from '../models/role-permission-details';

@Service()
export class RolePermissionService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl =
        `${environment.apiUrl}${API_ENDPOINTS.rolePermissions}`;

    getRoles() {
        return this.http.get<RoleListItem[]>(`${this.baseUrl}/roles`);
    }

    getRolePermissions(roleId: string) {
        return this.http.get<RolePermissionDetails>(`${this.baseUrl}/${roleId}`);
    }

    updateRolePermissions(roleId: string, permissionIds: number[]) {
        return this.http.put(`${this.baseUrl}/${roleId}`, { permissionIds });
    }
}
