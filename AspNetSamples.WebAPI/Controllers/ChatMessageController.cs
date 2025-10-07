using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMessages(Guid conversationId)
        {
            return Ok(new { Message = "This is a placeholder for chat messages." });
        }


        [HttpPost]
        public IActionResult PostMessage([FromBody] object message)
        {
            return Ok(new { Status = "Message received." });
        }

        [HttpGet("main-page")]
        public IActionResult GetMainPageData()
        {
            return Ok(new { Message = "This is a placeholder for main page data." });
        }
    }
}
