using AspNetSamples.Core.Dto;
using AspNetSamples.Database;
using AspNetSamples.Database.Entities;
using AspNetSamples.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Services
{
    public class ArticleService : IArticleService
    {
        private readonly GoodArticleAggregatorContext _context;
        public ArticleService(GoodArticleAggregatorContext context)
        {
            _context = context;
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token = default)
        {
           var article = await _context.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .SingleOrDefaultAsync(article => article.Id.Equals(id), token);
           return article == null
               ? null
               : new ArticleDto
               {
                   Id = article.Id,
                   Title = article.Title,
                   Description = article.Description,
                   Text = article.Content,
                   CreatedAt = article.CreatedAt,
                   SourceName = article.Source.Name,
                   SourceId = article.SourceId,
                   Rate = 0
               };
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
                .Select(article => new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Text = article.Content,
                    CreatedAt = article.CreatedAt,
                    SourceName = article.Source.Name,
                    SourceId = article.SourceId,
                    Rate = 0
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<int> TotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Articles
                .CountAsync();
                
        }

        public async Task UpdateArticleAsync(ArticleDto articleDto, CancellationToken token = default)
        {
            var article = await _context.Articles
                //.AsNoTracking()
                .SingleOrDefaultAsync(article => article.Id.Equals(articleDto.Id), cancellationToken: token);
            
            if (article == null) 
                return;

            //await _context.Articles.Update(article)
            
            
            article.Title = articleDto.Title;
            article.Description = articleDto.Description;
            article.Content = articleDto.Text;
            article.SourceId = articleDto.SourceId;
            //_context.Update(article);
            await _context.SaveChangesAsync(token);
            
            //var patchDictionary = new Dictionary<string, object>();
            ////not best practice, but for simplicity
            //if (!article.Content.Equals(articleDto.Text))
            //{
            //    patchDictionary.Add(nameof(article.Content), articleDto.Text);
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
            await _context.Articles.AddAsync(new Article
            {
                Id = articleDto.Id,
                Title = articleDto.Title,
                Description = articleDto.Description,
                Content = articleDto.Text,
                OriginUrl = "https://example.com/article/" + articleDto.Id,
                CreatedAt = articleDto.CreatedAt,
                SourceId = defaultSourceId

            }, token);

            await _context.SaveChangesAsync(token);
        }

        public async Task AddArticlesAsync(IEnumerable<ArticleDto> articleDto, CancellationToken token = default)
        {
            var articles = articleDto.Select(article => new Article
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Content = article.Text,
                CreatedAt = article.CreatedAt,
                OriginUrl = "https://example.com/article/" + article.Id,

                SourceId = article.SourceId
            }).ToArray();

            await _context.Articles.AddRangeAsync(articles, token);
            await _context.SaveChangesAsync(token);
        }
    }
}
