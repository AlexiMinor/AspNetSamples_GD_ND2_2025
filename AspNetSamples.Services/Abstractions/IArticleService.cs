using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IArticleService
{
    public Task<ArticleDto> GetArticleByIdAsync(Guid id, CancellationToken token = default);
    public Task<ArticleDto[]> GetArticlesByPageAsync(int currentPage, int pageSize,
        CancellationToken cancellationToken = default);
    public Task<int> TotalCountAsync(CancellationToken cancellationToken = default);
    //public Task UpdateArticleAsync(ArticleDto articleDto, CancellationToken token = default);
    //public Task AddArticleAsync(ArticleDto articleDto, CancellationToken token =default);
    public Task WebScrapArticleTextAsync(CancellationToken token = default);
    public Task AggregateArticlesAsync(CancellationToken token = default);
    Task<int> GetArticlesCountAsync(CancellationToken httpContextRequestAborted);
}