import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
      // CRITICAL: Every request to our API must include withCredentials,
      // otherwise the browser won't attach HttpOnly cookies!
    const clonedRequest = req.clone({
      withCredentials: true
    });

    return next(clonedRequest).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Session expired! Redirecting to Gateway for re-authentication
          console.warn('Session expired. Redirecting to login...');
          //window.location.href = `${environment.uiHost}/login`;
        }
        return throwError(() => error);
      })
    );
};
