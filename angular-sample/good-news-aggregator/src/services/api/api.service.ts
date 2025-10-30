import { Injectable } from '@angular/core';
import {HttpClient, HttpRequest} from '@angular/common/http';
import queryString from 'query-string';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  //todo change
  private getUrl(url: string): string {
    return environment.apiUrl + url;
  }

  get<T>(url: string, data: object = {}): Observable<T | null> {
    if (Object.keys(data).length > 0) {
      url = `${url}?${queryString.stringify(data)}`;
    }
    return this.http.get<T>(this.getUrl(url))
      .pipe(data => {return data;});
  }

  post<T>(url: string, data: object = {}, options = {}): Observable<T | null> {
    return this.http.post<T>(this.getUrl(url), data, options)
  }

  request(method:string, url:string, data:object, options:object={}){
    const request = new HttpRequest(method, this.getUrl(url), data, options);
    return this.http.request(request);
  }

  //if needed - add free any methods you need
  patch<T>(url: string, data: object = {}, options = {}): Observable<T | null> {
    return this.http.patch<T>(this.getUrl(url), data, options)
  }
}
