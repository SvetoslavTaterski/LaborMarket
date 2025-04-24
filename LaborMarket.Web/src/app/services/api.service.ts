import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl = 'https://localhost:7207/';

  constructor(private http: HttpClient) {}

  ping(): Observable<string> {
    return this.http.get<string>(this.baseUrl + 'main/ping');
  }
}
