import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly jwtKey: string = 'warehouse-engine.jwt';

  public setJwtToken(jwtToken: string) {
    sessionStorage.setItem(this.jwtKey, jwtToken);
  }

  public getJwtToken(): string | null {
    return sessionStorage.getItem(this.jwtKey);
  }

  public unsetJwtToken(): void {
    sessionStorage.removeItem(this.jwtKey);
  }
}
