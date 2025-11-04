using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticlesByPageQueryHandler : IRequestHandler<GetArticlesByPageQuery, PagedArticlesDto>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;

    public GetArticlesByPageQueryHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }

    public async Task<PagedArticlesDto> Handle(GetArticlesByPageQuery request, CancellationToken cancellationToken)
    {
        var articles = await _context.Articles
            .AsNoTracking()
            .Where(article => !string.IsNullOrWhiteSpace(article.Description) && 
                              article.Rate.HasValue)
            .Include(article => article.Source)
            .OrderBy(article => EF.Property<object>(article, request.OrderBy)) // Dynamic order by
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(article => _articleMapper.MapArticleToArticleDto(article))
            .ToArrayAsync(cancellationToken);
        var totalArticles = await _context.Articles.AsNoTracking()
            .Where(article => !string.IsNullOrWhiteSpace(article.Description) &&
                              article.Rate.HasValue)
            .CountAsync(cancellationToken: cancellationToken);

        return new PagedArticlesDto()
        {
            Articles = articles,
            TotalArticles = totalArticles,
        };
    }
}