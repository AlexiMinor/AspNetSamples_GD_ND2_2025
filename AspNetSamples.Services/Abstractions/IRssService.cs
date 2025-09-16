using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IRssService
{
    Task<IEnumerable<ArticleDto>?> GetRssFeedBySourceAsync(SourceDto dto, HashSet<string> existingArticles,
        CancellationToken token = default);
    
}