import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedIn = false;
  private userRole: string | null = null;

  login(role: string) {
    this.isLoggedIn = true;
    this.userRole = role;
    localStorage.setItem('isLoggedIn', 'true');
    localStorage.setItem('userRole', role);
  }

  logout() {
    this.isLoggedIn = false;
    this.userRole = null;
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('userRole');
  }

  getLoggedInStatus(): boolean {
    const storedStatus = localStorage.getItem('isLoggedIn');
    return storedStatus === 'true';
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }
}
