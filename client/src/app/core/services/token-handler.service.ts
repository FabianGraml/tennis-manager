import { Injectable } from '@angular/core';

const JWT_TOKEN_KEY = 'auth-token';
const REFRESH_TOKEN_KEY = 'refresh-token';
const USER_ID = 'user-id';

@Injectable({
  providedIn: 'root',
})
export class TokenHandlerService {
  constructor() {}

  signOut(): void {
    window.sessionStorage.clear();
  }
  public saveToken(token: string): void {
    window.sessionStorage.removeItem(JWT_TOKEN_KEY);
    window.sessionStorage.setItem(JWT_TOKEN_KEY, token);
  }
  public saveRefreshToken(token: string): void {
    window.sessionStorage.removeItem(REFRESH_TOKEN_KEY);
    window.sessionStorage.setItem(REFRESH_TOKEN_KEY, token);
  }
  public saveUserId(token: string): void {
    window.sessionStorage.removeItem(USER_ID);
    window.sessionStorage.setItem(USER_ID, this.extractUserId(token));
  }
  public getUserId(): string | null {
    return window.sessionStorage.getItem(USER_ID);
  }
  public getRefreshToken(): string | null {
    return window.sessionStorage.getItem(REFRESH_TOKEN_KEY);
  }
  public getEmailFromToken(): string | null {
    const token = this.getToken();
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
      ];
    }
    return null;
  }
  public getToken(): string | null {
    return window.sessionStorage.getItem(JWT_TOKEN_KEY);
  }
  public extractUserId(token: string): string {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload[
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
    ];
  }
  public isUserLoggedIn(): boolean {
    let user = this.getToken();
    return !(user === null);
  }
}