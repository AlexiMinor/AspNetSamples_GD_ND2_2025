using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface ISourceService
{
    public Task AddSourcesAsync(IEnumerable<SourceDto> sourceDtos, CancellationToken token =default);
    public Task<IEnumerable<SourceDto>> GetAllAsync(CancellationToken token = default);
}