import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SendJsonModels } from './send-json.models';

@Injectable({
  providedIn: 'root'
})
export class SendJsonService {

  constructor(private http: HttpClient) { }

  sendJsonData(url: string, json: SendJsonModels) {
    return this.http.post(url, json);
  }
}
