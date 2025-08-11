import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SendJsonModels } from './send-json.models';

@Injectable({
  providedIn: 'root'
})
export class SendJsonService {

  private readonly apiUrl = 'http://localhost:7144/api/jsonfiles';

  constructor(private http: HttpClient) { }

  sendJsonData(json: SendJsonModels) {
    return this.http.post(this.apiUrl, json);
  }
}
