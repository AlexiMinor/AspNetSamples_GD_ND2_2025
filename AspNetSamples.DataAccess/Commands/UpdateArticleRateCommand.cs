using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class UpdateArticleRateCommand : IRequest
{
    public Guid ArticleId { get; set; }
    public int NewRating { get; set; }
}
