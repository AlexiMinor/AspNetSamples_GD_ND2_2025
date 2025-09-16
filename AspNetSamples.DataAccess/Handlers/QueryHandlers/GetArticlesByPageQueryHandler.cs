using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticlesByPageQueryHandler : IRequestHandler<GetArticlesByPageQuery, ArticleDto[]>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;

    public GetArticlesByPageQueryHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }

    public async Task<ArticleDto[]> Handle(GetArticlesByPageQuery request, CancellationToken cancellationToken)
    {
        return await _context.Articles
            .AsNoTracking()
            .Where(article => !string.IsNullOrWhiteSpace(article.Description))
            .Include(article => article.Source)
            .OrderBy(article => EF.Property<object>(article, request.OrderBy)) // Dynamic order by
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(article => _articleMapper.MapArticleToArticleDto(article))
            .ToArrayAsync(cancellationToken);
    }
}