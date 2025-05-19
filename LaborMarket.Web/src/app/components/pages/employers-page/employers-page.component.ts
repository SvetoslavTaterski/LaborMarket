import { Component, OnInit } from '@angular/core';
import { EmployerService } from '../../../services/employer.service';
import { MatTableDataSource } from '@angular/material/table';
import { EmployerDataModel } from '../../../models/employer-model';

@Component({
  selector: 'app-employers-page',
  standalone: true,
  imports: [],
  templateUrl: './employers-page.component.html',
  styleUrl: './employers-page.component.scss',
})
export class EmployersPageComponent implements OnInit {
  public employersData = new MatTableDataSource<EmployerDataModel>([]);

  constructor(private employerService: EmployerService) {}

  ngOnInit(): void {
    this.employerService.getAllEmployers().subscribe({
        next: (response) => {
          this.employersData.data = response;
        },
        error: (err) => {
          console.error('Failed to load jobs:', err);
        }
      });
  }
}
