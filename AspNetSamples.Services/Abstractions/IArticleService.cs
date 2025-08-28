using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IArticleService
{
    public Task<ArticleDto> GetArticleByIdAsync(Guid id, CancellationToken token = default);
    public Task<List<ArticleDto>> GetArticlesByPageAsync(int currentPage, int pageSize,
        CancellationToken cancellationToken = default);
    public Task<int> TotalCountAsync(CancellationToken cancellationToken = default);
    public Task UpdateArticleAsync(ArticleDto articleDto, CancellationToken token = default);
    public Task DeleteArticleAsync(Guid id, CancellationToken token = default);
    public Task AddArticleAsync(ArticleDto articleDto, CancellationToken token =default);
    public Task AddArticlesAsync(IEnumerable<ArticleDto> articleDto, CancellationToken token =default);
    public HashSet<string> GetExistingArticleUrls(CancellationToken token);
    public Task AggregateArticleTextAsync(CancellationToken token);
}