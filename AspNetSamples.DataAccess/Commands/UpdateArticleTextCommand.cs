using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class UpdateArticleTextCommand : IRequest
{
    public Guid ArticleId { get; set; }
    public string NewContent { get; set; }
}
