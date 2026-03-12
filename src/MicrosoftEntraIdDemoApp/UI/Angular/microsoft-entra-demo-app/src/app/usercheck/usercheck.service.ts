import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { UserCheckResponse } from './models/user-check.model';

@Injectable({ providedIn: 'root' })
export class UserCheckService {
  constructor(private readonly http: HttpClient) {}

  getCurrentUser(): Observable<UserCheckResponse> {
    return this.http.get<UserCheckResponse>(`${environment.apiHost}/bff/user`);
  }
}
