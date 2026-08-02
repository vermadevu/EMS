import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const onboardingGuard: CanActivateFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const user = authService.currentUser();

    if (!user) {
        router.navigate(['/login']);
        return false;
    }

    if (user.employeeStatus !== 'Pending' && user.employeeStatus !== 'Inactive') {
        router.navigate(['/dashboard']);
        return false;
    }

    return true;
};
