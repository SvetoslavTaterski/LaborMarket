import { Component } from '@angular/core';
import { HeaderComponent } from '../../header/header.component';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [HeaderComponent],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss',
})
export class ProfilePageComponent {
  public userEmail: string | null = null;


  constructor(public authService: AuthService) {
  }

  ngOnInit(): void {
    this.userEmail = localStorage.getItem('email');
  }
}
