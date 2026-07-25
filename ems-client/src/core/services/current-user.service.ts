import { computed, Service, signal } from '@angular/core';
import { CurrentUser } from '../authentication/current-user.model';

@Service()
export class CurrentUserService {

  readonly user = signal<CurrentUser | null>(null);

  readonly isAuthenticated = computed(() => this.user() !== null);

  readonly roles = computed(() => this.user()?.roles ?? []);

  readonly permissions = computed(() => this.user()?.permissions ?? []);

  setUser(user: CurrentUser): void {
    this.user.set(user);
  }

  clear(): void {
    this.user.set(null);
  }

  hasRole(role: string): boolean {
    return this.roles().includes(role);
  }

  hasPermission(permission: string): boolean {
    return this.permissions().includes(permission);
  }
}