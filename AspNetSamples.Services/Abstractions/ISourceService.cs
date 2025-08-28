using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface ISourceService
{
    public Task AddSourcesAsync(IEnumerable<SourceDto> sourceDtos, CancellationToken token = default);
    public Task<IEnumerable<SourceDto>> GetAllSourcesAsync(CancellationToken token = default);
    public Task<List<SourceDto>> GetAllSourcesWithRssAsync(CancellationToken token = default);
}