import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { firstValueFrom } from 'rxjs';

export async function authInitializer() {

  const authService = inject(AuthService);

  await firstValueFrom(authService.restoreSession());
}