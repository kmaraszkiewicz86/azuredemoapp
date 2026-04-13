import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

import { UserCheckService } from './usercheck.service';

@Component({
  selector: 'app-usercheck',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './usercheck.component.html',
  styleUrl: './usercheck.component.scss',
})
export class UserCheckComponent implements OnInit {
  private readonly usercheckService = inject(UserCheckService);

  isLoading = false;
  userName: string | null = null;
  errorMessage: string | null = null;
  roles: string[] | null = null;

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.loadUserAsync();
  }

  private async loadUserAsync(): Promise<void> {
    try {
      const response = await lastValueFrom(this.usercheckService.getCurrentUser());
      this.userName = response.name;
      this.roles = response.roles;
      this.isLoading = false;
    } catch (error) {
      const httpError = error as HttpErrorResponse;
      this.errorMessage = httpError.status === 404
        ? 'User endpoint was not found (404).'
        : httpError.error?.message ?? 'Failed to fetch user details.';
      this.isLoading = false;
    }
  }
}
