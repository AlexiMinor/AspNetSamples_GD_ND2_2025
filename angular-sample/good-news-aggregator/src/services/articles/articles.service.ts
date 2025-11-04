import {Injectable} from '@angular/core';
import {Article} from '../../models/article';
import {ARTICLES_STORAGE} from '../../models/ARTICLES_STORAGE';
import {Observable} from 'rxjs';
import {ApiService} from '../api/api.service';
import {PagedArticles} from '../../models/paged-articles';

@Injectable({
  providedIn: 'root',
})

export class ArticlesService {
  constructor(private apiService: ApiService) {
  }

  getArticles(currentPage:number, pageSize:number): Observable<PagedArticles | null> {
    return this.apiService.get<PagedArticles>('articles', { pageSize: pageSize, currentPage: currentPage})
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
