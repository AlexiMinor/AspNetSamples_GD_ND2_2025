namespace AspNetSamples.Services.Abstractions;

public interface IArticleRateService
{
    public Task RateArticlesAsync(CancellationToken token = default);
    Task RateArticlesInParallelAsync(CancellationToken token = default);
}