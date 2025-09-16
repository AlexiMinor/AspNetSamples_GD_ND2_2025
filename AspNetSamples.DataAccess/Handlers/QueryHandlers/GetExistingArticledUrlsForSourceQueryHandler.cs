using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetExistingArticledUrlsForSourceQueryHandler : IRequestHandler<GetExistingArticledUrlsForSourceQuery, HashSet<string>>
{
    private readonly GoodArticleAggregatorContext _context;

    public GetExistingArticledUrlsForSourceQueryHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> Handle(GetExistingArticledUrlsForSourceQuery query, CancellationToken cancellationToken)
    {
        return _context.Articles
            .AsNoTracking()
            .Where(article => article.SourceId.Equals(query.SourceId))
            .Select(article => article.OriginUrl).ToHashSet();
    }
}