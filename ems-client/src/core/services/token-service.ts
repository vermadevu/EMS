import { inject, Service } from '@angular/core';

@Service()
export class TokenService {

  private readonly storage = localStorage;

  private readonly TOKEN_KEY = 'access_token';

  get token(): string | null {
    return this.storage.getItem(this.TOKEN_KEY);
  }

  save(token: string): void {
    this.storage.setItem(this.TOKEN_KEY, token);
  }

  remove(): void {
    this.storage.removeItem(this.TOKEN_KEY);
  }

  clear(): void {
    this.storage.clear();
  }

  get hasToken(): boolean {
    return !!this.token;
  }
}