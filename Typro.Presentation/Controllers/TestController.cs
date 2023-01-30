using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Typro.Presentation.Controllers;

[ApiController]
[Route("test")]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Test()
    {
        return Ok("pong");
    }
}