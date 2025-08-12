using System.Diagnostics;
using AspNetSamples.UI.Configuration;
using AspNetSamples.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; //services
        private readonly IConfiguration _configuration; 
        //constructor
        public HomeController(ILogger<HomeController> logger, 
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        //Action (& Endpoint) - every PUBLIC method
        [HttpGet]
        public IActionResult Index()
        {
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
