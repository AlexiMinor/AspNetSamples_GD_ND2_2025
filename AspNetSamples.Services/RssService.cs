using System.ServiceModel.Syndication;
using System.Xml;
using AspNetSamples.Core.Dto;
using AspNetSamples.Mappers;
using AspNetSamples.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace AspNetSamples.Services;

public class RssService : IRssService
{
    private readonly IArticleService _articleService;
    private readonly ILogger<RssService> _logger;
    private readonly ArticleMapper _articleMapper;

    public RssService(IArticleService articleService,
        ArticleMapper articleMapper,
        ILogger<RssService> logger)
    {
        _articleService = articleService;
        _articleMapper = articleMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ArticleDto>?> GetRssFeedBySourceAsync(SourceDto dto, CancellationToken token = default)
    {
        using (var xmlReader = XmlReader.Create(dto.RssLink))
        {
            var feed = SyndicationFeed.Load(xmlReader);
            if (feed == null)
            {
                _logger.LogWarning("Failed to load RSS feed");
                return null;
            }

            var existingArticleUrls = _articleService.GetExistingArticleUrls(token);
            _logger.LogDebug($"Existing articles count: {existingArticleUrls.Count}");

            var articleInfoArray = feed.Items
                .Where(item => !existingArticleUrls.Contains(item.Id))
                .Select(item => _articleMapper.MapOnlinerSyndicationItemToArticleDto(item, Guid.NewGuid(), dto.Id))
                .ToArray();
            _logger.LogInformation($"New articles found: {articleInfoArray.Length}");
            return articleInfoArray;
        }
    }
}