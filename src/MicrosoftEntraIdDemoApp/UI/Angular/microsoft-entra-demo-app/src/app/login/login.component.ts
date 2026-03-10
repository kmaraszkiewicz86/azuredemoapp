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
      this.message = 'Witaj test, zalogowałeś się poprawnie!';
      this.isError = false;
      this.loginService.login();
    } catch {
      this.message = 'Logowanie nie udane';
      this.isError = true;
    }
  }
}
