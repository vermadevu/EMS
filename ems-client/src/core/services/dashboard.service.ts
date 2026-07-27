import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { environment } from '../../environments/environment';
import { Dashboard } from '../../features/dashboard/models/dashboard';

@Service()
export class DashboardService {
    private readonly http = inject(HttpClient);
    
    getDashboard() {
    return this.http.get<Dashboard>(
        `${environment.apiUrl}${API_ENDPOINTS.dashboard}`
    );
}
}
