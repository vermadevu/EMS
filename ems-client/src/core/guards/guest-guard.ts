import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserService } from '../services/current-user.service';


export const guestGuard: CanActivateFn = () => {

  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  if (!currentUser.isAuthenticated()) {
    return true;
  }

  return router.createUrlTree(['/dashboard']);

};