using Microsoft.AspNetCore.Mvc;

namespace ModuleWeb.Controllers
{
    [ApiController]
    [Route("api/helloworld")]
    public class HelloWorldController: ControllerBase
    {
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello World");
        }
    }
}
