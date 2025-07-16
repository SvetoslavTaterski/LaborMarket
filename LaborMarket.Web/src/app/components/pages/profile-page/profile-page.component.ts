import { Component } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { UserDataModel } from '../../../models/user-model';
import { EmployerDataModel } from '../../../models/employer-model';
import { EmployerService } from '../../../services/employer.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { JobApplicationsTableComponent } from '../../job-applications-table/job-applications-table/job-applications-table.component';
import { CreateApplicationModel } from '../../../models/job-application-model';
import { JobApplicationService } from '../../../services/job-application.service';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [
    HeaderComponent,
    FormsModule,
    CommonModule,
    JobApplicationsTableComponent,
  ],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss',
})
export class ProfilePageComponent {
  public userRole: string | null = null;
  public jobApplications: CreateApplicationModel[] = [];
  public selectedFile: File | null = null;
  private userEmail: string | null = null;

  public displayModel = {
    displayName: '',
    email: '',
    phoneNumber: '',
    description: '',
    profileImageUrl: '',
  };

  constructor(
    public authService: AuthService,
    private userService: UserService,
    private employerService: EmployerService,
    private jobApplicationService: JobApplicationService
  ) {}

  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole');
    this.userEmail = localStorage.getItem('email');

    this.jobApplicationService.getApplicationsByEmployerEmail(this.userEmail!).subscribe({
      next: (applications: CreateApplicationModel[]) => {
        this.jobApplications = applications;
      },
      error: (err) => {
        console.error('Error fetching job applications:', err);
      }
    });

    if (this.userRole === 'User') {
      this.userService.getUserByEmail().subscribe({
        next: (user: UserDataModel) => {
          this.displayModel = {
            displayName: user.firstName + ' ' + user.lastName,
            email: user.email,
            phoneNumber: user.phoneNumber,
            description: user.cv,
            profileImageUrl: user.profileImageUrl || '',
          };
        },
        error: (err) => {
          console.error('Error fetching user data:', err);
        },
      });
    } else if (this.userRole === 'Employer') {
      this.employerService.getEmployerByEmail().subscribe({
        next: (employer: EmployerDataModel) => {
          this.displayModel = {
            displayName: employer.companyName,
            email: employer.contactEmail,
            phoneNumber: employer.contactPhone,
            description: employer.description,
            profileImageUrl: employer.profileImageUrl || '',
          };
        },
        error: (err) => {
          console.error('Error fetching employer data:', err);
        },
      });
    }
  }

  onSave() {
    if (this.userRole === 'User') {
      this.userService.setUserCv(this.displayModel.description).subscribe({
        next: () => {
          console.log('CV updated successfully');
        },
        error: (err) => {
          console.error('Error updating CV:', err);
        },
      });
    } else if (this.userRole === 'Employer') {
      this.employerService
        .setEmployerDescription(this.displayModel.description)
        .subscribe({
          next: () => {
            console.log('Description updated successfully');
          },
          error: (err) => {
            console.error('Error updating description:', err);
          },
        });
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      if (this.userRole === 'User') {
        this.userService.uploadProfileImage(file).subscribe({
          next: (response) => {
            console.log('Image uploaded successfully', response);
            window.location.reload();
          },
          error: (err) => {
            // Optionally, show an error message
            console.error('Image upload failed', err);
          },
        });
      } else if (this.userRole === 'Employer') {
        this.employerService.uploadProfileImage(file).subscribe({
          next: (response) => {
            console.log('Image uploaded successfully', response);
            window.location.reload();
          },
          error: (err) => {
            // Optionally, show an error message
            console.error('Image upload failed', err);
          },
        });
      }
    }
  }

  approve(application: any) {
  this.jobApplicationService.changeApplicationStatus(application.applicationId, 'Одобрен').subscribe({
    next: () => {
      application.status = 'Одобрен';
    },
    error: (err) => {
      console.error('Error approving application:', err);
    }
  });
}

decline(application: any) {
  this.jobApplicationService.changeApplicationStatus(application.applicationId, 'Отказан').subscribe({
    next: () => {
      application.status = 'Отказан';
    },
    error: (err) => {
      console.error('Error declining application:', err);
    }
  });
}
}
