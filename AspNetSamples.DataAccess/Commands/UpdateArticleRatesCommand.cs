using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class UpdateArticleRatesCommand : IRequest
{
    public Dictionary<Guid, int> ArticleRatings { get; set; }
}
