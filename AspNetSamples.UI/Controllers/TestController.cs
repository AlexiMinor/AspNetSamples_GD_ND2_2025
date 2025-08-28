using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Physical;

namespace AspNetSamples.UI.Controllers;

//public class Test1 : Controller
//{

//}

//public class Test2Controller
//{

//}

//[Controller]
//public class Test3
////{

//}

public class TestController : Controller
{
    
    private readonly ILifeTimeSampleService _lifeTimeSampleService1;
    private readonly ILifeTimeSampleService _lifeTimeSampleService2;


    public TestController(ILifeTimeSampleService lifeTimeSampleService1, 
        ILifeTimeSampleService lifeTimeSampleService2)
    {
        _lifeTimeSampleService1 = lifeTimeSampleService1;
        _lifeTimeSampleService2 = lifeTimeSampleService2;
    }

    [HttpGet]
    [TypeFilter(typeof(CustomResponseResourceFilter))]
    //[ServiceFilter(typeof(CustomResponseResourceFilter))]
    public IActionResult Index(int id)
    {
        var result1 = _lifeTimeSampleService1.Test();
        var result2 = _lifeTimeSampleService2.Test();
        
        
        
        return Ok(new
        {
            result1 = new
            {
                Transient = result1.Item1,
                Scoped = result1.Item2,
                Singleton = result1.Item3
            },
            result2 = new { Transient = result2.Item1, Scoped = result2.Item2, Singleton = result2.Item3 }
        });
    }

    [HttpPost]
    public IActionResult Data(int id)
    {
        return Content(id.ToString());
    }

    [HttpGet]
    public IActionResult Do()
    {

        //return Content("blablabla"); //return string
        //return Empty; //void 
        //return NoContent(); //204 - useful

        //return File();
        //return new FileContentResult();

        //return new VirtualFileResult();
        //return new PhysicalFileResult();
        //return new FileStreamResult();

        //often used results
        //return StatusCode(200);
        //return Unauthorized(); //401 - usually filters or middleware
        //return NotFound(); //404
        ////return new NotFoundObjectResult();
        ////return BadRequest() - 400
        return Ok(new object()); //200
        //return Created(); // API, 201
        //return Accepted();//202
        //return Json();
        //return PartialView()
        //return Redirect() //-> redirect

        //return View(); //return HTML view rendered by Razor Engine 
        //expect View placed in specific place OR name
    }

    [HttpGet]
    public IActionResult Test(string name, int id)
    {
        return Ok(new {name, id});
    }

    [HttpGet]
    public IActionResult Test2(string name, int id)
    {
        return View();
    }

}

