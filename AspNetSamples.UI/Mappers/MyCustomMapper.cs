using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;

namespace AspNetSamples.UI.Mappers;

public class MyCustomMapper
{
    public static ArticleDto ConvertArticleToArticleDto(Article article)
    {
        if (article == null) throw new ArgumentNullException(nameof(article));

        return new ArticleDto
        {
            Id = article.Id,
            Title = article.Title,
            Description = article.Description,
            Content = article.Content,
            SourceId = article.SourceId,
            Rate = article.Rate ?? 0.0,
        };
    }

    public static Article ConvertArticleDtoToArticle(ArticleDto article)
    {
        if (article == null) throw new ArgumentNullException(nameof(article));

        return new Article
        {
            Id = article.Id,
            Title = article.Title,
            Description = article.Description,
            Content = article.Content,
            SourceId = article.SourceId,
            Rate = article.Rate,
        };
    }
}
