using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticleCountQueryHandler : IRequestHandler<GetArticleCountQuery, int>
{
    private readonly GoodArticleAggregatorContext _context;

    public GetArticleCountQueryHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
        }

    public async Task<int> Handle(GetArticleCountQuery request, CancellationToken cancellationToken)
    {
        return await _context.Articles.CountAsync(cancellationToken);
    }
}