using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IWebParserService
{
    public Task<string> ParseArticlesAsync(string url, CancellationToken cancellationToken);
}