import {Component, OnInit, signal} from '@angular/core';
import {Article} from '../../models/article';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ArticlePreviewComponent} from '../article-preview/article-preview.component';
import {ArticlesService} from '../../services/articles/articles.service';
import {userSignal} from '../../signals/user-signal';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatPaginatorModule, PageEvent} from '@angular/material/paginator';

@Component({
  selector: 'app-articles',
  imports: [CommonModule,
    FormsModule,
    ArticlePreviewComponent,
    MatProgressSpinnerModule,
    MatPaginatorModule],
  templateUrl: './articles.html',
  styleUrl: './articles.scss',
})

export class ArticlesComponent implements OnInit {
  userSignal = userSignal;
  articles = signal<Article[] | null>([]);
  pageIndex = signal<number>(1);
  pageSize = signal<number>(10);
  totalArticlesCount = signal<number | null>(null);
  selectedArticle: Article | null = null;
  showSpinner = signal(false);

  constructor(private articlesService: ArticlesService) {
  }

  ngOnInit(): void {
    this.showSpinner.set(true);
    this.getArticles(this.pageIndex(), this.pageSize());
  }

  getArticles(currentPage:number, pageSize:number ): void {
    this.articlesService.getArticles(currentPage, pageSize)
      .subscribe(articles => {
        this.articles.set(articles ? articles.articles : null);
        this.totalArticlesCount.set(articles ? articles.totalArticles : null);
        this.showSpinner.set(false);
      });
  }

  selectArticle(article: Article): void {
    this.selectedArticle = article;
  }

  onPageChange(e: PageEvent): void {
    this.pageSize.set(e.pageSize);
    this.pageIndex.set(e.pageIndex+1);
    this.getArticles(this.pageIndex(), this.pageSize())
  }

}
