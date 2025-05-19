import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { HeaderComponent } from "../../header/header.component";
import { FooterComponent } from "../../footer/footer.component";
import { JobDataModel } from '../../../models/job-model';
import { JobsService } from '../../../services/jobs.service';

@Component({
  selector: 'app-positions-page',
  standalone: true,
  imports: [HeaderComponent, FooterComponent,CommonModule, MatTableModule, MatPaginator],
  templateUrl: './positions-page.component.html',
  styleUrl: './positions-page.component.scss'
})
export class PositionsPageComponent implements OnInit {
    public jobsData = new MatTableDataSource<JobDataModel>([]);
    public displayedColumns: string[] = ['title', 'company', 'location', 'postedAt'];
  
    @ViewChild(MatPaginator) paginator!: MatPaginator;
  
    constructor(private jobService: JobsService) {}
  
    ngOnInit() {
  
      this.jobService.getAllJobs().subscribe({
        next: (response) => {
          this.jobsData.data = response;
        },
        error: (err) => {
          console.error('Failed to load jobs:', err);
        }
      });
      
    }
  
    ngAfterViewInit() {
      this.jobsData.paginator = this.paginator;
    }
}
