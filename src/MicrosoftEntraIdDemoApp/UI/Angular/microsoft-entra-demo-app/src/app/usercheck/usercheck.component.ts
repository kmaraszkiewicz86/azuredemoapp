import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { UserCheckService } from './usercheck.service';

@Component({
  selector: 'app-usercheck',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './usercheck.component.html',
  styleUrl: './usercheck.component.scss',
})
export class UserCheckComponent implements OnInit {
  isLoading = false;
  userName: string | null = null;
  errorMessage: string | null = null;

  constructor(private readonly usercheckService: UserCheckService) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.usercheckService.getCurrentUser().subscribe({
      next: response => {
        this.userName = response.name;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.status === 404
          ? 'User endpoint was not found (404).'
          : error.error?.message ?? 'Failed to fetch user details.';
        this.isLoading = false;
      },
    });
  }
}
