import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
  export class GlobalErrorInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // Obsługa globalnych błędów
        // Możesz tu dodać np. wyświetlanie toastów, przekierowanie, logowanie itd.
        console.error('Global error intercepted:', error);

        // Przykład: przekierowanie przy 401
        // if (error.status === 401) {
        //   this.router.navigate(['/login']);
        // }

        return throwError(() => error);
      })
    );
  }
}
