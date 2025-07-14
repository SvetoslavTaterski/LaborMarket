import { Component } from '@angular/core';
import { JobSummaryModel } from '../../../models/job-model';
import { CommonModule } from '@angular/common';
import { JobsService } from '../../../services/jobs.service';
import { ActivatedRoute } from '@angular/router';
import { EmployerDataModel } from '../../../models/employer-model';
import { EmployerService } from '../../../services/employer.service';

@Component({
  selector: 'app-position-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './position-page.component.html',
  styleUrl: './position-page.component.scss',
})
export class PositionPageComponent {
  public jobData: JobSummaryModel = {} as JobSummaryModel;
  public employerData: EmployerDataModel = {} as EmployerDataModel;
  public jobId: number | null = null;
  public userRole: string | null = null;

  constructor(
    private jobService: JobsService,
    private route: ActivatedRoute,
    private employerService: EmployerService
  ) {}

  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole');
    
    this.route.params.subscribe((params) => {
      this.jobId = +params['id'];
      console.log('Job ID:', this.jobId);
      // Fetch job data using the jobId
      if (this.jobId) {
        this.jobService.getJobById(this.jobId).subscribe({
          next: (data) => {
            this.jobData = data;
            this.employerService.getEmployerById(data.employerId).subscribe({
              next: (employer) => {
                this.employerData = employer;
              },
              error: (err) => {
                console.error('Failed to fetch employer data:', err);
              },
            });
            console.log('Job Data:', this.jobData); // Debugging
          },
          error: (err) => {
            console.error('Failed to fetch job data:', err);
          },
        });
      }
    });
  }

  onGoBack(): void {
    window.history.back();
  }

  onSendCv() {
    // TODO: Implement CV sending logic
  }
}
