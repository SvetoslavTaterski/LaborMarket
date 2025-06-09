import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateUserModel, UserDataModel } from '../models/user-model';
import { CreateEmployerModel, EmployerDataModel } from '../models/employer-model';
import { LoginModel } from '../models/login-model';
import { LoginResponseModel } from '../models/login-response-model';

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

  login(loginModel: LoginModel): Observable<LoginResponseModel> {
    return this.http.post<LoginResponseModel>(this.baseUrl + 'login', loginModel);
  }

}