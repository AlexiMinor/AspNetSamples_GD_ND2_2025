using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using AspNetSamples.Core.Dto;
using AspNetSamples.Mappers;
using AspNetSamples.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace AspNetSamples.Services;

public class RssService : IRssService
{
    private readonly ISyndicationFeedReader _syndicationFeedReader;
    private readonly ILogger<RssService> _logger;
    private readonly ArticleMapper _articleMapper;

    public RssService(ArticleMapper articleMapper,
        ILogger<RssService> logger, ISyndicationFeedReader syndicationFeedReader)
    {
        _articleMapper = articleMapper;
        _logger = logger;
        _syndicationFeedReader = syndicationFeedReader;
    }

    public async Task<IEnumerable<ArticleDto>?> GetRssFeedBySourceAsync(SourceDto dto, HashSet<string> existingArticleUrls,
        CancellationToken token = default)
    {
        using (var xmlReader = XmlReader.Create(dto.RssLink))
        {
            var feed = _syndicationFeedReader.GetSyndicationFeed(xmlReader);
            if (feed == null)
            {
                _logger.LogWarning("Failed to load RSS feed");
                return null;
            }

           

            var articleInfoArray = feed.Items
                .Where(item => !existingArticleUrls.Contains(item.Id))
                .Select(item => _articleMapper.MapOnlinerSyndicationItemToArticleDto(item, Guid.NewGuid(), dto.Id))
                .ToArray();
            _logger.LogInformation($"New articles found: {articleInfoArray.Length}");

            foreach (var articleDto in articleInfoArray)
            {
                var imageUrl = Regex.Match(articleDto.Description, "<img[^>]+src=\"([^\">]+)\"").ToString();
                articleDto.DescriptionPictureUrl = imageUrl;
            }


            return articleInfoArray;
        }
    }
}