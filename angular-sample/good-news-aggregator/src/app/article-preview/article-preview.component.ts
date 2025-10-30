import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Article } from '../../models/article';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-article-preview',
  templateUrl: './article-preview.component.html',
  styleUrls: ['./article-preview.component.scss'],
  imports: [RouterLink],
})

export class ArticlePreviewComponent {
  @Input() article?:Article;
   @Output() selectedArticleEvent = new EventEmitter<Article>();

   selectArticle(article: Article): void {
     this.selectedArticleEvent.emit(article);
   }
}
