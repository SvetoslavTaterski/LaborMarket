import { Component } from '@angular/core';
import { UserDataModel } from '../../../models/user-model';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-worker-details-page',
  standalone: true,
  imports: [],
  templateUrl: './worker-details-page.component.html',
  styleUrl: './worker-details-page.component.scss',
})
export class WorkerDetailsPageComponent {
  user: UserDataModel = {} as UserDataModel;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.userService.getUserById(+userId).subscribe(
        (data) => {
          this.user = data;
        },
        (error) => {
          console.error('Failed to fetch employer details', error);
        }
      );
    }
  }

  onBack(): void {
    window.history.back();
  }
}
