using AspNetSamples.Core.Dto;
using AspNetSamples.Database;
using AspNetSamples.Database.Entities;
using AspNetSamples.Mappers;
using AspNetSamples.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetSamples.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ArticleMapper _articleMapper;
        private readonly ILogger<ArticleService> _logger;
        private readonly GoodArticleAggregatorContext _context;

        private readonly IWebParserService _webParserService;

        public ArticleService(GoodArticleAggregatorContext context, ArticleMapper articleMapper, IWebParserService webParserService, ILogger<ArticleService> logger)
        {
            _context = context;
            _articleMapper = articleMapper;
            _webParserService = webParserService;
            _logger = logger;
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token = default)
        {
            var article = await _context.Articles
                 .AsNoTracking()
                 .Include(article => article.Source)
                 .SingleOrDefaultAsync(article => article.Id.Equals(id), token);
            return article == null
                ? null
                : _articleMapper.MapArticleToArticleDto(article);
        }

        public async Task<List<ArticleDto>> GetArticlesByPageAsync(int currentPage, int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _context.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .OrderBy(article => article.Title) //todo change to smth different, like CreatedAt
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(article => _articleMapper.MapArticleToArticleDto(article))
                .ToListAsync(cancellationToken);
        }

        public async Task<int> TotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Articles
                .CountAsync(cancellationToken: cancellationToken);

        }

        public async Task UpdateArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        {
            var article = await _context.Articles
                //.AsNoTracking()
                .SingleOrDefaultAsync(article => article.Id.Equals(articleDto.Id), cancellationToken: token);

            if (article == null)
                return;

            article.Title = articleDto.Title;
            article.Description = articleDto.Description;
            article.Content = articleDto.Content;
            article.SourceId = articleDto.SourceId;
            //_context.Update(article);
            await _context.SaveChangesAsync(token);

            //var patchDictionary = new Dictionary<string, object>();
            ////not best practice, but for simplicity
            //if (!article.Content.Equals(articleDto.Content))
            //{
            //    patchDictionary.Add(nameof(article.Content), articleDto.Content);
            //}

            //if (!article.Title.Equals(articleDto.Title))
            //{
            //    patchDictionary.Add(nameof(article.Title), articleDto.Title);
            //}

            //if (!article.Description.Equals(articleDto.Description))
            //{
            //    patchDictionary.Add(nameof(article.Description), articleDto.Description);
            //}

            //if (!article.SourceId.Equals(articleDto.SourceId))
            //{
            //    patchDictionary.Add(nameof(article.SourceId), articleDto.SourceId);
            //}

            //if (patchDictionary.Any())
            //{
            //    var entry = _context.Entry(article);
            //    entry.CurrentValues.SetValues(patchDictionary);
            //    entry.State = EntityState.Modified; //auto
            //    await _context.SaveChangesAsync(token);

            //}


        }

        public Task DeleteArticleAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        {
            articleDto.Id = Guid.NewGuid();
            articleDto.CreatedAt = DateTime.Now;

            var defaultSourceId = Guid.NewGuid();

            if (!_context.Sources.Any())
            {
                var defaultSource = new Source
                {
                    Id = defaultSourceId,
                    Name = "Default Source",
                    DomainName = "defaultsource.com"
                };
                await _context.Sources.AddAsync(defaultSource, token);
            }
            else
            {
                defaultSourceId = (await _context.Sources.FirstOrDefaultAsync(token)).Id;
            }

            //add to database using EF
            await _context.Articles.AddAsync(_articleMapper.MapArticleDtoToArticle(articleDto), token);

            await _context.SaveChangesAsync(token);
        }

        public async Task AddArticlesAsync(IEnumerable<ArticleDto> articleDto, CancellationToken token = default)
        {
            var articles = articleDto
                .Select(article => _articleMapper
                    .MapArticleDtoToArticle(article))
                .ToArray();

            await _context.Articles.AddRangeAsync(articles, token);
            await _context.SaveChangesAsync(token);
        }

        public HashSet<string> GetExistingArticleUrls(CancellationToken token)
        {
            return _context.Articles.AsNoTracking().Select(article => article.OriginUrl).ToHashSet();
        }

        public async Task AggregateArticleTextAsync(CancellationToken token)
        {
            try
            {
                var articles = await _context.Articles
                    .Where(article => string.IsNullOrEmpty(article.Content))
                    .ToArrayAsync(token);
          
                foreach (var article in articles)
                {
                    var articleText = await _webParserService.ParseArticlesAsync(article.OriginUrl, token);
                    article.Content = articleText;
                    await _context.SaveChangesAsync(token);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during articles web scrapping");
                throw;
            }

        }
    }
}
