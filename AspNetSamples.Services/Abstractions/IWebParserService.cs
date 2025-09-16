using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IWebParserService
{
    Task<string> ParseArticlesAsync(string url, CancellationToken cancellationToken);
}