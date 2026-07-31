import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Designation } from '../models/designation';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';

@Service()
export class DesignationService {
    private readonly http = inject(HttpClient);

    getDesignations(){
        return this.http.get<Designation[]>(`${environment.apiUrl}${API_ENDPOINTS.designations}`);
    }
}
