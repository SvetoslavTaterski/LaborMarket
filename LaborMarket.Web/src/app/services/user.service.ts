import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserDataModel } from '../models/user-model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = 'https://localhost:7207/UserController/';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<UserDataModel[]> {
    return this.http.get<UserDataModel[]>(this.baseUrl + 'GetAllUsers');
  }
}
