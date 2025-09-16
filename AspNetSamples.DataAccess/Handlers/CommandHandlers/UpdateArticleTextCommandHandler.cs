using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class UpdateArticleTextCommandHandler : IRequestHandler<UpdateArticleTextCommand>
{
    private readonly GoodArticleAggregatorContext _context;

    public UpdateArticleTextCommandHandler(GoodArticleAggregatorContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateArticleTextCommand command, CancellationToken cancellationToken)
    {
        var article = await _context.Articles
            .SingleOrDefaultAsync(art 
            => art.Id.Equals(command.ArticleId), 
                cancellationToken: cancellationToken);
        if (article == null)
        {
            throw new ArgumentException($"Article {command.ArticleId} not found");
        }
        article.Content = command.NewContent;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

