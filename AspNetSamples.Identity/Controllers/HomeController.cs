using AspNetSamples.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace AspNetSamples.Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<MyCustomUser> _signInManager;
        private readonly UserManager<MyCustomUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public HomeController(ILogger<HomeController> logger, 
            SignInManager<MyCustomUser> signInManager, 
            UserManager<MyCustomUser> userManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
