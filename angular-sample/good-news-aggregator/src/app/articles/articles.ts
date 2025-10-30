import {Component, OnInit, signal} from '@angular/core';
import { Article } from '../../models/article';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ArticlePreviewComponent } from '../article-preview/article-preview.component';
import { ArticlesService } from '../../services/articles/articles.service';
import {userSignal} from '../../signals/user-signal';

@Component({
  selector: 'app-articles',
  imports: [CommonModule, FormsModule, ArticlePreviewComponent],
  templateUrl: './articles.html',
  styleUrl: './articles.scss',
})

export class ArticlesComponent implements OnInit {
  userSignal = userSignal;
  articles = signal<Article[] | null >([]);
  selectedArticle: Article | null = null;

  constructor(private articlesService:ArticlesService) {}

  ngOnInit(): void {
    this.getArticles();
    console.log(this.articles);
  }

  getArticles(): void {
    this.articlesService.getArticles()
      .subscribe(articles => {
        this.articles.set(articles);
        console.log(this.articles);
      });
  }

  selectArticle(article: Article): void {
    this.selectedArticle = article;
  }

  closeArticle(): void {
    this.selectedArticle = null;
  }
  // }
}
