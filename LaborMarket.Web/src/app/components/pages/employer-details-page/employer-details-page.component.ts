import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployerService } from '../../../services/employer.service';
import { EmployerDataModel } from '../../../models/employer-model';

@Component({
  selector: 'app-employer-details-page',
  standalone: true,
  imports: [],
  templateUrl: './employer-details-page.component.html',
  styleUrl: './employer-details-page.component.scss',
})
export class EmployerDetailsPageComponent {
  employer: EmployerDataModel = {} as EmployerDataModel;

  constructor(
    private route: ActivatedRoute,
    private employerService: EmployerService
  ) {}

  ngOnInit(): void {
    const employerId = this.route.snapshot.paramMap.get('id');
    if (employerId) {
      this.employerService.getEmployerById(+employerId).subscribe(
        (data) => {
          this.employer = data;
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
