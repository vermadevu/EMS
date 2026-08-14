import { Service } from '@angular/core';

@Service()
export class TokenService {

  private readonly storage = localStorage;

  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';

  get token(): string | null {
    return this.storage.getItem(this.ACCESS_TOKEN_KEY);
  }

  get refreshToken(): string | null {
    return this.storage.getItem(this.REFRESH_TOKEN_KEY);
  }

  saveAccessToken(accessToken: string): void {
    this.storage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
  }

  saveRefreshToken(refreshToken: string): void {
    this.storage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  saveTokens(accessToken: string, refreshToken: string): void {
    this.saveAccessToken(accessToken);
    this.saveRefreshToken(refreshToken);
  }

  remove(): void {
    this.storage.removeItem(this.ACCESS_TOKEN_KEY);
    this.storage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  clear(): void {
    this.storage.clear();
  }

  get hasToken(): boolean {
    return !!this.token;
  }
}