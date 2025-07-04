import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateJobModel, JobDataModel, JobSummaryModel } from '../models/job-model';

@Injectable({
  providedIn: 'root',
})
export class JobsService {
  private baseUrl = 'https://localhost:7207/JobController/';

  constructor(private http: HttpClient) {}

  getAllJobs(): Observable<JobDataModel[]> {
    return this.http.get<JobDataModel[]>(this.baseUrl + 'GetAllJobs');
  }

  getJobById(jobId: number): Observable<JobDataModel> {
    return this.http.get<JobDataModel>(this.baseUrl + 'GetJobById?jobId=' + jobId);
  }

  createJob(job: CreateJobModel): Observable<JobDataModel> {
    return this.http.post<JobDataModel>(this.baseUrl + 'CreateJob', job);
  }

  editJob(job: JobSummaryModel): Observable<JobSummaryModel> {
    return this.http.put<JobSummaryModel>(this.baseUrl + 'EditJob', job);
  }

  deleteJob(jobId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'DeleteJob?jobId=' + jobId);
  }
}
