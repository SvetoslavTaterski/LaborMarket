import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateUserModel, UserDataModel } from '../models/user-model';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private baseUrl = 'https://localhost:7207/RegisterController/';

  constructor(private http: HttpClient) {}

  registerUser(user: CreateUserModel): Observable<UserDataModel> {
    return this.http.post<UserDataModel>(this.baseUrl + 'register-user', user);
  }

}