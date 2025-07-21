import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateUserModel } from '../../../models/user-model';
import { CreateEmployerModel } from '../../../models/employer-model';
import { RegisterService } from '../../../services/register.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent {
  userModel: CreateUserModel = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: 'user',
    createdAt: new Date().toISOString(), // Set current date as createdAt
  };

  employerModel: CreateEmployerModel = {
    companyName: '',
    contactEmail: '',
    contactPhone: '',
    password: '',
    role: 'employer',
  };

  constructor(
    private registerService: RegisterService,
    private router: Router,
    private toastrService: ToastrService
  ) {}

  createUser() {
    if (this.userModel.role === 'user') {
      this.registerService.registerUser(this.userModel).subscribe({
        next: () => {
          this.toastrService.success('User registered successfully:');
          this.router.navigate(['/login']);
        },
        error: () => {
          this.toastrService.error('Error registering user:');
        },
      });
    } else if (this.userModel.role === 'employer') {
      this.registerService.registerEmployer(this.employerModel).subscribe({
        next: () => {
          this.toastrService.success('Employer registered successfully:');
          this.router.navigate(['/login']);
        },
        error: () => {
          this.toastrService.error('Error registering employer:');
        },
      });
    }
  }
}
