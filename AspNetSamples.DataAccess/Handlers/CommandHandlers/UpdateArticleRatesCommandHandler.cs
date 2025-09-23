using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class UpdateArticleRatesCommandHandler : IRequestHandler<UpdateArticleRatesCommand>
{
    private readonly GoodArticleAggregatorContext _context;

    public UpdateArticleRatesCommandHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateArticleRatesCommand command, CancellationToken cancellationToken)
    {
        foreach (var kvp in command.ArticleRatings)
        {
            var article = await _context.Articles
                .SingleOrDefaultAsync(art => art.Id.Equals(kvp.Key),
                    cancellationToken: cancellationToken);
            if (article == null)
            {
                throw new ArgumentException($"Article {kvp.Key} not found");
            }
            article.Rate = kvp.Value;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

