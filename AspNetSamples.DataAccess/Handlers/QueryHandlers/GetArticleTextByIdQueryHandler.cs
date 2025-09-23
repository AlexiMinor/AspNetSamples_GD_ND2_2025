using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetArticleTextByIdQueryHandler : IRequestHandler<GetArticleTextByIdQuery, string?>
{
    private readonly GoodArticleAggregatorContext _context;

    public GetArticleTextByIdQueryHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
    }

    public async Task<string?> Handle(GetArticleTextByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .SingleOrDefaultAsync(article => article.Id.Equals(request.ArticleId), cancellationToken);

        return article?.Content;
    }
}