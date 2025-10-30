import {Component, OnInit, signal} from '@angular/core';
import { ArticlesService } from '../../services/articles/articles.service';
import { Article } from '../../models/article';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ArticleDetailsComponent',
  templateUrl: './article-details.component.html',
  styleUrls: ['./article-details.component.scss']
})
export class ArticleDetailsComponent implements OnInit {
  article = signal<Article | null>(null);

  constructor(private articleService:ArticlesService, private route:ActivatedRoute, private location:Location) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.articleService.getArticleById(id).subscribe(
        article => {
          this.article.set(article);
        },
        error => {
          console.error('Error fetching article:', error);
        }
      );
    }
  }

  goBack(): void {
    this.location.back();
  }

}
