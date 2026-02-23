import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JsonModel } from './send-json.models';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SendJsonService {

  private readonly jsonfilesApiUrl = `${environment.azureFunctionUrl}/jsonfiles`;

  constructor(private http: HttpClient) { }

  sendJsonData(json: JsonModel): Observable<any> {
    return this.http.post(this.jsonfilesApiUrl, json);
  }

  getJsonData(): Observable<JsonModel[]> {
    const test = this.http.get<JsonModel[]>(this.jsonfilesApiUrl).subscribe(data => console.log(data));

    console.log(test);

    return this.http.get<JsonModel[]>(this.jsonfilesApiUrl);
  }
}
