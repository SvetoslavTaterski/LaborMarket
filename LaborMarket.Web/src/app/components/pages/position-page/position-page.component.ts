import { Component } from '@angular/core';
import { JobSummaryModel } from '../../../models/job-model';
import { CommonModule } from '@angular/common';
import { JobsService } from '../../../services/jobs.service';
import { ActivatedRoute } from '@angular/router';
import { HeaderComponent } from "../../header/header.component";

@Component({
  selector: 'app-position-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './position-page.component.html',
  styleUrl: './position-page.component.scss',
})
export class PositionPageComponent {
  public jobData: JobSummaryModel = {} as JobSummaryModel;
  public jobId: number | null = null;

  constructor(private jobService: JobsService, private route: ActivatedRoute) {}

  //TODO: Пипни го малко по-добре стилизирай страницата и оправи кода в ngOnInit
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.jobId = +params['id'];
      console.log('Job ID:', this.jobId);
      // Fetch job data using the jobId
      if (this.jobId) {
        this.jobService.getJobById(this.jobId).subscribe({
          next: (data) => {
            this.jobData = data;
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
}
