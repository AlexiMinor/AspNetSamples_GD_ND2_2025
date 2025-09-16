using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface ISourceService
{
    public Task<IEnumerable<SourceDto>> GetAllSourcesAsync(CancellationToken token = default);
}