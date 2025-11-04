import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Article } from '../../models/article';
import { RouterLink } from '@angular/router';
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';

@Component({
  selector: 'app-article-preview',
  templateUrl: './article-preview.component.html',
  styleUrls: ['./article-preview.component.scss'],
  imports: [RouterLink, MatCardModule, MatButtonModule, MatIconModule],
})

export class ArticlePreviewComponent {
  @Input() article?:Article;
   @Output() selectedArticleEvent = new EventEmitter<Article>();

   selectArticle(article: Article): void {
     this.selectedArticleEvent.emit(article);
   }
}
