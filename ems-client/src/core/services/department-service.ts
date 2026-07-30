import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { Department } from '../models/department';

@Service()
export class DepartmentService {
    private readonly http = inject(HttpClient);

    getDepartments() {
        return this.http.get<Department[]>(`${environment.apiUrl}${API_ENDPOINTS.departments}`);
    }
    
}
