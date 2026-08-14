import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenService } from '../services/token-service';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const tokenService = inject(TokenService);
  const authService = inject(AuthService);

  const token = tokenService.token;

  if (token) {

    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (
        error.status !== 401 ||
        req.url.includes('/account/login') ||
        req.url.includes('/account/refresh')
      ) {
        return throwError(() => error);
      }

      return authService.refreshToken().pipe(

        switchMap(() => {
          const newToken = tokenService.token!;
          const cloned = req.clone({
            setHeaders: {
              Authorization: `Bearer ${newToken}`
            }
          });
          return next(cloned);
        }),

        catchError(err => {
          authService.logout();
          return throwError(() => err);
        })
      );
    })
  );
};