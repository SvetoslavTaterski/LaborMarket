import { Component } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { UserDataModel } from '../../../models/user-model';
import { EmployerDataModel } from '../../../models/employer-model';
import { EmployerService } from '../../../services/employer.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [HeaderComponent, FormsModule, CommonModule],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss',
})
export class ProfilePageComponent {
  public userRole: string | null = null;
  selectedFile: File | null = null;

  public displayModel = {
    displayName: '',
    email: '',
    phoneNumber: '',
    description: '',
  };

  constructor(
    public authService: AuthService,
    private userService: UserService,
    private employerService: EmployerService
  ) {}

  //TODO: Change model to display profile picutre
  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole');

    if (this.userRole === 'User') {
      this.userService.getUserByEmail().subscribe({
        next: (user: UserDataModel) => {
          this.displayModel = {
            displayName: user.firstName + ' ' + user.lastName,
            email: user.email,
            phoneNumber: user.phoneNumber,
            description: user.cv,
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
      this.employerService.uploadProfileImage(file).subscribe({
        next: (response) => {
          console.log('Image uploaded successfully', response);
        },
        error: (err) => {
          // Optionally, show an error message
          console.error('Image upload failed', err);
        }
      });
    }
  }
}
