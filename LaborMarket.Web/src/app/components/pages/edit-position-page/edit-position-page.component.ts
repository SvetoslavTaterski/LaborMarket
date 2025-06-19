import { Component } from '@angular/core';
import { JobSummaryModel } from '../../../models/job-model';
import { EmployerDataModel } from '../../../models/employer-model';
import { JobsService } from '../../../services/jobs.service';
import { ActivatedRoute } from '@angular/router';
import { EmployerService } from '../../../services/employer.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-position-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-position-page.component.html',
  styleUrl: './edit-position-page.component.scss',
})
export class EditPositionPageComponent {
  public jobData: JobSummaryModel = {} as JobSummaryModel;
  public employerData: EmployerDataModel = {} as EmployerDataModel;
  public jobId: number | null = null;

  constructor(
    private jobService: JobsService,
    private route: ActivatedRoute,
    private employerService: EmployerService
  ) {}

  ngOnInit(): void {
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

  onSave(){
    console.log('Saving job data:', this.jobData);
  }

  onGoBack(): void {
    window.history.back();
  }
}
