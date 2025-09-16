using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class AddArticlesCollectionCommand : IRequest
{
    public IEnumerable<ArticleDto> Articles { get; set; }
}
