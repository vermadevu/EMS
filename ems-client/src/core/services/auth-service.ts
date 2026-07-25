import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { TokenService } from './token-service';
import { LoginRequest } from '../authentication/login-request.model';
import { Observable, tap } from 'rxjs';
import { LoginResponse } from '../authentication/long-response.model';
import { environment } from '../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints';

@Service()
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly tokenService = inject(TokenService);
    private baseUrl = environment.apiUrl;
    readonly isAuthenticated = signal(this.tokenService.hasToken);

    readonly loggedIn = computed(() => this.isAuthenticated());

    login(request: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(
            `${this.baseUrl}${API_ENDPOINTS.account.login}`,
            request
        ).pipe(
            tap(response => {
                this.tokenService.save(response.token);
                this.isAuthenticated.set(true);
            })
        );
    }

    logout(): void {
        this.tokenService.remove();
        this.isAuthenticated.set(false);
    }
}
