using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetArticleByIdQuery : IRequest<ArticleDto?>
{
    public Guid ArticleId { get; set; }

}