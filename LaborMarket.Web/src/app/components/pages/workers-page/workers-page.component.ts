import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { UserService } from '../../../services/user.service';
import { UserDataModel } from '../../../models/user-model';

@Component({
  selector: 'app-workers-page',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './workers-page.component.html',
  styleUrl: './workers-page.component.scss',
})
export class WorkersPageComponent implements OnInit {
  public userData: UserDataModel[] = [];
  public displayedColumns: string[] = ['firstName', 'lastName', 'email', 'createdAt'];

  constructor(private userService: UserService) {}

  ngOnInit() {

    this.userService.getAllUsers().subscribe({
      next: (response) => {
        this.userData = response;
      },
      error: (err) => {
        console.error('Failed to load users:', err);
      }
    });
    
  }

}
