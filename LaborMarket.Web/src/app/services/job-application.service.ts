import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateApplicationModel } from '../models/job-application-model';

@Injectable({
  providedIn: 'root',
})
export class JobApplicationService {
  private baseUrl = 'https://localhost:7207/JobApplicationController/';

  constructor(private http: HttpClient) {}

  createJobApplication(model: CreateApplicationModel): Observable<CreateApplicationModel> {
    return this.http.post<CreateApplicationModel>(`${this.baseUrl}CreateJobApplication`, model);
  }

  getApplicationsByEmployerEmail(email: string): Observable<CreateApplicationModel[]> {
  return this.http.get<CreateApplicationModel[]>(`${this.baseUrl}GetEmployerApplicationsByEmployerEmail?employerEmail=${email}`);
  }
  
}
