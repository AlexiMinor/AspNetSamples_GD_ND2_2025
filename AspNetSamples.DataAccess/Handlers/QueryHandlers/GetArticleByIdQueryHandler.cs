using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto?>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;

    public GetArticleByIdQueryHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }

    public async Task<ArticleDto?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .SingleOrDefaultAsync(article => article.Id.Equals(request.ArticleId), cancellationToken);

        return _articleMapper.MapArticleToArticleDto(article);
    }
}