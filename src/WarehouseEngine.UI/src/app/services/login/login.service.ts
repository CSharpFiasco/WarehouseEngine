import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import type { JwtTokenResponse } from 'src/app/types/jwt';
import { Observable, catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly httpClient: HttpClient = inject(HttpClient);

  login$(username: string, password: string): Observable<JwtTokenResponse> {
    return this.httpClient
      .post<JwtTokenResponse>(
        'https://localhost:7088/api/v1/Authenticate',
        {
          username: username,
          password: password,
        },
        { observe: 'response' }
      )
      .pipe(map(this.#handleLoginResponse), catchError(this.#handleError));
  }

  #handleLoginResponse(response: HttpResponse<JwtTokenResponse>): JwtTokenResponse {
    const isUnauthorized = response.status === 401;
    if (isUnauthorized) {
      return { type: 'Unauthorized' };
    }

    const bearerToken = response.headers.get('Bearer');
    if (bearerToken === null) {
      return { type: 'Failed', error: 'Failed to retrieve bearer token' };
    }

    return { type: 'Success', jwt: bearerToken };
  }

  #handleError(error: unknown): Observable<JwtTokenResponse> {
    if (!(error instanceof HttpErrorResponse)) {
      console.error('An unexpected error occurred:', error);
      return of({ type: 'Failed', error: 'An unexpected error occurred. Please try again later.' });
    }

    console.error(`Backend returned code ${error.status}, body was: `, error.error);
    return of({ type: 'Failed', error: 'Something went wrong. Please try again later.' });
  }
}
