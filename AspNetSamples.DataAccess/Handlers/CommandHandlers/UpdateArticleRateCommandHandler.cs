using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class UpdateArticleRateCommandHandler : IRequestHandler<UpdateArticleRateCommand>
{
    private readonly GoodArticleAggregatorContext _context;

    public UpdateArticleRateCommandHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateArticleRateCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .SingleOrDefaultAsync(art 
            => art.Id.Equals(command.ArticleId), 
                cancellationToken: cancellationToken);
        if (article == null)
        {
            throw new ArgumentException($"Article {command.ArticleId} not found");
        }
        article.Rate = command.NewRating;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

