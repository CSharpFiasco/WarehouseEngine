import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../auth.service';
import type { Warehouse } from '../../types/warehouse';

@Injectable({
  providedIn: 'root',
})
export class WarehouseService {
  private readonly httpClient: HttpClient = inject(HttpClient);
  private readonly authService: AuthService = inject(AuthService);

  private readonly baseUrl = 'https://localhost:7088/api/v1/warehouse';

  getAll$(): Observable<Warehouse[]> {
    return this.httpClient.get<Warehouse[]>(`${this.baseUrl}/list`, {
      headers: this.#authHeaders(),
    });
  }

  #authHeaders(): HttpHeaders {
    const token = this.authService.getJwtToken();
    return new HttpHeaders(token ? { Authorization: `Bearer ${token}` } : {});
  }
}
