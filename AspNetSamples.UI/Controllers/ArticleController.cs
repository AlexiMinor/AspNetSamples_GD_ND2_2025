using System.Collections.Concurrent;
using System.Diagnostics;
using AspNetSamples.Core.Dto;
using AspNetSamples.Mappers;
using AspNetSamples.Models;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.UI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArticleRateService _articleRateService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly ArticleMapper _articleMapper;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService,
            ISourceService sourceService,
            ArticleMapper articleMapper,
            IRssService rssService, ILogger<ArticleController> logger, IArticleRateService articleRateService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _articleMapper = articleMapper;
            _rssService = rssService;
            _logger = logger;
            _articleRateService = articleRateService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1, int pageSize = 12)
        {
            try
            {
                var articles = await _articleService.GetArticlesByPageAsync(currentPage, pageSize, HttpContext.RequestAborted);
                _logger.LogDebug("Articles read from db");
                var count = await _articleService.TotalCountAsync(HttpContext.RequestAborted);
                _logger.LogDebug("Articles counts from db");
                var model = new ArticlesCollectionWithPaginationModel()
                {
                    IsUserAdmin = User.IsInRole("Admin"),
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Aggregate()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AggregateProcessing()
        {
            try
            {
                await _articleService.AggregateArticlesAsync(HttpContext.RequestAborted);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during aggregation process");
                return StatusCode(500, new { ErrorMessage = ex.Message });
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
            //await _articleService.AddArticleAsync(_articleMapper.MapCreateArticleModelToArticleDto(model),
            //    HttpContext.RequestAborted);

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
        public async Task<IActionResult> Rate(Guid id)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var token = HttpContext.RequestAborted;
                await _articleRateService.RateArticlesInParallelAsync(token);

                sw.Stop();
                return Ok(new { Duration = sw.ElapsedMilliseconds });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
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

            //await _articleService.UpdateArticleAsync(dto, HttpContext.RequestAborted);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("api/articles/count")]
        public async Task<IActionResult> GetArticlesCount()
        {
            var count = await _articleService.GetArticlesCountAsync(HttpContext.RequestAborted);
            return Json(new { Count = count });
        }
    }

}
