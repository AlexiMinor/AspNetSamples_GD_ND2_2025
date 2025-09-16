using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class AddArticlesCollectionCommandHandler : IRequestHandler<AddArticlesCollectionCommand>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly ArticleMapper _articleMapper;
    
    public AddArticlesCollectionCommandHandler(GoodArticleAggregatorContext context, ArticleMapper articleMapper)
    {
        _context = context;
        _articleMapper = articleMapper;
    }
    
    public async Task Handle(AddArticlesCollectionCommand command, CancellationToken cancellationToken)
    {
        await _context.Articles.AddRangeAsync(command.Articles.Select(a => 
            _articleMapper.MapArticleDtoToArticle(a)), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}

