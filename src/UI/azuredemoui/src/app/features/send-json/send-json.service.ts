import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SendJsonModels } from './send-json.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SendJsonService {

  private readonly jsonfilesApiUrl = `${environment.azureFunctionUrl}/jsonfiles`;

  constructor(private http: HttpClient) { }

  sendJsonData(json: SendJsonModels) {
    return this.http.post(this.jsonfilesApiUrl, json);
  }
}
