using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileLoggerTest.Controllers
{
    [ApiController]
    [Route("api/Home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult Get()
        {
            return Ok(new { Name = "Sandesh" });
        }
    }
}
