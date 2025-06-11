import { Injectable } from '@angular/core';
import { LoginResponseModel } from '../models/login-response-model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedIn = false;
  private userRole: string | null = null;

  constructor(private router: Router) {
  }

  login(response: LoginResponseModel) {
    this.isLoggedIn = true;
    this.userRole = response.role;
    localStorage.setItem('isLoggedIn', 'true');
    localStorage.setItem('userRole', response.role);
    localStorage.setItem('email', response.email);
    localStorage.setItem('phoneNumber', response.phoneNumber);
  }

  logout() {
    this.isLoggedIn = false;
    this.userRole = null;
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('userRole');
    localStorage.removeItem('email');
    localStorage.removeItem('phoneNumber');
    this.router.navigate(['/']);
  }

  getLoggedInStatus(): boolean {
    const storedStatus = localStorage.getItem('isLoggedIn');
    return storedStatus === 'true';
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }
}
