import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
@Injectable()
export class AzureFunctionTokenInterceptor implements HttpInterceptor {

  private readonly azureFunctionToken = environment.azureFunctionToken;

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Sprawdź, czy to request do Azure Function
    if (req.url.includes('azurewebsites.net')) {
      // Dodaj token do query string
      const url = this.appendTokenToUrl(req.url, this.azureFunctionToken);
      const cloned = req.clone({ url });
      return next.handle(cloned);
    }
    return next.handle(req);
  }

  private appendTokenToUrl(url: string, token: string): string {
    const separator = url.includes('?') ? '&' : '?';
    return `${url}${separator}token=${token}`;
  }
}
