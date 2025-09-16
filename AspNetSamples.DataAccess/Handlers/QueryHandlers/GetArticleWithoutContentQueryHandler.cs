using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticleWithoutContentQueryHandler : IRequestHandler<GetArticlesWithoutContentQuery, ArticleDto[]>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;

    public GetArticleWithoutContentQueryHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }

    public async Task<ArticleDto[]> Handle(GetArticlesWithoutContentQuery request, CancellationToken cancellationToken)
    {
        var articles = await _context.Articles
            .AsNoTracking()
            .Where(article => string.IsNullOrEmpty(article.Content))
            .Select(article => _articleMapper.MapArticleToArticleDto(article))
            .ToArrayAsync(cancellationToken);

        return articles;
    }
}