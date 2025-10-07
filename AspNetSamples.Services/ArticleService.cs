using AspNetSamples.Core.Dto;
using AspNetSamples.Services.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using AspNetSamples.DataAccess.Commands;
using AspNetSamples.DataAccess.Queries;
using MediatR;

namespace AspNetSamples.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private readonly IRssService _rssService;
        private readonly IWebParserService _webParserService;
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;

        public ArticleService(IWebParserService webParserService, 
            ILogger<ArticleService> logger, 
            IRssService rssService, 
            IMediator mediator, 
            ICacheService cacheService)
        {
            _webParserService = webParserService;
            _logger = logger;
            _rssService = rssService;
            _mediator = mediator;
            _cacheService = cacheService;
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token = default)
        {
            var articleFromCache = _cacheService.Get<ArticleDto?>($"article_{id}");
            if (articleFromCache != null)
            {
                return articleFromCache;
            }

            var article = await _mediator.Send(new GetArticleByIdQuery() { ArticleId = id }, token);

            if (article != null)
            {
                _cacheService.Set($"article_{id}", article, DateTimeOffset.UtcNow.AddHours(1));
            }

            return article;
        }

        public async Task<ArticleDto[]> GetArticlesByPageAsync(int currentPage, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetArticlesByPageQuery()
            {
                PageNumber = currentPage,
                PageSize = pageSize,
                OrderBy = nameof(ArticleDto.CreatedAt)
            }, cancellationToken);
        }

        public async Task<int> TotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetArticleCountQuery(), cancellationToken);

        }

        public async Task WebScrapArticleTextAsync(CancellationToken token = default)
        {
            try
            {
                var articles = await _mediator.Send(new GetArticlesWithoutContentQuery(), token);

                //you should limit the degree of parallelism in production code depending
                //on your system capabilities & anti-ddos system
                foreach (var article in articles)
                {
                    var articleText = await _webParserService.ParseArticlesAsync(article.OriginUrl, token);
                   
                    var updateCommand = new UpdateArticleTextCommand()
                    {
                        ArticleId = article.Id,
                        NewContent = articleText
                    };
                    await _mediator.Send(updateCommand, token);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during articles web scrapping");
                throw;
            }
        }

        public async Task AggregateArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Aggregation started");
            var sourcesToAggregate = await _mediator.Send(
                    new GetAllSourcesWithRssQuery(),token);
            
            _logger.LogDebug($"Sources to aggregate: {sourcesToAggregate.Length}");

            if (!sourcesToAggregate.Any())
            {
                _logger.LogWarning("No sources with RSS found for aggregation");
                return;
            }
            var aggregatedArticles = new ConcurrentBag<ArticleDto>();
            await Parallel.ForEachAsync(sourcesToAggregate, token,
                async (source, cancellationToken) =>
                {
                    var existingArticleUrls = await _mediator
                        .Send(new GetExistingArticledUrlsForSourceQuery()
                        {
                            SourceId = source.Id
                        }, cancellationToken);
                    _logger.LogDebug($"Existing articles count: {existingArticleUrls.Count}");
                    
                    var articles = await _rssService.GetRssFeedBySourceAsync(source, existingArticleUrls, cancellationToken);
                    if (articles != null && articles.Any())
                    {
                        foreach (var article in articles)
                        {
                            aggregatedArticles.Add(article);
                        }
                    }
                });
            _logger.LogDebug($"Aggregated articles count: {aggregatedArticles.Count}");
            var addArticlesCollectionCommand = new AddArticlesCollectionCommand()
            {
                Articles = aggregatedArticles
            };
            await _mediator.Send(addArticlesCollectionCommand, token);
            _logger.LogInformation("Aggregated articles added to database");
            
            _logger.LogInformation("Starting web scrapping for articles");
            await WebScrapArticleTextAsync(token);
            _logger.LogInformation("Web scrapping completed");
        }

        public async Task<int> GetArticlesCountAsync(CancellationToken httpContextRequestAborted)
        {
            return await _mediator.Send(new GetArticleCountQuery(), httpContextRequestAborted);
        }
    }
}
