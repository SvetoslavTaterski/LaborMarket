import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateUserModel } from '../../../models/user-model';
import { CreateEmployerModel } from '../../../models/employer-model';
import { RegisterService } from '../../../services/register.service';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  userModel: CreateUserModel ={
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: 'user',
    createdAt: new Date().toISOString() // Set current date as createdAt
  }

  employerModel: CreateEmployerModel = {
    companyName: '',
    contactEmail: '',
    contactPhone: '',
    password: '',
    role: 'employer',
  }

  constructor(private registerService: RegisterService) {
  }

  createUser() {
    if(this.userModel.role === 'user'){
      this.registerService.registerUser(this.userModel).subscribe({
        next: (response) => {
          console.log('User registered successfully:', response);
        },
        error: (error) => {
          console.error('Error registering user:', error);
        }
      });
    }
    else if(this.userModel.role === 'employer'){
      this.registerService.registerEmployer(this.employerModel).subscribe({
        next: (response) => {
          console.log('Employer registered successfully:', response);
        },
        error: (error) => {
          console.error('Error registering employer:', error);
        }
      });
    }
  }
}
