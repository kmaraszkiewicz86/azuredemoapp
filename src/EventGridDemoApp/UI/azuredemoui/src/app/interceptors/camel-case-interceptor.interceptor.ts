import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable()
export class CamelCaseInterceptor implements HttpInterceptor {
  private toCamelCase(obj: any): any {
    if (Array.isArray(obj)) {
      return obj.map(v => this.toCamelCase(v));
    } else if (obj !== null && typeof obj === 'object') {
      return Object.keys(obj).reduce((result: any, key: string) => {
        const camelKey = key.charAt(0).toLowerCase() + key.slice(1);
        result[camelKey] = this.toCamelCase(obj[key]);
        return result;
      }, {});
    }
    return obj;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      map(event => {
        if (event instanceof HttpResponse && event.body) {
          return event.clone({ body: this.toCamelCase(event.body) });
        }
        return event;
      })
    );
  }
}
