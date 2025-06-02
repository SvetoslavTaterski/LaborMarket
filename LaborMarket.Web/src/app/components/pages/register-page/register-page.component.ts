import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { FormsModule } from '@angular/forms';
import { CreateUserModel } from '../../../models/user-model';
import { CreateEmployerModel } from '../../../models/employer-model';

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
    passwordHash: '',
    phoneNumber: '',
    role: 'user',
    createdAt: new Date().toISOString() // Set current date as createdAt
  }

  employerModel: CreateEmployerModel = {
    companyName: '',
    contactEmail: '',
    contactPhone: '',
    companyPassword: '',
    role: 'employer',
  }
  
  constructor(private userService: UserService) {
  }

  createUser() {
    this.userService.createUser(this.userModel).subscribe({
      next: (response) => {
        console.log('User created successfully:', response);
      },
      error: (err) => {
        console.error('Error creating user:', err);
      },
    });
  }
}
