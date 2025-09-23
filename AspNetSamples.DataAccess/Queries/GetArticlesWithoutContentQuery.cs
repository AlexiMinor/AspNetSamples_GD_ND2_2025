using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetArticlesWithoutContentQuery : IRequest<ArticleDto[]>
{
    
}