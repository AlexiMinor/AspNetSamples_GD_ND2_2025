using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetExistingArticledUrlsForSourceQuery : IRequest<HashSet<string>>
{
    public Guid SourceId { get; set; }
}