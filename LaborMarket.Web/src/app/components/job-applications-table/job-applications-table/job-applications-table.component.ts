import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-job-applications-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './job-applications-table.component.html',
  styleUrl: './job-applications-table.component.scss',
})
export class JobApplicationsTableComponent {
  @Input() jobApplications: any[] = [];
  @Output() approve = new EventEmitter<any>();
  @Output() decline = new EventEmitter<any>();
}
