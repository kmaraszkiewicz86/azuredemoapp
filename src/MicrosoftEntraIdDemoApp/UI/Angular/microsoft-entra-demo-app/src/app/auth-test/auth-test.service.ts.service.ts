import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { AuthTestDto } from './auth-test.dto';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthTestServiceTsService {

  constructor(private http: HttpClient) { }

  getData(type: string): Observable<AuthTestDto> {
    return this.http.get<AuthTestDto>(`${environment.apiHost}/api/${type}`).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';

        if (error.status === 404) {
          errorMessage = 'Resource not found (404).';
        } else if (error.status === 500) {
          errorMessage = 'Internal server error (500). Please try again later.';
        } else {
          errorMessage = `An unexpected error occurred: ${error.message || 'Unknown error'}`;
        }

        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
