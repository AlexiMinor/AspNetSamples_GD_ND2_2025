using System.Diagnostics;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Configuration;
using AspNetSamples.UI.Filters;
using AspNetSamples.UI.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<HomeController> _logger; //services
        private readonly IConfiguration _configuration; 
        //constructor
        public HomeController(ILogger<HomeController> logger, 
            IConfiguration configuration, IArticleService articleService)
        {
            _logger = logger;
            _configuration = configuration;
            _articleService = articleService;
        }

        //Action (& Endpoint) - every PUBLIC method
        [HttpGet]
        [LastVisitResourceFilter]
        public async Task<IActionResult> Index()
        {
            ////should be placed in app startup
            //RecurringJob.AddOrUpdate("article-aggregation",
            //    ()=> _articleService.AggregateArticlesAsync(CancellationToken.None),
            //    "0 * * * *"
            //    );

            //var backJobId = BackgroundJob.Enqueue(
            //    () => _logger.LogDebug("Fire-and-forget job executed")
            //    );

            //var cJobId = BackgroundJob.ContinueJobWith(
            //    backJobId,
            //    () => _logger.LogDebug("Continuation job executed")
            //    );

            _logger.LogInformation("Home page loaded successfully");
            //var secretValue = _configuration["AppSettings:VerySecretValue"];
            //var section = _configuration.GetSection("ConnectionStrings");
            //var children = _configuration.GetChildren();
            //var sectionChildren = section.GetChildren();
            //var sData = _configuration["SecretData"];
            //var pageInfo = _configuration.GetSection("AppSettings:PageConfigInfo").Get<PageConfigInfo>();

            //var configReloadToken = _configuration.GetReloadToken();
            ////generate reread configuration 
            //_configuration();

            return View();
        }

        [HttpGet]
        public IActionResult Page404()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Policy = "AgeLimited")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public string Test(string name, TestEnum enumData)
        {
            var isP1Exists = HttpContext.Request.Query.TryGetValue("p1", out var data);
            return $"Hello world {name}";
        }

        [HttpPost]
        public string TestProcessing([FromQuery] MyClass a)
        {
            return "Hello world Post";
        }

        private void SourceValidatorProcessing()
        {
            var isP1Exists = HttpContext.Request.Query.TryGetValue("p1", out var data);

        }

        public enum TestEnum
        {
            Ok,
            NotOk,
            SomethingDifferent
        }

        public class MyClass
        {
            public string A { get; set; }
            public string B { get; set; }
        }
    }
}
