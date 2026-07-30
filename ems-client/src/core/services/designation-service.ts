import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Department } from '../models/department';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';

@Service()
export class DesignationService {
    private readonly http = inject(HttpClient);

    getDesignations(){
        return this.http.get<Department[]>(`${environment.apiUrl}${API_ENDPOINTS.designations}`);
    }
}
