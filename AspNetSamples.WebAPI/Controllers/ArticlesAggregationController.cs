using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.WebAPI.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    /// <summary>
    /// Controller for aggregating articles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesAggregationController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesAggregationController(IArticleService articleService, ILogger<ArticlesController> logger, IMediator mediator, GoodArticleAggregatorContext context)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AggregateArticles()
        {
            await _articleService.AggregateArticlesAsync(HttpContext.RequestAborted);
            return Ok();
        }
    }
}
