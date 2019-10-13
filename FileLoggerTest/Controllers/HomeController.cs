using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FileLoggerTest.Controllers
{
    [ApiController]
    [Route("api/Home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult Get()
        {
            _logger.LogWarning("Request for home get");
            return Ok(new { Name = "Sandesh" });
        }
    }
}
