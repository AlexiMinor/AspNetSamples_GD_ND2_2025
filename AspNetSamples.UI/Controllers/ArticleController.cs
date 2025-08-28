using System.Collections.Concurrent;
using AspNetSamples.Core.Dto;
using AspNetSamples.Mappers;
using AspNetSamples.Models;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.UI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly ArticleMapper _articleMapper;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService,
            ISourceService sourceService,
            ArticleMapper articleMapper,
            IRssService rssService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _articleMapper = articleMapper;
            _rssService = rssService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1, int pageSize = 15)
        {
            try
            {
                var articles = await _articleService.GetArticlesByPageAsync(currentPage, pageSize, HttpContext.RequestAborted);
                _logger.LogDebug("Articles read from db");
                var count = await _articleService.TotalCountAsync(HttpContext.RequestAborted);
                _logger.LogDebug("Articles counts from db");
                var model = new ArticlesCollectionWithPaginationModel()
                {
                    Articles = articles,
                    PagingInfo = new PagingInfoModel()
                    {
                        CurrentPage = currentPage,
                        PageSize = pageSize,
                        TotalItems = count
                    }
                };
                _logger.LogDebug("Models created");
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Index action");
                throw;
            }
          
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                var model = new CreateArticleModel()
                {
                    Sources = (await _sourceService.GetAllSourcesAsync())
                        .Select(s => new SourceModel()
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList()
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Add action");
                throw;
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> Aggregate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AggregateProcessing()
        {
            try
            {
                _logger.LogInformation("Aggregation started");
                var sourcesToAggregate = await _sourceService.GetAllSourcesWithRssAsync(HttpContext.RequestAborted);
                _logger.LogDebug($"Sources to aggregate: {sourcesToAggregate.Count}");

                if (!sourcesToAggregate.Any())
                {
                    _logger.LogWarning("No sources with RSS found for aggregation");
                    return RedirectToAction("Index");
                }
                var aggregatedArticles = new ConcurrentBag<ArticleDto>();
                await Parallel.ForEachAsync(sourcesToAggregate, HttpContext.RequestAborted,
                    async (source, token) =>
                    {
                        var articles = await _rssService.GetRssFeedBySourceAsync(source, token);
                        if (articles != null && articles.Any())
                        {
                            foreach (var article in articles)
                            {
                                aggregatedArticles.Add(article);
                            }
                        }
                    });
                _logger.LogDebug($"Aggregated articles count: {aggregatedArticles.Count}");
                await _articleService.AddArticlesAsync(aggregatedArticles, HttpContext.RequestAborted);
                _logger.LogInformation("Aggregated articles added to database");
                _logger.LogInformation("Starting web scrapping for articles");
                await _articleService.AggregateArticleTextAsync(HttpContext.RequestAborted);
                _logger.LogInformation("Web scrapping completed");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during aggregation process");
                return StatusCode(500, new {ErrorMessage = ex.Message});
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> AddProcessing([FromForm] CreateArticleModel model)
        {

            if (ModelState.IsValid == false)
            {
                model.Sources = (await _sourceService.GetAllSourcesAsync())
                    .Select(s => new SourceModel()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();
                return View("Add", model);
            }

            //var _articleService = HttpContext.RequestServices.GetRequiredService<IArticleService>();

            //add cancellation token from controller context
            await _articleService.AddArticleAsync(_articleMapper.MapCreateArticleModelToArticleDto(model),
                HttpContext.RequestAborted);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ArticlePreview(Guid id)
        {
            var token = HttpContext.RequestAborted;
            var article = await _articleService.GetArticleByIdAsync(id, token);
            if (article == null)
            {
                return NotFound();
            }



            return PartialView("ArticlePreview", article);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var token = HttpContext.RequestAborted;
            var article = await _articleService.GetArticleByIdAsync(id, token);
            if (article != null)
            {
                return View(article);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cToken = HttpContext.RequestAborted;
            var articleForEdit = await _articleService.GetArticleByIdAsync(id, cToken);
            if (articleForEdit == null)
            {
                return NotFound();
            }
            var sources = await _sourceService.GetAllSourcesAsync(cToken);

            var model = new EditArticleViewModel()
            {
                EditModel = _articleMapper.MapArticleDtoToEditModel(articleForEdit),
                Sources = sources
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProcessing([FromForm] EditArticleViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            var dto = _articleMapper.MapEditArticleModelToArticleDto(model.EditModel);

            await _articleService.UpdateArticleAsync(dto, HttpContext.RequestAborted);

            return RedirectToAction("Index");
        }
    }
}
