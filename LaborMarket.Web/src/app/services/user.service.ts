import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateUserModel, UserDataModel } from '../models/user-model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = 'https://localhost:7207/UserController/';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<UserDataModel[]> {
    return this.http.get<UserDataModel[]>(this.baseUrl + 'GetAllUsers');
  }

  getUserByEmail(): Observable<UserDataModel> {
    const email = localStorage.getItem('email');

    if (!email) 
      throw new Error('Email not found in local storage');
    
    return this.http.get<UserDataModel>(`${this.baseUrl}GetUserByEmail?userEmail=${email}`);
  }

  getUserById(id: number): Observable<UserDataModel> {
    return this.http.get<UserDataModel>(`${this.baseUrl}GetUserById?userId=${id}`);
  }

  setUserCv(cv: string): Observable<void> {
    const email = localStorage.getItem('email');

    if (!email)
      throw new Error('Email not found in local storage');

    return this.http.put<void>(`${this.baseUrl}SetUserCv?userEmail=${email}&cv=${cv}`, null);
  }

  uploadProfileImage(file: File): Observable<{ imageUrl: string }> {
  const email = localStorage.getItem('email');
  if (!email) throw new Error('Email not found in local storage');

  const formData = new FormData();
  formData.append('file', file);

  return this.http.post<{ imageUrl: string }>(
    `${this.baseUrl}UploadProfileImage?userEmail=${email}`,
    formData
  );
}
}
