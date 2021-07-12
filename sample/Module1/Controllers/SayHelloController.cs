using Microsoft.AspNetCore.Mvc;

namespace Module1.Controllers
{
    [ApiController]
    [Route("api/module")]
    public class ModuleController: Controller
    {
        [HttpGet]
        public IActionResult GetModule()
        {
            return Ok("Module1");
        }
    }
}
