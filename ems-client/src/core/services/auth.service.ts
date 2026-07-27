import { HttpClient } from '@angular/common/http';
import { computed, inject, Service } from '@angular/core';
import { Router } from '@angular/router';
import { tap, switchMap, EMPTY, catchError, of } from 'rxjs';
import { TokenService } from './token-service';
import { LoginRequest } from '../authentication/login-request.model';
import { LoginResponse } from '../authentication/long-response.model';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import { environment } from '../../environments/environment';
import { CurrentUser } from '../authentication/current-user.model';
import { CurrentUserService } from './current-user.service';

@Service()
export class AuthService {

    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);

    private readonly tokenService = inject(TokenService);
    private readonly currentUserService = inject(CurrentUserService);

    readonly isAuthenticated = computed(() =>
        this.currentUserService.isAuthenticated()
    );

    login(request: LoginRequest) {

        return this.http.post<LoginResponse>(
            `${environment.apiUrl}${API_ENDPOINTS.account.login}`,
            request
        ).pipe(

            tap(response => {
                this.tokenService.save(response.token);
            }),

            switchMap(() =>
                this.http.get<CurrentUser>(
                    `${environment.apiUrl}${API_ENDPOINTS.account.me}`
                )
            ),

            tap(user => {
                this.currentUserService.setUser(user);
            })
        );
    }

    logout(): void {

        this.tokenService.remove();

        this.currentUserService.clear();

        this.router.navigate(['/login']);
    }

    restoreSession() {

        if (!this.tokenService.hasToken) {
            return of(null);
        }

        return this.http.get<CurrentUser>(
            `${environment.apiUrl}${API_ENDPOINTS.account.me}`
        ).pipe(

            tap(user => {
            this.currentUserService.setUser(user);
            }),

            catchError(() => {

            this.tokenService.remove();
            this.currentUserService.clear();

            return of(null);
    })
  );
}

}