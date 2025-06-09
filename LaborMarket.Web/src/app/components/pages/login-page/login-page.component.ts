import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterService } from '../../../services/register.service';
import { LoginModel } from '../../../models/login-model';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';
import { LoginResponseModel } from '../../../models/login-response-model';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  loginModel: LoginModel = {
    email: '',
    password: '',
  };

  constructor(
    private readonly registerService: RegisterService,
    private readonly toastrService: ToastrService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  login() {
    this.registerService.login(this.loginModel).subscribe({
      next: (response: LoginResponseModel) => {
        console.log('Login successful:', response);
        this.authService.login(response.role);
        this.toastrService.success("Успешно влизане");
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error logging in:', error);
        this.toastrService.error("Грешно потребителско име или парола","Error");
      },
    });
  }
}
