import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CurrentUserService } from '../services/current-user.service';

export const authGuard: CanActivateFn = (route, state) => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  if(currentUser.isAuthenticated()){
    return true;
  }
  return router.navigateByUrl('/login');
};
