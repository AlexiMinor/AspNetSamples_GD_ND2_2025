using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetSamples.UI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;

        public ArticleController(IArticleService articleService, ISourceService sourceService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
        }

        //[Route("{controller}/{action}/{pageSize=15}")]


        [HttpGet]
        //[Route("/article-list/{pageSize=15}")]
        public async Task<IActionResult> Index(int currentPage=1, int pageSize=3)
        {
            var articles = await _articleService.GetArticlesByPageAsync(currentPage, pageSize, HttpContext.RequestAborted);
            var count = await _articleService.TotalCountAsync(HttpContext.RequestAborted);
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
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProcessing([FromForm]CreateArticleModel model)
        {
            var _articleService = HttpContext.RequestServices.GetRequiredService<IArticleService>();
            //add cancellation token from controller context
            await _articleService.AddArticleAsync(ConvertCreateArticleModelToArticle(model), HttpContext.RequestAborted);

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
            var sources = await _sourceService.GetAllAsync(cToken);

            var model = new EditArticleModel()
            {
                Id = articleForEdit.Id,
                Title = articleForEdit.Title,
                Description = articleForEdit.Description,
                Text = articleForEdit.Text,
                SourceId = articleForEdit.SourceId,
                Sources = sources
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProcessing([FromForm] EditArticleModel model)
        {
            
            var dto = new ArticleDto()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Text = model.Text,
                SourceId = model.SourceId
            };
            
            await _articleService.UpdateArticleAsync(dto, HttpContext.RequestAborted);

            return RedirectToAction("Index");
        }

        //TO BE REWORKED IN FUTURE
        private static ArticleDto ConvertCreateArticleModelToArticle(CreateArticleModel model)
        {
            return new ArticleDto()
            {
                //implement 
                Text = model.Text,
                Title = model.Title,
                Description = model.Description,

            };
        }
    }
}
