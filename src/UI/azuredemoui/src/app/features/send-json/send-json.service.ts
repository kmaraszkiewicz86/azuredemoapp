import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JsonModel } from './send-json.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SendJsonService {

  private readonly jsonfilesApiUrl = `${environment.azureFunctionUrl}/jsonfiles`;

  constructor(private http: HttpClient) { }

  sendJsonData(json: JsonModel) {
    return this.http.post(this.jsonfilesApiUrl, json);
  }

  getJsonData() {
    return this.http.get<JsonModel[]>(this.jsonfilesApiUrl);
  }
}
