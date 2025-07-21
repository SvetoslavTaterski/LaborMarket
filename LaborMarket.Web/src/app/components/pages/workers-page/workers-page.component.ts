import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { UserService } from '../../../services/user.service';
import { UserDataModel } from '../../../models/user-model';
import { HeaderComponent } from "../../header/header.component";
import { FooterComponent } from "../../footer/footer.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-workers-page',
  standalone: true,
  imports: [MatTableModule, CommonModule, HeaderComponent, FooterComponent, MatPaginator],
  templateUrl: './workers-page.component.html',
  styleUrl: './workers-page.component.scss',
})
export class WorkersPageComponent implements OnInit {
  public userData= new MatTableDataSource<UserDataModel>([]);
  public displayedColumns: string[] = ['firstName', 'lastName', 'email', 'createdAt'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit() {

    this.userService.getAllUsers().subscribe({
      next: (response) => {
        this.userData.data = response;
      },
      error: (err) => {
        console.error('Failed to load users:', err);
      }
    });
    
  }

  ngAfterViewInit() {
    this.userData.paginator = this.paginator;
  }

  onRowDblClick(rowId: number) {
    this.router.navigate(['/worker', rowId]);
  }

}
