import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JobDataModel } from '../models/job-model';

@Injectable({
  providedIn: 'root',
})
export class JobsService {
  private baseUrl = 'https://localhost:7207/JobController/';

  constructor(private http: HttpClient) {}

  getAllJobs(): Observable<JobDataModel[]> {
    return this.http.get<JobDataModel[]>(this.baseUrl + 'GetAllJobs');
  }
}