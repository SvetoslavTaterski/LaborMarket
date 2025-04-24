import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  constructor(private apiService: ApiService) {}

  onAboutClick() {
    this.apiService.ping().subscribe({
      next: (response) => console.log('API response:', response),
      error: (err) => console.error('API error:', err)
    });
  }
}
