import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const dashboardGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
    const router = inject(Router);

    const user = authService.currentUser();

    if (!user) {
        router.navigate(['/login']);
        return false;
    }
    if (user.employeeStatus === 'Pending') {
        router.navigate(['/onboarding']);
        return false;
    }

    return true;
};
