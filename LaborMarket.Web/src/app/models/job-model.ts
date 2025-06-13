import { EmployerDataModel } from './employer-model';

export type JobDataModel = {
  jobId: number;
  title: string;
  description: string;
  company: string;
  location: string;
  postedAt: Date;
  employerId: number;
  employer: EmployerDataModel;
};

export type CreateJobModel ={
  title: string;
  description: string;
  location: string;
  employerEmail: string;
}
