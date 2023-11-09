import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly jwtKey: string = "warehouse-engine.jwt";

  public setJwtToken(jwtToken: string) {
    sessionStorage.setItem(this.jwtKey, jwtToken);
  }

  public getJwtToken() {
    sessionStorage.getItem(this.jwtKey);
  }
}
