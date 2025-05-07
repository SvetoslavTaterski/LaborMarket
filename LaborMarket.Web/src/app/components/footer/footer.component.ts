import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  constructor(private apiService: UserService) {}

  onAboutClick() {
    this.apiService.getAllUsers().subscribe({
      next: (response) => console.log('API response:', response),
      error: (err) => console.error('API error:', err)
    });
  }
}
