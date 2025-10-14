using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangfireDashboardController : ControllerBase
    {
       
        public HangfireDashboardController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            //add auth headers to redirect
            return RedirectPermanent("/hangfire");
        }
    }
}
