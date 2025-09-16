using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetArticlesByPageQuery : IRequest<ArticleDto[]>
{
    //nameof parameter for order by
    public string OrderBy { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

}