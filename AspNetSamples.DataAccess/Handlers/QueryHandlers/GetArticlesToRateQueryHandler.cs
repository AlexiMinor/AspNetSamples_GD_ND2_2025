using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticlesToRateQueryHandler : IRequestHandler<GetArticlesToRateQuery, ArticleDto[]>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;

    public GetArticlesToRateQueryHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }

    public async Task<ArticleDto[]> Handle(GetArticlesToRateQuery request, CancellationToken cancellationToken)
    {
        var articles = await _context.Articles
            .AsNoTracking()
            .Where(article =>article.Rate == null)
            .Select(article => _articleMapper.MapArticleToArticleDto(article))
            .ToArrayAsync(cancellationToken);

        return articles;
    }
}