import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoginService {
  login(): void {
    window.location.href = 'https://localhost:7161/login';
  }
}
