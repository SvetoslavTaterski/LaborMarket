import { Component, OnInit, ViewChild } from '@angular/core';
import { EmployerService } from '../../../services/employer.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { EmployerDataModel } from '../../../models/employer-model';
import { MatPaginator } from '@angular/material/paginator';
import { HeaderComponent } from '../../header/header.component';
import { FooterComponent } from '../../footer/footer.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employers-page',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, MatPaginator, MatTableModule],
  templateUrl: './employers-page.component.html',
  styleUrl: './employers-page.component.scss',
})
export class EmployersPageComponent implements OnInit {
  public employersData = new MatTableDataSource<EmployerDataModel>([]);
  public displayedColumns: string[] = [
    'companyName',
    'companyEmail',
    'companyPhone',
  ];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private employerService: EmployerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.employerService.getAllEmployers().subscribe({
      next: (response) => {
        this.employersData.data = response;
      },
      error: (err) => {
        console.error('Failed to load employers:', err);
      },
    });
  }

  ngAfterViewInit() {
    this.employersData.paginator = this.paginator;
  }

  onRowDblClick(rowId: number) {
    console.log('Row double clicked:', rowId);
    this.router.navigate(['/employer', rowId]);
  }
}
