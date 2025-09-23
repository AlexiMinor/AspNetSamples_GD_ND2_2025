using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetArticlesToRateQuery : IRequest<ArticleDto[]>
{
    
}