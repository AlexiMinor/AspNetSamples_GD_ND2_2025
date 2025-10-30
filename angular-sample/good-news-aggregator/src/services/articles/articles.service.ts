import {Injectable} from '@angular/core';
import {Article} from '../../models/article';
import {ARTICLES_STORAGE} from '../../models/ARTICLES_STORAGE';
import {Observable} from 'rxjs';
import {ApiService} from '../api/api.service';

@Injectable({
  providedIn: 'root',
})

export class ArticlesService {
  constructor(private apiService: ApiService) {
  }

  getArticles(): Observable<Article[] | null> {
    return this.apiService.get<Article[]>('articles', { pageSize: 100})
  }

  getArticleById(id: string): Observable<Article | null> {
    return this.apiService.get<Article>(`articles/${id}`)
  }

  // private handleError<T>(operation = 'operation', result?: T) {
  //   return (error: any): Observable<T> => {
  //     console.error(operation);
  //     return of(result as T);
  //   }
  // }
}
