using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.WebAPI.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing articles, including retrieving, updating, 
    /// and deleting articles. Supports filtering and pagination for article retrieval.
    /// </summary>
    /// <remarks>
    /// This controller interacts with the article service, mediator, and database context 
    /// to perform operations related to articles.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IMediator _mediator;
        private readonly GoodArticleAggregatorContext _context;

        public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger, IMediator mediator, GoodArticleAggregatorContext context)
        {
            _articleService = articleService;
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of articles based on the specified filters.
        /// </summary>
        /// <param name="sourceId">The unique identifier of the source to filter articles by. Optional.</param>
        /// <param name="minRate">The minimum rating to filter articles by. Optional.</param>
        /// <param name="currentPage">The current page number for pagination. Defaults to 1.</param>
        /// <param name="pageSize">The number of articles per page for pagination. Defaults to 12.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a paginated list of articles, 
        /// including metadata such as total articles, total pages, and the current page.
        /// </returns>
        [HttpGet]
        public IActionResult GetArticles(Guid? sourceId, double? minRate, int currentPage = 1, int pageSize = 12)
        {
            //move to query + mediator
            var articles = _context.Articles.AsQueryable();

            if (sourceId.HasValue)
            {
                articles = articles.Where(a => a.SourceId == sourceId.Value);
            }

            if (minRate.HasValue)
            {
                articles = articles.Where(a => a.Rate >= minRate.Value);
            }

            var totalArticles = articles.Count();
            var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

            var pagedArticles = articles.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                TotalArticles = totalArticles,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                Articles = pagedArticles
            };

            return Ok(response);
        }



        /// <summary>
        /// Returns a top 5 articles for today by rate desc
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a paginated list of articles, 
        /// including metadata such as total articles, total pages, and the current page.
        /// </returns>
        [HttpGet]
        [Route("favorites")]
        public IActionResult GetFavoriteArticles()
        {
            return Ok();
        }




        [HttpGet("{id}")]
        [ProducesResponseType(statusCode:StatusCodes.Status200OK, type:typeof(ArticleDto))]
        [ProducesResponseType(statusCode:StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode:StatusCodes.Status500InternalServerError, type:typeof(ErrorModel))]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            try
            {
                var data = await _articleService.GetArticleByIdAsync(id);
                if (data != null)
                {
                    return Ok(data);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving article with ID {ArticleId}", id);
                var error = new ErrorModel()
                {
                    Message = "Error retrieving article"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            
        }
        
        
        
        [HttpPut("{id}")]
        public IActionResult PutArticle(int id, [FromBody] object article)
        {
            // Logic to update an existing article
            return Ok();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchArticle(Guid id, [FromBody] UpdateArticleTextModel article)
        {
            // Logic to update an existing article

            var data = await _mediator.Send(new GetArticleByIdQuery { ArticleId = id });
            if (data != null)
            {
                //make update1
                return Ok(data);
            }

            return NotFound();
        }

        //think do we need Post and Delete methods in this controller?
        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(Guid id)
        {
            //_mediator.Send(new DeleteArticleCommand { ArticleId = id });
            //// Logic to delete an article
            return Ok();
        }
    }
}
