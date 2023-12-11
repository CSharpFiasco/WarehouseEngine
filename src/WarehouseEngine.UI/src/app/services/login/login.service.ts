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
      .pipe(map(this.handleLoginResponse), catchError(this.handleError));
  }

  private handleLoginResponse(response: HttpResponse<JwtTokenResponse>): JwtTokenResponse {
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

  private handleError(error: HttpErrorResponse): Observable<JwtTokenResponse> {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(`Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return of({ type: 'Failed', error: 'Something bad happened; please try again later.' });
  }
}
