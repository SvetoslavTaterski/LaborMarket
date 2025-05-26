import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormsModule } from '@angular/forms';
import { CreateUserModel } from '../../../models/user-model';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  userModel: CreateUserModel ={
    firstName: '',
    lastName: '',
    email: '',
    passwordHash: '',
    createdAt: new Date().toISOString() // Set current date as createdAt
  }
  constructor(private userService: UserService) {
  }

  createUser() {
    this.userService.createUser(this.userModel).subscribe({
      next: (response) => {
        console.log('User created successfully:', response);
        alert('User registered successfully!');
      },
      error: (err) => {
        console.error('Error creating user:', err);
        alert('Failed to register user.');
      },
    });
  }
}
