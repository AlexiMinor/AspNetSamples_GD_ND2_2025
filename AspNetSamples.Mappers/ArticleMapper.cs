using System.ServiceModel.Syndication;
using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using AspNetSamples.Models;
using Riok.Mapperly.Abstractions;

namespace AspNetSamples.Mappers;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class ArticleMapper
{

    [MapProperty([nameof(Article.Source), nameof(Article.Source.Name)], nameof(ArticleDto.SourceName))] 
    [MapProperty(nameof(Article.Source), nameof(ArticleDto.SourceDto))] 
    public partial ArticleDto MapArticleToArticleDto(Article article);
    
    public partial Article? MapArticleDtoToArticle(ArticleDto dto);


    [MapProperty(nameof(CreateArticleModel.Text), nameof(ArticleDto.Content))]
    public partial ArticleDto MapCreateArticleModelToArticleDto(CreateArticleModel model);

    [MapProperty(nameof(ArticleDto.Content), nameof(EditArticleModel.Text))]
    public partial EditArticleModel MapArticleDtoToEditModel(ArticleDto dto);

    [MapProperty(nameof(EditArticleModel.Text), nameof(ArticleDto.Content))]
    public partial ArticleDto MapEditArticleModelToArticleDto(EditArticleModel model);


    [MapProperty([nameof(SyndicationItem.Title), nameof(SyndicationItem.Title.Text)], 
        nameof(ArticleDto.Title))]
    [MapProperty([nameof(SyndicationItem.Summary), nameof(SyndicationItem.Summary.Text)], 
        nameof(ArticleDto.Description))]
    [MapProperty([nameof(SyndicationItem.PublishDate), nameof(SyndicationItem.PublishDate.DateTime)],
        nameof(ArticleDto.CreatedAt))]
    [MapProperty(nameof(SyndicationItem.Id), nameof(ArticleDto.OriginUrl))]
    public partial ArticleDto MapOnlinerSyndicationItemToArticleDto(SyndicationItem item, Guid id, Guid sourceId);
}