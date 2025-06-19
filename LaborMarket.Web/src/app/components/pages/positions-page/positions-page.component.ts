import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { HeaderComponent } from '../../header/header.component';
import { FooterComponent } from '../../footer/footer.component';
import { CreateJobModel, JobDataModel } from '../../../models/job-model';
import { JobsService } from '../../../services/jobs.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-positions-page',
  standalone: true,
  imports: [
    HeaderComponent,
    FooterComponent,
    CommonModule,
    MatTableModule,
    MatPaginator,
    FormsModule,
  ],
  templateUrl: './positions-page.component.html',
  styleUrl: './positions-page.component.scss',
})
export class PositionsPageComponent implements OnInit {
  public userRole: string | null = null;
  public loggedInEmail: string | null = null;

  public createJobModel: CreateJobModel = {
    title: '',
    description: '',
    location: '',
    employerEmail: '',
  };
  public jobsData = new MatTableDataSource<JobDataModel>([]);
  public displayedColumns: string[] = [
    'title',
    'company',
    'location',
    'postedAt',
    'actions',
  ];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private jobService: JobsService, private router: Router) {}

  ngOnInit() {
    this.userRole = localStorage.getItem('userRole');
    this.loggedInEmail = localStorage.getItem('email');
    this.jobService.getAllJobs().subscribe({
      next: (response) => {
        this.jobsData.data = response.map((job) => ({
          ...job,
          isCreator: job.employer?.contactEmail === this.loggedInEmail, // Add a flag for creator
        }));
      },
      error: (err) => {
        console.error('Failed to load jobs:', err);
      },
    });
  }

  ngAfterViewInit() {
    this.jobsData.paginator = this.paginator;
  }

  onRowDblClick(jobId: number) {
    console.log('Row double-clicked:', jobId);
    this.router.navigate(['/position', jobId]);
  }

  onCreateJob() {
    this.createJobModel.employerEmail = localStorage.getItem('email') || ''; // Set employer email
    this.jobService.createJob(this.createJobModel).subscribe({
      next: () => {
        console.log('Job created successfully:', this.createJobModel);
        window.location.reload();
      },
      error: (err) => {
        console.error('Failed to create job:', err);
      },
    });
  }

  onDelete(jobModel: JobDataModel) {
    this.jobService.deleteJob(jobModel.jobId).subscribe({
      next: () => {
        console.log('Job deleted successfully:', jobModel.jobId);
        this.jobsData.data = this.jobsData.data.filter(
          (job) => job.jobId !== jobModel.jobId
        );
      },
      error: (err) => {
        console.error('Failed to delete job:', err);
      },
    });
  }

  onEdit(jobModel: JobDataModel) {
    this.router.navigate(['/edit-position', jobModel.jobId]);
  }
}
