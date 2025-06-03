import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateUserModel, UserDataModel } from '../models/user-model';
import { CreateEmployerModel, EmployerDataModel } from '../models/employer-model';
import { LoginModel } from '../models/login-model';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private baseUrl = 'https://localhost:7207/RegisterController/';

  constructor(private http: HttpClient) {}

  registerUser(user: CreateUserModel): Observable<UserDataModel> {
    return this.http.post<UserDataModel>(this.baseUrl + 'register-user', user);
  }

  registerEmployer(employer: CreateEmployerModel): Observable<EmployerDataModel> {
    return this.http.post<EmployerDataModel>(this.baseUrl + 'register-employer', employer);
  }

  login(loginModel: LoginModel): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(this.baseUrl + 'login', loginModel);
  }

}