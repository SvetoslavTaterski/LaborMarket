import { Component } from '@angular/core';
import { JobSummaryModel } from '../../../models/job-model';
import { CommonModule } from '@angular/common';
import { JobsService } from '../../../services/jobs.service';
import { ActivatedRoute } from '@angular/router';
import { EmployerDataModel } from '../../../models/employer-model';
import { EmployerService } from '../../../services/employer.service';
import { JobApplicationService } from '../../../services/job-application.service';
import { UserService } from '../../../services/user.service';
import { UserDataModel } from '../../../models/user-model';
import { ToastrService } from 'ngx-toastr';

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
  public userData: UserDataModel = {} as UserDataModel;
  public userRole: string | null = null;

  constructor(
    private jobService: JobsService,
    private route: ActivatedRoute,
    private employerService: EmployerService,
    private jobApplicationService: JobApplicationService,
    private userService: UserService,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole');

    if (this.userRole === 'User') {
      this.userService.getUserByEmail().subscribe({
        next: (user) => {
          this.userData = user;
        },
        error: (err) => {
          this.toastrService.error('Failed to fetch user data!', err);
        },
      });
    }

    this.route.params.subscribe((params) => {
      this.jobId = +params['id'];
      // Fetch job data using the jobId
      if (this.jobId) {
        this.jobService.getJobById(this.jobId).subscribe({
          next: (data) => {
            this.jobData = data;
            this.employerService.getEmployerById(data.employerId).subscribe({
              next: (employer) => {
                this.employerData = employer;
              },
              error: () => {
                this.toastrService.error('Failed to fetch employer data!');
              },
            });
          },
          error: () => {
            this.toastrService.error('Failed to fetch job data!');
          },
        });
      }
    });
  }

  onGoBack(): void {
    window.history.back();
  }

  onSendCv() {
    const model = {
      userId: this.userData.userId,
      jobId: this.jobId!,
      applicationDate: new Date(),
      status: 'Pending',
    };

    this.jobApplicationService.createJobApplication(model).subscribe({
      next: () => {
        this.toastrService.success('CV изпратено успешно!');
      },
      error: () => {
        this.toastrService.error('Грешка при изпращане на CV!');
      },
    });
  }
}
