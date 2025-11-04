import {Article} from './article';

export interface PagedArticles {
  articles: Article[];
  totalArticles: number;
}
