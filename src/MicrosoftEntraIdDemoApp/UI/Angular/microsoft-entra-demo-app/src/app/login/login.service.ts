import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class LoginService {
  login(): void {
    window.location.href = `${environment.apiHost}/login?redirect=${environment.uiHost}/userCheck`;
  }
}
