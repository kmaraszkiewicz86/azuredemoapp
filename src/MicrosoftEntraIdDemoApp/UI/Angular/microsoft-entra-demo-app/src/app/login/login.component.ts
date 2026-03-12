import { Component } from '@angular/core';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  message: string | null = null;
  isError = false;

  constructor(private loginService: LoginService) {}

  onLogin(): void {
    try {
      this.message = 'Login successful. Redirecting...';
      this.isError = false;
      this.loginService.login();
    } catch {
      this.message = 'Login failed.';
      this.isError = true;
    }
  }
}
