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

    if (!email)
      throw new Error('Email not found in local storage');

    return this.http.get<EmployerDataModel>(
      `${this.baseUrl}GetEmployerByEmail?userEmail=${email}`
    );
  }

  getEmployerById(employerId: number): Observable<EmployerDataModel> {
    return this.http.get<EmployerDataModel>(
      `${this.baseUrl}GetEmployerById?userId=${employerId}`
    );
  }
}
