import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = 'https://localhost:7207/UserController/';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<string> {
    return this.http.get<string>(this.baseUrl + 'GetAllUsers');
  }
}
