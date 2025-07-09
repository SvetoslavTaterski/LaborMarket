import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EmployerDataModel } from '../models/employer-model';

@Injectable({
  providedIn: 'root',
})
export class EmployerService {
  private baseUrl = 'https://localhost:7207/EmployerController/';

  constructor(private http: HttpClient) {}

  getAllEmployers(): Observable<EmployerDataModel[]> {
    return this.http.get<EmployerDataModel[]>(this.baseUrl + 'GetAllEmployers');
  }

  getEmployerByEmail(): Observable<EmployerDataModel> {
    const email = localStorage.getItem('email');

    if (!email) throw new Error('Email not found in local storage');

    return this.http.get<EmployerDataModel>(
      `${this.baseUrl}GetEmployerByEmail?userEmail=${email}`
    );
  }

  getEmployerById(employerId: number): Observable<EmployerDataModel> {
    return this.http.get<EmployerDataModel>(
      `${this.baseUrl}GetEmployerById?userId=${employerId}`
    );
  }

  setEmployerDescription(description: string): Observable<void> {
    const email = localStorage.getItem('email');

    if (!email) throw new Error('Email not found in local storage');

    return this.http.put<void>(
      `${this.baseUrl}SetEmployerDescription?employerEmail=${email}&description=${description}`,
      null
    );
  }

  uploadProfileImage(file: File): Observable<{ imageUrl: string }> {
    const email = localStorage.getItem('email');
    if (!email) throw new Error('Email not found in local storage');

    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<{ imageUrl: string }>(
      `${this.baseUrl}UploadProfileImage?employerEmail=${email}`,
      formData
    );
  }
}
