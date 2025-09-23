using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetArticleTextByIdQuery : IRequest<string>
{
    public Guid ArticleId { get; set; }
}