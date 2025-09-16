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

        public ArticleService(IWebParserService webParserService, 
            ILogger<ArticleService> logger, 
            ISourceService sourceService, 
            IRssService rssService, 
            IMediator mediator)
        {
            _webParserService = webParserService;
            _logger = logger;
            _rssService = rssService;
            _mediator = mediator;
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token = default)
        {
          return await _mediator.Send(new GetArticleByIdQuery() { ArticleId = id }, token);
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

        //public async Task UpdateArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        //{
        //    var article = await _context.Articles
        //        //.AsNoTracking()
        //        .SingleOrDefaultAsync(article => article.Id.Equals(articleDto.Id), cancellationToken: token);

        //    if (article == null)
        //        return;

        //    article.Title = articleDto.Title;
        //    article.Description = articleDto.Description;
        //    article.Content = articleDto.Content;
        //    article.SourceId = articleDto.SourceId;
        //    //_context.Update(article);
        //    await _context.SaveChangesAsync(token);

        //    //var patchDictionary = new Dictionary<string, object>();
        //    ////not best practice, but for simplicity
        //    //if (!article.Content.Equals(articleDto.Content))
        //    //{
        //    //    patchDictionary.Add(nameof(article.Content), articleDto.Content);
        //    //}

        //    //if (!article.Title.Equals(articleDto.Title))
        //    //{
        //    //    patchDictionary.Add(nameof(article.Title), articleDto.Title);
        //    //}

        //    //if (!article.Description.Equals(articleDto.Description))
        //    //{
        //    //    patchDictionary.Add(nameof(article.Description), articleDto.Description);
        //    //}

        //    //if (!article.SourceId.Equals(articleDto.SourceId))
        //    //{
        //    //    patchDictionary.Add(nameof(article.SourceId), articleDto.SourceId);
        //    //}

        //    //if (patchDictionary.Any())
        //    //{
        //    //    var entry = _context.Entry(article);
        //    //    entry.CurrentValues.SetValues(patchDictionary);
        //    //    entry.State = EntityState.Modified; //auto
        //    //    await _context.SaveChangesAsync(token);

        //    //}
        //}
        
        //public async Task AddArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        //{
        //    articleDto.Id = Guid.NewGuid();
        //    articleDto.CreatedAt = DateTime.Now;

        //    var defaultSourceId = Guid.NewGuid();

        //    if (!_context.Sources.Any())
        //    {
        //        var defaultSource = new Source
        //        {
        //            Id = defaultSourceId,
        //            Name = "Default Source",
        //            DomainName = "defaultsource.com"
        //        };
        //        await _context.Sources.AddAsync(defaultSource, token);
        //    }
        //    else
        //    {
        //        defaultSourceId = (await _context.Sources.FirstOrDefaultAsync(token)).Id;
        //    }

        //    //add to database using EF
        //    await _context.Articles.AddAsync(_articleMapper.MapArticleDtoToArticle(articleDto), token);

        //    await _context.SaveChangesAsync(token);
        //}

        public async Task WebScrapArticleTextAsync(CancellationToken token)
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

        public async Task AggregateArticlesAsync(CancellationToken token)
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
    }
}
