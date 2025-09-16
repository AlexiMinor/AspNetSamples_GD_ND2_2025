using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetAllSourcesWithRssQueryHandler : IRequestHandler<GetAllSourcesWithRssQuery, SourceDto[]>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly SourceMapper _sourceMapper;

    public GetAllSourcesWithRssQueryHandler(GoodArticleAggregatorContext context, SourceMapper sourceMapper)
    {
        _context = context;
        _sourceMapper = sourceMapper;
    }

    public async Task<SourceDto[]> Handle(GetAllSourcesWithRssQuery request, CancellationToken cancellationToken)
    {
        var sources = await _context.Sources
            .AsNoTracking()
            .Where(source => !string.IsNullOrWhiteSpace(source.RssLink))
            .Select(source => _sourceMapper.MapSourceToSourceDto(source))
            .ToArrayAsync(cancellationToken);

        return sources;
    }
}